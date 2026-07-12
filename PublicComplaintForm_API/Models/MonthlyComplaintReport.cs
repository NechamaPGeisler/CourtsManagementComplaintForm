namespace PublicComplaintForm_API.Models
{
    public class MonthlyComplaintReport
    {
        public string Department { get; set; } = string.Empty;
        public int CurrentMonthCount { get; set; }
        public int PreviousMonthCount { get; set; }
        public int SameMonthLastYearCount { get; set; }
    }
}
