import pandas as pd
import os
from concurrent.futures import ThreadPoolExecutor
import shutil

def load_csv_file(file_path):
    try:
        df = pd.read_csv(file_path)
        print("The CSV file was loaded successfully.")
        return df
    except Exception as e:
        print(f"Error loading CSV file: {e}")
        return pd.DataFrame() #מחזיר קובץ ריק כדי למנוע בעיות בהמשך

def clean_data(df):
    # המרה לפורמט תאריך וניקוי נתונים
    df['timestamp'] = pd.to_datetime(df['timestamp'], errors='coerce', dayfirst=True)
    df = df.dropna(subset=['timestamp', 'value']).copy()
    df['value'] = pd.to_numeric(df['value'], errors='coerce')
    df = df.dropna(subset=['value']).copy()
    df=remove_duplicates(df)
    return df

def remove_duplicates(df):
    # מציאת רשומות כפולות לפי 'timestamp'
    duplicate_mask = df.duplicated(subset=['timestamp'], keep=False)
    # בדיקת סתירות בין הערכים באותו 'timestamp'
    conflicting_mask = df[duplicate_mask].groupby('timestamp')['value'].transform('nunique') > 1
    #מחזירה לאינדקסים המקוריים כדי שלא יהיה בעיה עם המחיקות- אם לא נמצא כתוב שם FALSE
    conflicting_mask = conflicting_mask.reindex(df.index, fill_value=False)
    # מחיקת רשומות עם סתירות
    df = df[~conflicting_mask]
    return df


def split_data_by_day(df):
    # יצירת תיקיית קבצים יומיים אם היא לא קיימת
    os.makedirs("daily_section", exist_ok=True)

    # פיצול הנתונים לפי ימים (מתעלמים מהשעות)
    for date, group in df.groupby(df['timestamp'].dt.date):
        group.to_csv(f"daily_section/data_{date}.csv", index=False)


# פונקציה לחישוב ממוצע שעתיים עבור כל קובץ יומי
def calculate_hourly_average(file_name):
    path = os.path.join("daily_section", file_name)
    df_day = pd.read_csv(path)

    # המרה לפורמט datetime וניקוי נתונים
    df_day['timestamp'] = pd.to_datetime(df_day['timestamp'], errors='coerce')
    df_day['value'] = pd.to_numeric(df_day['value'], errors='coerce')
    df_day = df_day.dropna(subset=['timestamp', 'value'])

    # התאמת עמודת timestamp כך שתכיל גם תאריך וגם שעה עגולה
    df_day['timestamp'] = df_day['timestamp'].dt.floor('h')  # תאריך מלא עם שעה עגולה

    # חישוב ממוצע לפי תאריך ושעה עגולה עם עיגול ל-2 ספרות
    hourly_avg = df_day.groupby('timestamp')['value'].mean().round(2).reset_index()

    # שינוי שמות העמודות בקובץ הסופי
    hourly_avg.rename(columns={'timestamp': 'Start Time', 'value': 'Average'}, inplace=True)

    return hourly_avg

# פונקציה לעיבוד מקבילי ואיחוד התוצאות
def compute_and_merge():
    # יצירת רשימת קבצים בתיקיית daily_section
    section_files = [f for f in os.listdir("daily_section") if os.path.isfile(os.path.join("daily_section", f))]

    # עיבוד מקבילי של כל הקבצים
    all_results = []
    with ThreadPoolExecutor() as executor:
        processed_files = list(executor.map(calculate_hourly_average, section_files))
        all_results.extend(processed_files)

    # איחוד כל התוצאות לקובץ אחד
    final_df = pd.concat(all_results)
    final_df.sort_values(by='Start Time', inplace=True)  # סידור לפי שעה בלבד
    save_to_csv(final_df,"final_output.csv" )

def save_to_csv(df, output_file):
    try:
        df.to_csv(output_file, index=False)
        print(f"The result was successfully saved at {output_file}")
    except Exception as e:
        print(f"Error saving the file: {e}")

# הרצת הקוד כולו
# מחיקת תיקיות קיימות (אם קיימות)
shutil.rmtree("daily_section", ignore_errors=True)

# שלב 1: קריאת קובץ המקור וניקוי ראשוני
file_path = "time_series(1).csv"
df= load_csv_file(file_path)
if not df.empty:
    df = clean_data(df)
    # שלב 2: פיצול לקבצים יומיים
    split_data_by_day(df)
    # שלב 3: עיבוד מקבילי של חישוב ממוצעים ואיחוד סופי
    compute_and_merge()


