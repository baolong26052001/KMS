using Microsoft.AspNetCore.Mvc;
using KMS.Models;
using KMS.Tools;
using System.Data;
using Microsoft.Data.SqlClient;
using Twilio.Http;

namespace KMS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoanStatementController : ControllerBase
    {
        private readonly KioskManagementSystemContext _dbcontext;
        private IConfiguration _configuration;
        private readonly ExecuteQuery _exQuery;

        public LoanStatementController(IConfiguration configuration, KioskManagementSystemContext _context, ExecuteQuery exQuery)
        {
            _dbcontext = _context;
            _configuration = configuration;
            _exQuery = exQuery;
        }

        [HttpGet]
        [Route("ShowLoanStatement")]
        public JsonResult ShowLoanStatement()
        {
            ResponseDto response = new ResponseDto();
            try
            {
                string query = @"
                SELECT *,
                       ROW_NUMBER() OVER (ORDER BY transactionDate) AS id
                FROM (
                    SELECT a.transactionDate,
                           a.memberId,
                           b.fullName,
                           a.contractId,
                           a.loanId,
                           a.loanTerm,
                           a.debt,
                           a.totalDebtMustPay,
                           0 AS payback 
                    FROM LoanTransaction a
                    LEFT JOIN LMember b ON b.id = a.memberId 
                    UNION ALL
                    SELECT a.transactionDate,
                           a.memberId,
                           b.fullName,
                           a.contractId,
                           a.loanId,
                           c.loanTerm,
                           0 AS debt,
                           c.totalDebtMustPay,
                           a.payback 
                    FROM Payback a
                    LEFT JOIN LMember b ON b.id = a.memberId
                    LEFT JOIN LoanTransaction c ON c.loanId = a.loanId
                ) AS combined_data 
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
        [Route("ShowLoanStatementByMemberId/{memberId}")]
        public JsonResult ShowLoanStatementByMemberId(int memberId)
        {
            ResponseDto response = new ResponseDto();
            try
            {
                string query = @"SELECT *, ROW_NUMBER() OVER (ORDER BY transactionDate) AS id FROM (SELECT a.transactionDate,a.memberId,b.fullName,a.contractId,a.loanId,a.loanTerm,a.debt,a.totalDebtMustPay,0 AS payback 
                FROM LoanTransaction a LEFT JOIN LMember b ON b.id = a.memberId 
                WHERE a.memberId = @memberId UNION ALL SELECT a.transactionDate,a.memberId,b.fullName,a.contractId,a.loanId,c.loanTerm,0 AS debt,c.totalDebtMustPay,a.payback 
                FROM Payback a LEFT JOIN LMember b ON b.id = a.memberId LEFT JOIN LoanTransaction c ON c.loanId = a.loanId 
                WHERE a.memberId = @memberId) AS combined_data ORDER BY transactionDate";

                SqlParameter parameter = new SqlParameter("@memberId", memberId);
                DataTable table = _exQuery.ExecuteRawQuery(query, new[] { parameter });

                if (table.Rows.Count > 0)
                {
                    return new JsonResult(table);
                }
                else
                {
                    return new JsonResult("Loan Statement not found");
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
        [Route("SearchLoanStatement")]
        public JsonResult SearchLoanStatement(string searchQuery)
        {
            ResponseDto response = new ResponseDto();
            try
            {
                string query = @"SELECT *, ROW_NUMBER() OVER (ORDER BY transactionDate) AS id FROM (SELECT a.transactionDate,a.memberId,b.fullName,a.contractId,a.loanId,a.loanTerm,a.debt,a.totalDebtMustPay,0 AS payback 
                FROM LoanTransaction a LEFT JOIN LMember b ON b.id = a.memberId 
                WHERE a.memberId LIKE @searchQuery UNION ALL SELECT a.transactionDate,a.memberId,b.fullName,a.contractId,a.loanId,c.loanTerm,0 AS debt,c.totalDebtMustPay,a.payback 
                FROM Payback a LEFT JOIN LMember b ON b.id = a.memberId LEFT JOIN LoanTransaction c ON c.loanId = a.loanId 
                WHERE a.memberId LIKE @searchQuery) AS combined_data ORDER BY transactionDate";

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
        [Route("FilterLoanStatement")]
        public IActionResult FilterLoanStatement([FromQuery] DateTime? startDate = null, [FromQuery] DateTime? endDate = null)
        {
            ResponseDto response = new ResponseDto();
            try
            {
                string query = @"SELECT *, ROW_NUMBER() OVER (ORDER BY transactionDate) AS id FROM (
                            SELECT a.transactionDate, a.memberId, b.fullName, a.contractId, a.loanId, a.loanTerm, a.debt, a.totalDebtMustPay,0 AS payback 
                            FROM LoanTransaction a 
                            LEFT JOIN LMember b ON b.id = a.memberId 
                             
                            UNION ALL 
                            SELECT a.transactionDate, a.memberId, b.fullName, a.contractId, a.loanId, c.loanTerm, 0 AS debt, c.totalDebtMustPay,a.payback 
                            FROM Payback a 
                            LEFT JOIN LMember b ON b.id = a.memberId 
                            LEFT JOIN LoanTransaction c ON c.loanId = a.loanId 
                            
                        ) AS combined_data";

                List<SqlParameter> parameters = new List<SqlParameter>();

                if (startDate.HasValue && endDate.HasValue)
                {
                    startDate = startDate.Value.Date.AddHours(0).AddMinutes(0).AddSeconds(0);
                    endDate = endDate.Value.Date.AddHours(23).AddMinutes(59).AddSeconds(59);

                    query += " WHERE transactionDate >= @startDate AND transactionDate <= @endDate ORDER BY transactionDate";
                    
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