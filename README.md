# PublicComplaintForm - טופס פניות הציבור

## תכולת הפרויקט

מערכת לניהול פניות ציבור הכוללת:
- **Frontend** - אפליקציית Angular (תיקיית `PublicComplaintForm/`) עם טופס רב-שלבי:
  1. סוג הפניה (בקשה/תלונה)
  2. פרטי הפונה
  3. פרטי הפניה (מהות, בית משפט, מספר תיק)
  4. העלאת מסמכים
  5. סיכום ושליחה
- **Backend** - ASP.NET Core 8 Minimal API (תיקיית `PublicComplaintForm_API/`) המספק endpoints לטיפול בנתוני הטופס

## שיטת הבנייה והשיקולים

### ארכיטקטורה
- **Minimal API** - נבחר לפשטות ומהירות פיתוח, מתאים לפרויקט עם מספר endpoints מוגדר
- **Standalone Components** - בצד הלקוח, כל קומפוננטה עצמאית (Angular Standalone)
- **Service Layer** - הפרדה בין לוגיקה עסקית (Services) למודלים (Models)

### רכיבים עיקריים
| רכיב | תפקיד |
|------|--------|
| `DatabaseService` | גישה לנתונים, שליפת בתי משפט מ-data.gov.il |
| `CaptchaService` | יצירה ואימות של CAPTCHA |
| `SanitizingService` | ניקוי קלט מקוד זדוני |
| `FormHandlerService` | ניהול מצב הטופס בצד הלקוח |
| `CourtHandlerService` | שליפת רשימת בתי משפט |

## אבטחה

- **CAPTCHA** - אימות שהמשתמש אנושי לפני שליחת הטופס
- **Input Sanitization** - ניקוי HTML/XSS מכל הקלטים באמצעות HtmlSanitizer
- **Antiforgery Token** - הגנה מפני CSRF (X-CSRF-TOKEN header)
- **Security Headers** - Referrer-Policy, X-Frame-Options, Content-Security-Policy
- **File Extension Validation** - הגבלת סוגי קבצים מותרים להעלאה
- **Input Validation** - ולידציה בצד לקוח (Angular Validators) ובצד שרת (Data Annotations)

## טיפול בשגיאות

- **Global Exception Handler** - middleware שתופס שגיאות לא מטופלות ומחזיר תשובה אחידה
- **Logging** - שימוש ב-log4net לתיעוד שגיאות ואירועים לקובץ
- **Client-side validation** - הודעות שגיאה ויזואליות למשתמש (שדה חובה, פורמט לא תקין)
- **Error boundaries** - הצגת הודעות שגיאה ידידותיות למשתמש בצד הלקוח

## מנגנוני קישור (Dependency Injection)

- שימוש ב-DI המובנה של ASP.NET Core
- רישום שירותים כ-Singleton: `DatabaseService`, `CaptchaService`, `ILog`
- `IMemoryCache` לשמירת session של CAPTCHA

## Cross-Domain (CORS)

- מוגדר CORS policy שמאפשר גישה מכל origin בסביבת פיתוח
- בפרודקשן יש להגביל ל-origins ספציפיים

## הוראות התקנה והפעלה

### דרישות מקדימות
- .NET 8 SDK
- Node.js 18+
- Angular CLI

### Backend
```bash
cd PublicComplaintForm_API
dotnet restore
dotnet run
```
השרת ירוץ על `http://localhost:5209`

### Frontend
```bash
cd PublicComplaintForm
npm install
ng serve
```
האפליקציה תרוץ על `http://localhost:4200`

### קונפיגורציה
- `appsettings.json` - הגדרות connection strings ונתיבי שמירה לפי סביבה
- `public/config.json` - כתובת ה-API בצד הלקוח לפי סביבה
- משתנה סביבה `ServerIdentity` - קובע איזה section ב-appsettings לטעון

### Endpoints עיקריים
| Method | Path | תיאור |
|--------|------|--------|
| GET | `/courts` | רשימת בתי משפט |
| GET | `/captcha` | יצירת CAPTCHA חדש |
| POST | `/submit-form` | שליחת הטופס המלא |
| POST | `/complaint-details` | שליחת פרטי הפניה (מחזיר את האובייקט) |
| GET | `/monthly-report` | דוח פניות חודשי |
| POST | `/send-email` | שליחת מייל התראה על בקשה חסומה |
| POST | `/survey` | שליחת סקר |
| GET | `/log` | צפייה בלוגים אחרונים |
