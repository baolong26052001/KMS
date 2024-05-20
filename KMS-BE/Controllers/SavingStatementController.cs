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
            ResponseDto response = new ResponseDto();
            try
            {
                string query = @"SELECT transactionDate, memberId, fullName, contractId, savingId, savingTerm, topUp, withdraw,
                       ROW_NUMBER() OVER (ORDER BY transactionDate) AS id
                FROM (
                    SELECT a.transactionDate, a.memberId, b.fullName, a.contractId, a.savingId, a.savingTerm, a.topUp, 0 AS withdraw
                    FROM SavingTransaction a
                    LEFT JOIN LMember b ON b.id = a.memberId 
                    WHERE a.topUp != 0 
                    UNION ALL 
                    SELECT a.transactionDate, a.memberId, b.fullName, a.contractId, a.savingId, c.savingTerm, 0 AS topUp, a.withdraw
                    FROM Withdraw a
                    LEFT JOIN LMember b ON b.id = a.memberId
                    LEFT JOIN SavingTransaction c ON c.savingId = a.savingId 
                    WHERE a.withdraw != 0
                ) AS combined_data
                GROUP BY transactionDate, memberId, fullName, contractId, savingId, savingTerm, topUp, withdraw
                ORDER BY transactionDate";

                DataTable table = _exQuery.ExecuteRawQuery(query);
                return new JsonResult(table);
            }
            catch (Exception ex)
            {
                response.Code = -1;
                response.Message = ex.Message;
                response.Exception = ex.ToString();
                response.Data = null;
            }
            return new JsonResult(response);
            
        }

        [HttpGet]
        [Route("ShowSavingStatementByMemberId/{memberId}")]
        public JsonResult ShowSavingStatementByMemberId(int memberId)
        {
            ResponseDto response = new ResponseDto();
            try
            {
                string query = @"SELECT transactionDate, memberId, fullName, contractId, savingId, savingTerm, topUp, withdraw,
                       ROW_NUMBER() OVER (ORDER BY transactionDate) AS id
                FROM (
                    SELECT a.transactionDate, a.memberId, b.fullName, a.contractId, a.savingId, a.savingTerm, a.topUp, 0 AS withdraw
                    FROM SavingTransaction a
                    LEFT JOIN LMember b ON b.id = a.memberId 
                    WHERE a.topUp != 0 and a.memberId = @memberId
                    UNION ALL 
                    SELECT a.transactionDate, a.memberId, b.fullName, a.contractId, a.savingId, c.savingTerm, 0 AS topUp, a.withdraw
                    FROM Withdraw a
                    LEFT JOIN LMember b ON b.id = a.memberId
                    LEFT JOIN SavingTransaction c ON c.savingId = a.savingId 
                    WHERE a.withdraw != 0 and a.memberId = @memberId
                ) AS combined_data
                GROUP BY transactionDate, memberId, fullName, contractId, savingId, savingTerm, topUp, withdraw
                ORDER BY transactionDate";

                SqlParameter parameter = new SqlParameter("@memberId", memberId);
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
            catch (Exception ex)
            {
                response.Code = -1;
                response.Message = ex.Message;
                response.Exception = ex.ToString();
                response.Data = null;
            }
            return new JsonResult(response);
            
        }

        [HttpGet]
        [Route("SearchSavingStatement")]
        public JsonResult SearchSavingStatement(string searchQuery)
        {
            ResponseDto response = new ResponseDto();
            try
            {
                string query = @"SELECT transactionDate, memberId, fullName, contractId, savingId, savingTerm, topUp, withdraw,
                       ROW_NUMBER() OVER (ORDER BY transactionDate) AS id
                FROM (
                    SELECT a.transactionDate, a.memberId, b.fullName, a.contractId, a.savingId, a.savingTerm, a.topUp, 0 AS withdraw
                    FROM SavingTransaction a
                    LEFT JOIN LMember b ON b.id = a.memberId 
                    WHERE a.topUp != 0 and a.memberId LIKE @searchQuery
                    UNION ALL 
                    SELECT a.transactionDate, a.memberId, b.fullName, a.contractId, a.savingId, c.savingTerm, 0 AS topUp, a.withdraw
                    FROM Withdraw a
                    LEFT JOIN LMember b ON b.id = a.memberId
                    LEFT JOIN SavingTransaction c ON c.savingId = a.savingId 
                    WHERE a.withdraw != 0 and a.memberId LIKE @searchQuery
                ) AS combined_data
                GROUP BY transactionDate, memberId, fullName, contractId, savingId, savingTerm, topUp, withdraw
                ORDER BY transactionDate";

                SqlParameter parameter = new SqlParameter("@searchQuery", "%" + searchQuery + "%");
                DataTable table = _exQuery.ExecuteRawQuery(query, new[] { parameter });

                return new JsonResult(table);
            }
            catch (Exception ex)
            {
                response.Code = -1;
                response.Message = ex.Message;
                response.Exception = ex.ToString();
                response.Data = null;
            }
            return new JsonResult(response);
            
        }

        [HttpGet]
        [Route("FilterSavingStatement")]
        public IActionResult FilterSavingStatement([FromQuery] DateTime? startDate = null, [FromQuery] DateTime? endDate = null)
        {
            ResponseDto response = new ResponseDto();
            try
            {
                string query = @"SELECT transactionDate, memberId, fullName, contractId, savingId, savingTerm, topUp, withdraw,
                       ROW_NUMBER() OVER (ORDER BY transactionDate) AS id
                FROM (
                    SELECT a.transactionDate, a.memberId, b.fullName, a.contractId, a.savingId, a.savingTerm, a.topUp, 0 AS withdraw
                    FROM SavingTransaction a
                    LEFT JOIN LMember b ON b.id = a.memberId 
                    WHERE a.topUp != 0 
                    UNION ALL 
                    SELECT a.transactionDate, a.memberId, b.fullName, a.contractId, a.savingId, c.savingTerm, 0 AS topUp, a.withdraw
                    FROM Withdraw a
                    LEFT JOIN LMember b ON b.id = a.memberId
                    LEFT JOIN SavingTransaction c ON c.savingId = a.savingId 
                    WHERE a.withdraw != 0
                ) AS combined_data";

                List<SqlParameter> parameters = new List<SqlParameter>();

                if (startDate.HasValue && endDate.HasValue)
                {
                    startDate = startDate.Value.Date.AddHours(0).AddMinutes(0).AddSeconds(0);
                    endDate = endDate.Value.Date.AddHours(23).AddMinutes(59).AddSeconds(59);

                    query += " WHERE transactionDate >= @startDate AND transactionDate <= @endDate GROUP BY transactionDate, memberId, fullName, contractId, savingId, savingTerm, topUp, withdraw ORDER BY transactionDate";
                    
                    parameters.Add(new SqlParameter("@startDate", startDate));
                    parameters.Add(new SqlParameter("@endDate", endDate));
                }

                DataTable table = _exQuery.ExecuteRawQuery(query, parameters.ToArray());

                return new JsonResult(table);
            }
            catch (Exception ex)
            {
                response.Code = -1;
                response.Message = ex.Message;
                response.Exception = ex.ToString();
                response.Data = null;
            }
            return new JsonResult(response);
        }

    }
}
