using Microsoft.AspNetCore.Mvc;
using KMS.Models;
using KMS.Tools;
using System.Data;
using Microsoft.Data.SqlClient;

namespace KMS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SavingStatementController : ControllerBase
    {
        private readonly KioskManagementSystemContext _dbcontext;
        private IConfiguration _configuration;
        private readonly ExecuteQuery _exQuery;

        public SavingStatementController(IConfiguration configuration, KioskManagementSystemContext _context, ExecuteQuery exQuery)
        {
            _dbcontext = _context;
            _configuration = configuration;
            _exQuery = exQuery;
        }

        [HttpGet]
        [Route("ShowSavingStatement")]
        public JsonResult GetSavingStatement()
        {
            string query = "select * from SavingStatement";
            DataTable table = _exQuery.ExecuteRawQuery(query);
            return new JsonResult(table);
        }

        [HttpGet]
        [Route("ShowSavingStatement/{id}")]
        public JsonResult GetSavingStatementById(int id)
        {
            string query = "SELECT * " +
                           "FROM SavingStatement " +
                           "WHERE id = @Id";
            SqlParameter parameter = new SqlParameter("@Id", id);
            DataTable table = _exQuery.ExecuteRawQuery(query, new[] { parameter });

            if (table.Rows.Count > 0)
            {
                return new JsonResult(table);
            }
            else
            {
                return new JsonResult("Saving Statement not found");
            }
        }

        [HttpGet]
        [Route("SearchSavingStatement")]
        public JsonResult SearchSavingStatement(string searchQuery)
        {
            string query = "SELECT * " +
                           "FROM SavingStatement " +
                           "WHERE memberId LIKE @searchQuery OR " +
                           "accountId LIKE @searchQuery OR " +
                           "savingId LIKE @searchQuery OR " +
                           "description LIKE @searchQuery OR " +
                           "period LIKE @searchQuery OR " +
                           "annualRate LIKE @searchQuery OR " +
                           "status LIKE @searchQuery OR " +
                           "balance LIKE @searchQuery";

            SqlParameter parameter = new SqlParameter("@searchQuery", "%" + searchQuery + "%");
            DataTable table = _exQuery.ExecuteRawQuery(query, new[] { parameter });

            return new JsonResult(table);
        }

        [HttpGet]
        [Route("FilterSavingStatement")]
        public JsonResult FilterSavingStatement([FromQuery] bool? isActive = null, [FromQuery] DateTime? startDate = null, [FromQuery] DateTime? endDate = null)
        {
            string query = "SELECT * " +
                           "FROM SavingStatement ";

            List<SqlParameter> parameters = new List<SqlParameter>();

            if (isActive.HasValue)
            {
                query += (parameters.Count == 0 ? " WHERE " : " AND ") + "isActive = @isActive";
                parameters.Add(new SqlParameter("@isActive", isActive.Value));
            }

            if (startDate.HasValue && endDate.HasValue)
            {

                startDate = startDate.Value.Date.AddHours(0).AddMinutes(0).AddSeconds(0);
                endDate = endDate.Value.Date.AddHours(23).AddMinutes(59).AddSeconds(59);

                query += (parameters.Count == 0 ? " WHERE " : " AND ") + "dateSaving >= @startDate AND dateSaving <= @endDate";
                parameters.Add(new SqlParameter("@startDate", startDate.Value));
                parameters.Add(new SqlParameter("@endDate", endDate.Value));
            }

            DataTable table = _exQuery.ExecuteRawQuery(query, parameters.ToArray());

            return new JsonResult(table);
        }

    }
}