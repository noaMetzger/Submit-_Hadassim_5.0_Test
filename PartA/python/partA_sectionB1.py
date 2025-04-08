import pandas as pd

def load_csv_file(file_path):
    try:
        df = pd.read_csv(file_path)
        print("The CSV file was loaded successfully.")
        return df
    except Exception as e:
        print(f"Error loading CSV file: {e}")
        return pd.DataFrame() #מחזיר קובץ ריק כדי למנוע בעיות בהמשך

# בדיקות תקינות
def validation(df):
    # המרת עמודת תאריך
    df['timestamp'] = pd.to_datetime(df['timestamp'], errors='coerce', dayfirst=True)

    # הסרת שורות לא תקינות
    df = df.dropna(subset=['timestamp', 'value']).copy() #כיון שהאינדקסים שונים אחרי הורדת שורות מבצעים העתקה

    # המרת ערכים מספריים
    df['value'] = pd.to_numeric(df['value'], errors='coerce')
    df = df.dropna(subset=['value']).copy()

    # הסרת כפילויות
    df=remove_duplicates(df)

    return df

def remove_duplicates(df):
    # מציאת רשומות כפולות לפי 'timestamp'
    duplicate_mask = df.duplicated(subset=['timestamp'], keep=False)
    # בדיקת סתירות בין הערכים באותו 'timestamp'
    #אם יש שני שורות או יותר לאותו תאריך ויש לו יותר מערך אחד יחודי זה סתירה ולכן נמחק
    conflicting_mask = df[duplicate_mask].groupby('timestamp')['value'].transform('nunique') > 1
    #מחזירה לאינדקסים המקוריים כדי שלא יהיה בעיה עם המחיקות- אם לא נמצא כתוב שם FALSE
    conflicting_mask = conflicting_mask.reindex(df.index, fill_value=False)
    # מחיקת רשומות עם סתירות
    df = df[~conflicting_mask]
    return df


def calculate_hourly_average(df):
    # משנה את פורמט התאריך כך שיוצג כשעה עגולה בלי דקות ושניות
    df['timestamp'] = df['timestamp'].dt.floor('h')

    # חישוב ממוצע לפי שעה עגולה עם עיגול ל-2 ספרות ב- average
    hourly_avg = df.groupby('timestamp')['value'].mean().round(2).reset_index()

    # שינוי שמות עמודות
    hourly_avg.rename(columns={'timestamp': 'Start Time', 'value': 'Average'}, inplace=True)

    return hourly_avg

# שמירת קובץ הממוצעים לתיקיית הפרויקט
def save_to_csv(df, output_file_name):
    try:
        output_path = output_file_name
        # לא נשמור את האינדקס בקובץ ולכן זה FALSE
        df.to_csv(output_path, index=False)
        print(f"The result was successfully saved at {output_path}")
    except Exception as e:
        print(f"Error saving the file: {e}")

# הרצה
file_path = "time_series(1).csv"
df = load_csv_file(file_path)

if not df.empty:
    df = validation(df)
    hourly_avg = calculate_hourly_average(df)

    if not hourly_avg.empty:
        # שמירת הממוצעים לקובץ
        save_to_csv(hourly_avg, "hourly_averages.csv")
