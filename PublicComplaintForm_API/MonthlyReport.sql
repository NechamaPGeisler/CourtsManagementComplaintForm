-- דוח פניות חודשי - השוואה לחודש קודם ולאותו חודש בשנה שעברה
-- הנחה: טבלת Complaints עם עמודות: Id, Department, CreatedDate

DECLARE @CurrentMonth DATE = DATEFROMPARTS(YEAR(GETDATE()), MONTH(GETDATE()), 1);
DECLARE @PreviousMonth DATE = DATEADD(MONTH, -1, @CurrentMonth);
DECLARE @SameMonthLastYear DATE = DATEADD(YEAR, -1, @CurrentMonth);

SELECT 
    d.DepartmentName AS Department,
    ISNULL(cur.ComplaintCount, 0) AS CurrentMonthCount,
    ISNULL(prev.ComplaintCount, 0) AS PreviousMonthCount,
    ISNULL(lastyear.ComplaintCount, 0) AS SameMonthLastYearCount
FROM Departments d
LEFT JOIN (
    SELECT Department, COUNT(*) AS ComplaintCount
    FROM Complaints
    WHERE CreatedDate >= @CurrentMonth AND CreatedDate < DATEADD(MONTH, 1, @CurrentMonth)
    GROUP BY Department
) cur ON d.DepartmentName = cur.Department
LEFT JOIN (
    SELECT Department, COUNT(*) AS ComplaintCount
    FROM Complaints
    WHERE CreatedDate >= @PreviousMonth AND CreatedDate < @CurrentMonth
    GROUP BY Department
) prev ON d.DepartmentName = prev.Department
LEFT JOIN (
    SELECT Department, COUNT(*) AS ComplaintCount
    FROM Complaints
    WHERE CreatedDate >= @SameMonthLastYear AND CreatedDate < DATEADD(MONTH, 1, @SameMonthLastYear)
    GROUP BY Department
) lastyear ON d.DepartmentName = lastyear.Department
ORDER BY d.DepartmentName;

/*
הסבר השאילתה:
=============
1. מגדירים 3 תאריכי ייחוס: תחילת החודש הנוכחי, תחילת החודש הקודם, ותחילת אותו חודש בשנה שעברה.
2. משתמשים ב-LEFT JOIN עם 3 תת-שאילתות שכל אחת סופרת פניות לפי מחלקה בטווח תאריכים שונה.
3. LEFT JOIN מבטיח שמחלקות ללא פניות עדיין יופיעו בדוח (עם 0).

שיפור ביצועים:
==============
1. אינדקס מורכב על (CreatedDate, Department) - מאפשר סריקה יעילה לפי טווח תאריכים וקיבוץ לפי מחלקה.
   CREATE INDEX IX_Complaints_CreatedDate_Department ON Complaints(CreatedDate, Department);

2. אם הטבלה גדולה מאוד, ניתן ליצור Indexed View שמחשב מראש את הספירות החודשיות.

3. שימוש בפרטיציות (Partitioning) על עמודת CreatedDate לפי חודשים - מאפשר לסרוק רק את הפרטיציות הרלוונטיות.

4. שמירת תוצאות הדוח ב-Cache ברמת האפליקציה (MemoryCache) כיוון שהנתונים לא משתנים בתדירות גבוהה.
*/
