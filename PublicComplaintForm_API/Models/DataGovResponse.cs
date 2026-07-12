namespace PublicComplaintForm_API.Models
{
    public class DataGovResponse
    {
        public DataGovResult? result { get; set; }
    }

    public class DataGovResult
    {
        public List<DataGovRecord>? records { get; set; }
    }

    public class DataGovRecord
    {
        public string? court_name { get; set; }
    }
}
