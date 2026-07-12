using PublicComplaintForm_API.Models;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Net.Http.Json;
using Dapper;
using Microsoft.SqlServer.Server;
using log4net;

namespace PublicComplaintForm_API.Services
{
    public class DatabaseService
    {
        private readonly string _connectionString = string.Empty;
        private readonly string _surveyConnectionString = string.Empty;

        private readonly ILog _logger;

        public DatabaseService(ILog logger)
        {
            _logger = logger;
        }

        public DatabaseService(string connectionString, string surveyConnectionString, ILog logger)
        {
            _connectionString = connectionString ?? string.Empty;
            _surveyConnectionString = surveyConnectionString ?? string.Empty;
            _logger = logger;
        }

        public async Task<Guid> GetCourtId(string courtName)
        {
            return Guid.Empty;
        }

        public async Task<Guid> GetCityId(string cityName)
        {
            return Guid.Empty;
        }

        public async Task<Guid> DoesContactExist(string IdNumber)
        {
            return Guid.Empty;
        }

        public async Task<List<Court>> FetchCourtList()
        {
            using var httpClient = new HttpClient();
            var response = await httpClient.GetFromJsonAsync<DataGovResponse>("https://data.gov.il/api/3/action/datastore_search?resource_id=5f8cff43-30fb-449b-96b1-280b2aafb2c3&limit=120");

            if (response?.result?.records == null)
                return new List<Court>();

            return response.result.records.Select(r => new Court
            {
                CourtId = Guid.NewGuid(),
                CourtName = r.court_name?.Trim() ?? string.Empty
            }).ToList();
        }

        public async Task<Guid> InsertContact(PublicComplaintData formData)
        {
            return Guid.Empty;
        }

        public async Task<bool> InsertComplaint(
            PublicComplaintData formData,
            Guid contactId,
            Guid inquiryId,
            bool receivedFiles)
        {
            return false;
        }

        public async Task<bool> CanSubmitSurvey(SurveyData surveyData)
        {
            return false;
        }

        public async Task SubmitSurvey(SurveyData surveyData)
        {
            return;
        }

        public async Task SubmitForm(
            PublicComplaintData formData,
            List<string> files,
            Guid inquiryId,
            bool receivedFiles)
        {
            return;
        }
    }
}
