using Microsoft.AspNetCore.Mvc;
using KMS.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using KMS.Tools;

namespace KMS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionLogController : ControllerBase
    {
        private readonly KioskManagementSystemContext _dbcontext;
        private IConfiguration _configuration;
        private readonly ExecuteQuery _exQuery;

        public TransactionLogController(IConfiguration configuration, KioskManagementSystemContext _context, ExecuteQuery exQuery)
        {
            _dbcontext = _context;
            _configuration = configuration;
            _exQuery = exQuery;
        }

        [HttpGet]
        [Route("ShowTransactionLog")]
        public JsonResult GetTransactionLog()
        {
            string query = "select tl.id, tl.transactionDate, tl.kioskId, k.kioskName, tl.memberId, m.fullName, tl.transactionId, st.stationName, tl.transactionType, tl.kioskRemainingMoney, tl.status " +
                "from LTransactionLog tl, TKiosk k, LMember m, LAccount a, TStation st " +
                "where tl.kioskId = k.id and tl.memberId = m.id and tl.accountId = a.id and tl.stationId = st.id";
            DataTable table = _exQuery.ExecuteRawQuery(query);
            return new JsonResult(table);
        }

        [HttpGet]
        [Route("ShowTransactionLog/{id}")]
        public JsonResult GetTransactionLogById(int id)
        {
            string query = "SELECT tl.id, tl.transactionDate, tl.kioskId, k.kioskName, tl.memberId, m.fullName, tl.transactionId, st.stationName, tl.transactionType, tl.kioskRemainingMoney, tl.status " +
                "FROM LTransactionLog tl, TKiosk k, LMember m, LAccount a, TStation st " +
                "WHERE tl.kioskId = k.id AND tl.memberId = m.id AND tl.accountId = a.id AND tl.stationId = st.id AND tl.transactionId = @transactionId";

            SqlParameter parameter = new SqlParameter("@transactionId", id);
            DataTable table = _exQuery.ExecuteRawQuery(query, new[] { parameter });

            return new JsonResult(table);
        }


        [HttpGet]
        [Route("FilterTransactionLog")]
        public JsonResult FilterTransactionLog([FromQuery] string? transactionType = null, [FromQuery] string? stationName = null, [FromQuery] DateTime? startDate = null, [FromQuery] DateTime? endDate = null)
        {
            string query = "SELECT tl.id, tl.transactionDate, tl.kioskId, k.kioskName, tl.memberId, m.fullName, tl.transactionId, st.stationName, tl.transactionType, tl.status " +
                "FROM LTransactionLog tl, TKiosk k, LMember m, LAccount a, TStation st " +
                "WHERE tl.kioskId = k.id AND tl.memberId = m.id AND tl.accountId = a.id AND tl.stationId = st.id ";

            List<SqlParameter> parameters = new List<SqlParameter>();

            if (!string.IsNullOrEmpty(transactionType))
            {
                query += " AND " + "tl.transactionType LIKE @transactionType ";
                parameters.Add(new SqlParameter("@transactionType", "%" + transactionType + "%"));
            }

            if (!string.IsNullOrEmpty(stationName))
            {
                query += (parameters.Count == 0 ? " " : "AND ") + "st.stationName LIKE @stationName ";
                parameters.Add(new SqlParameter("@stationName", "%" + stationName + "%"));
            }

            if (startDate.HasValue)
            {
                query += (parameters.Count == 0 ? " " : "AND ") + "tl.transactionDate >= @startDate ";
                parameters.Add(new SqlParameter("@startDate", startDate.Value));
            }

            if (endDate.HasValue)
            {
                query += (parameters.Count == 0 ? " " : "AND ") + "tl.transactionDate <= @endDate ";
                parameters.Add(new SqlParameter("@endDate", endDate.Value));
            }

            DataTable table = _exQuery.ExecuteRawQuery(query, parameters.ToArray());

            return new JsonResult(table);
        }

        [HttpGet]
        [Route("SearchTransactionLog")]
        public JsonResult SearchTransactionLog(string searchQuery)
        {
            string query = "SELECT tl.id, tl.transactionDate, tl.kioskId, k.kioskName, tl.memberId, m.fullName, tl.transactionId, st.stationName, tl.transactionType, tl.status " +
                "FROM LTransactionLog tl, TKiosk k, LMember m, LAccount a, TStation st " +
                "WHERE tl.kioskId = k.id AND tl.memberId = m.id AND tl.accountId = a.id AND tl.stationId = st.id AND " +
                "(tl.kioskId LIKE @searchQuery OR " +
                "tl.memberId LIKE @searchQuery OR " +
                "k.kioskName LIKE @searchQuery OR " +
                "m.fullName LIKE @searchQuery OR " +
                "tl.transactionId LIKE @searchQuery OR " +
                "st.stationName LIKE @searchQuery OR " +
                "tl.transactionType LIKE @searchQuery OR " +
                "tl.status LIKE @searchQuery)";

            SqlParameter parameter = new SqlParameter("@searchQuery", "%" + searchQuery + "%");
            DataTable table = _exQuery.ExecuteRawQuery(query, new[] { parameter });

            return new JsonResult(table);
        }



    }
}
