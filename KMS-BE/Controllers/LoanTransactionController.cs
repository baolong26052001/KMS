using Microsoft.AspNetCore.Mvc;
using KMS.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Text;
using KMS.Tools;

namespace KMS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoanTransactionController : ControllerBase
    {
        private readonly KioskManagementSystemContext _dbcontext;
        private IConfiguration _configuration;
        private readonly ExecuteQuery _exQuery;

        public LoanTransactionController(IConfiguration configuration, KioskManagementSystemContext _context, ExecuteQuery exQuery)
        {
            _dbcontext = _context;
            _configuration = configuration;
            _exQuery = exQuery;
        }


        [HttpGet]
        [Route("ShowLoanTransaction")]
        public JsonResult GetLoanTransaction()
        {
            string query = "select * from LoanTransaction";
            DataTable table = _exQuery.ExecuteRawQuery(query);
            return new JsonResult(table);
        }

        [HttpGet]
        [Route("ShowLoanTransaction/{id}")]
        public JsonResult GetLoanTransactionById(int id)
        {
            string query = "select * from LoanTransaction where id=@id";

            SqlParameter parameter = new SqlParameter("@id", id);
            DataTable table = _exQuery.ExecuteRawQuery(query, new[] { parameter });

            if (table.Rows.Count > 0)
            {
                return new JsonResult(table);
            }
            else
            {
                return new JsonResult("Loan transaction not found");
            }
        }

        [HttpGet]
        [Route("ShowLoanTransactionDetail/{id}")]
        public JsonResult GetLoanTransactionDetail(int id)
        {
            string query = "select ltd.*, lt.loanDate, lt.dueDate, lt.debt " +
                "from LoanTransactionDetail ltd, LoanTransaction lt " +
                "where ltd.loanTransactionId = lt.id and lt.id=@id";

            SqlParameter parameter = new SqlParameter("@id", id);
            DataTable table = _exQuery.ExecuteRawQuery(query, new[] { parameter });

            if (table.Rows.Count > 0)
            {
                return new JsonResult(table);
            }
            else
            {
                return new JsonResult("Loan transaction detail not found");
            }
        }

        [HttpGet]
        [Route("SearchLoanTransaction")]
        public JsonResult SearchLoanTransaction(string searchQuery)
        {
            string query = "SELECT * " +
                           "FROM LoanTransaction " +
                           "WHERE memberId LIKE @searchQuery OR " +
                           "accountId LIKE @searchQuery OR " +
                           "loanTerm LIKE @searchQuery OR " +
                           "transactionType LIKE @searchQuery OR " +
                           "interestRate LIKE @searchQuery";

            SqlParameter parameter = new SqlParameter("@searchQuery", "%" + searchQuery + "%");
            DataTable table = _exQuery.ExecuteRawQuery(query, new[] { parameter });

            return new JsonResult(table);
        }

        [HttpGet]
        [Route("FilterLoanTransaction")]
        public JsonResult FilterLoanTransaction([FromQuery] bool? isActive = null, [FromQuery] DateTime? startDate = null, [FromQuery] DateTime? endDate = null)
        {
            string query = "SELECT * " +
                           "FROM LoanTransaction ";

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

                query += (parameters.Count == 0 ? " WHERE " : " AND ") + "loanDate >= @startDate AND loanDate <= @endDate";
                parameters.Add(new SqlParameter("@startDate", startDate.Value));
                parameters.Add(new SqlParameter("@endDate", endDate.Value));
            }

            DataTable table = _exQuery.ExecuteRawQuery(query, parameters.ToArray());

            return new JsonResult(table);
        }

    }
}