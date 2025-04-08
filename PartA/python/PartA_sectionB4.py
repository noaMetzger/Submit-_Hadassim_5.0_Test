import pandas as pd
import os
from concurrent.futures import ThreadPoolExecutor
import shutil

def load_parquet_file(file_path):
    try:
        df = pd.read_parquet(file_path)
        print("The PARQUET file was loaded successfully.")
        return df
    except Exception as e:
        print(f"Error loading PARQUET file: {e}")
        return pd.DataFrame() #מחזיר קובץ ריק כדי למנוע בעיות בהמשך

# פונקציה לטעינת נתונים עם ניקוי בסיסי
def clean_data_parquet(df):
    # המרה לפורמט תאריך וניקוי נתונים
    df['timestamp'] = pd.to_datetime(df['timestamp'], errors='coerce', dayfirst=True)
    df = df.dropna(subset=['timestamp', 'value']).copy()
    df['value'] = pd.to_numeric(df['value'], errors='coerce')
    df = df.dropna(subset=['value']).copy()
    df = remove_duplicates(df)
    return df

def remove_duplicates(df):
    # מציאת רשומות כפולות לפי 'timestamp'
    duplicate_mask = df.duplicated(subset=['timestamp'], keep=False)
    # בדיקת סתירות בין הערכים באותו 'timestamp'
    conflicting_mask = df[duplicate_mask].groupby('timestamp')['value'].transform('nunique') > 1
    conflicting_mask = conflicting_mask.reindex(df.index, fill_value=False)
    # מחיקת רשומות עם סתירות
    df = df[~conflicting_mask]
    return df

def split_data_by_day_parquet(df):
    # יצירת תיקיית קבצים יומיים אם היא לא קיימת
    os.makedirs("daily_section_parquet", exist_ok=True)

    # פיצול הנתונים לפי ימים (מתעלמים מהשעות)
    for date, group in df.groupby(df['timestamp'].dt.date):
        group.to_parquet(f"daily_section_parquet/data_{date}.parquet", index=False)

# פונקציה לחישוב ממוצע שעתיים עבור כל קובץ יומי בפורמט Parquet
def calculate_hourly_average_parquet(file_name):
    path = os.path.join("daily_section_parquet", file_name)
    df_day = pd.read_parquet(path)

    # המרה לפורמט datetime וניקוי נתונים
    df_day['timestamp'] = pd.to_datetime(df_day['timestamp'], errors='coerce')
    df_day['value'] = pd.to_numeric(df_day['value'], errors='coerce')
    df_day = df_day.dropna(subset=['timestamp', 'value'])

    # התאמת עמודת timestamp כך שתכיל גם תאריך וגם שעה עגולה
    df_day['timestamp'] = df_day['timestamp'].dt.floor('h')

    # חישוב ממוצע לפי תאריך ושעה עגולה עם עיגול ל-2 ספרות
    hourly_avg = df_day.groupby('timestamp')['value'].mean().round(2).reset_index()

    # שינוי שמות העמודות בקובץ הסופי
    hourly_avg.rename(columns={'timestamp': 'Start Time', 'value': 'Average'}, inplace=True)

    return hourly_avg

# פונקציה לעיבוד מקבילי ואיחוד התוצאות
def compute_and_merge_parquet():
    # יצירת רשימת קבצים בתיקיית daily_section_parquet
    section_files = [f for f in os.listdir("daily_section_parquet") if os.path.isfile(os.path.join("daily_section_parquet", f))]

    # עיבוד מקבילי של כל הקבצים
    all_results = []
    with ThreadPoolExecutor() as executor:
        processed_files = list(executor.map(calculate_hourly_average_parquet, section_files))
        all_results.extend(processed_files)

    # איחוד כל התוצאות לקובץ אחד
    final_df = pd.concat(all_results)
    final_df.sort_values(by='Start Time', inplace=True)
    save_to_csv(final_df, "final_output_parquet.csv")

def save_to_csv(df, output_file):
    try:
        df.to_csv(output_file, index=False)
        print(f"The result was successfully saved at {output_file}")
    except Exception as e:
        print(f"Error saving the file: {e}")

# הרצת הקוד כולו
# מחיקת תיקיות קיימות (אם קיימות)
shutil.rmtree("daily_section_parquet", ignore_errors=True)

# שלב 1: קריאת קובץ המקור וניקוי ראשוני
file_path = "time_series.parquet"
df= load_parquet_file(file_path)
if not df.empty:
    df = clean_data_parquet(df)
    # שלב 2: פיצול לקבצים יומיים
    split_data_by_day_parquet(df)
    # שלב 3: עיבוד מקבילי של חישוב ממוצעים ואיחוד סופי
    compute_and_merge_parquet()


"""
יתרונות הPARQUET:

מבנה עמודות:
הפרמטרים של המידע בפורמט הזה מוצגים בצורת עמודות,
כך שאפשר לקרוא רק את העמודות שצריך בלי לבזבז זמן או משאבים מיותרים. זה פשוט חוסך הרבה בהתמודדות עם נתונים.

דחיסות גבוהה:
הוא מנצל את המקום באחסון, כך שהוא לוקח פחות שטח בהשוואה לפורמטים אחרים.

מהירות:
כשעובדים עם כמויות נתונים ענקיות, PARQUET מאפשר לעבד את הנתונים מהר יותר, בלי שצריך לטפל בהם בצורה איטית ומסורבלת.

דיוק נתונים:
הפורמט שומר במדויק על נתונים, בין אם זה תאריך או מספר.

תמיכה במידע מורכב:
 מתאים גם לעבוד עם סוגי נתונים מתקדמים יותר ועם מערכות שמטפלות בביג דאטה
זה הופך אותו לבחירה מאד יעילה כשיש מידע מורכב שצריך לנהל.


*************

דרך הקוד ניתן לראות עד כמה קריאה מקובץ PARQUET הייתה יעילה בהשוואה לקריאה מקובץ CSV,
 גם מבחינת זמן העיבוד וגם מבחינת ניצול משאבים
"""