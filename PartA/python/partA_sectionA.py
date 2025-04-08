import heapq

"""
 ע"מ לשמור על סיבוכיות כמה שיותר קטנה
נרוץ על כל המקטעים של הקובץ ונכניס כל סוג שגיאה עם מספר הפעמים שמופיעה למילון 
אח"כ נבנה מזה ערימת מקסימום 
אח"כ ניקח את האינדקס הראשון של המקסימום ונשים בערימה חדשה 
 והעדיפות תהיה לפי מספר פעמים שמופיעה השגיאה
בכל שלב שנבצע שליפת המקסימום מהערימה החדשה נכניס את שני הבנים שלו לערימה 
"""


file_path="logs.txt"
def split_file(file_path, section_size):
    with open(file_path, "r") as file: #פתיחת קובץ לקריאה
        section = []
        for line in file:
            section.append(line)
            if len(section) == section_size:
                yield section
                section = []
        if section:  # החלק האחרון אם נשאר
            yield section
"""
סבוכיות כללית:
זמן: o(m) כאשר m שווה למספר השורות בקובץ
מקום: o(section_size) כל פעם נשמר בזכרון מקטע אחד בגודל קבוע
"""



dic = {}
#כל פעם כשחוזר yield, הfor יפעל עד שנגמר השורות ואז יבצע את המשך הפונקציה split ויחזיר yield הבא וכן הלאה
for section in split_file("logs.txt", 100000):
    for line in section:
        if "Error:" in line:
            num_error = line.split("Error:")[1].split('"')[0]
            dic[num_error] = dic.get(num_error, 0) + 1 #אם אינו קיים במילון ניצור חדש עם ערך התחלתי 0 ונוסיף לו 1
"""
סבוכיות כללית:
זמן: o(m) כאשר m שווה למספר השורות בקובץ
o(1) בממוצע הכנסה למילון
סה"כ o(m) + בממוצע o(m)
מקום: o(section_size) כל פעם נשמר בזכרון מקטע אחד בגודל קבוע
בנוסף עבור המילון o(k) כאשר k מספר סוגי השגיאות השונים
"""

#כיון שהספרייה תומכת בערימת מינימום נהפוך את הערכים לשליליים וכך נוכל לעבוד עם זה כערימת מקסימום
heap = [(-count_error, num_error) for num_error, count_error in dic.items()]
heapq.heapify(heap)  # o(k)בנית ערימה ב
"""
סבוכיות כללית:
זמן: o(k) כאשר k מספר סוגי השגיאות השונים
הוכחה עבור בניית ערימה כגודל מספר האיברים היא הכוחה שניתן להסביר בקלות
מקום: o(k) ערימה בגודל k
"""

i = 0
new_heap = []  # נכניס קודם כל את הראשון שהוא בהכרח המקסימלי
if (heap):
    item = heap[0]
    heapq.heappush(new_heap, (item[0], 0)) #0- המיקום הראשון

print("enter n:")
n = int(input())

"""
הלולאה עוברת N פעמים כדי למצוא את N המקסימלים
ניתן להוכיח בקלות כי מספר האיברים שיכנסו מקסימום לערימה יהיה 3N
כל פעם אנחנו נוציא איבר מקסימלי ונכניס במקומו את 2 ילדיו (אם יש)
זה יקרה N פעמים ולכן לכל היותר יהיה 3N איברים אפילו פחות
עבור כל N עוד 2 ילדים סה"כ 3N
ולכן כל פעולה של הוצאה והכנסה תיקח LG3N=LGN
"""
while n > 0:
    if new_heap:
        item = heapq.heappop(new_heap)  # מיקום בערימה המקורית
        i = item[1]
        print("code_error:", heap[i][1], "count_error:", -(heap[i][0]))
        left = i * 2 + 1
        if left < len(heap): #רק אם המיקום לא חורג
            item = heap[left]
            heapq.heappush(new_heap, (item[0], left))
        right = i * 2 + 2
        if right < len(heap):
            item = heap[right]
            heapq.heappush(new_heap, (item[0], right))
        n -= 1
    else:
        break

"""
סבוכיות כללית:
זמן: o(NLGN) כאשר N מספר סוגי השגיאות הרצוי
מקום: o(N) ערימה בגודל מקסימום 3N
"""



"""
סבוכיות סה"כ:
m =מספר שורות בקובץ
k =מספר שגיאות שונות  
N= מספר שגיאות רצויות
section_size= מספר קבוע לחילוק הקבצים לפי מספר שורות
זמן: o(m) בממוצע + o(k) + o(NlgN)
מקום: o(k) + o(section_size)
"""
