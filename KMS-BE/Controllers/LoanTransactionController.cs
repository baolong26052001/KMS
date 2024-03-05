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
            ResponseDto response = new ResponseDto();
            try
            {
                string query = "select * from LoanTransaction";
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
        [Route("ShowLoanTransaction/{id}")]
        public JsonResult GetLoanTransactionById(int id)
        {
            ResponseDto response = new ResponseDto();
            try
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
        [Route("ShowLoanTransactionDetailByHeaderId/{id}")]
        public JsonResult GetLoanTransactionDetail(int id)
        {
            ResponseDto response = new ResponseDto();
            try
            {
                string query = @"select * from LoanTransactionDetail where loanHeaderId = @id";

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
            catch (Exception ex)
            {
                response.Code = -1;
                response.Message = ex.Message;
                response.Exception = ex.ToString();
                response.Data = null;
            }
            return new JsonResult(response);
            
        }

        [HttpPost]
        [Route("SaveLoanTransaction")]
        public JsonResult SaveLoanTransaction([FromBody] LoanTransaction loanTransaction)
        {
            ResponseDto response = new ResponseDto();
            try
            {
                Random random = new Random();
                int contractId = random.Next(10000000, 99999999);

                string query = @"DECLARE @DueDate AS DATE
                SET @DueDate = DATEADD(MONTH, @LoanTerm, GETDATE())
                INSERT INTO LoanTransaction (accountId, contractId, loanTerm, debt, totalDebtMustPay, debtPayPerMonth, loanRate, loanDate, dueDate, status)
                VALUES (@AccountId, @ContractId, @LoanTerm, @Debt, @TotalDebtMustPay, @DebtPayPerMonth, @LoanRate, GETDATE(), @DueDate, 0)";

                SqlParameter[] parameters =
                {
                    
                    new SqlParameter("@AccountId", loanTransaction.AccountId),
                    new SqlParameter("@ContractId", contractId),
                    new SqlParameter("@LoanTerm", loanTransaction.LoanTerm),
                    new SqlParameter("@Debt", loanTransaction.Debt),
                    new SqlParameter("@TotalDebtMustPay", loanTransaction.Debt + (int)((double)loanTransaction.Debt * loanTransaction.LoanRate)),
                    new SqlParameter("@DebtPayPerMonth", (loanTransaction.Debt + loanTransaction.Debt * loanTransaction.LoanRate) / loanTransaction.LoanTerm),
                    new SqlParameter("@LoanRate", loanTransaction.LoanRate),
                    
                };


                _exQuery.ExecuteRawQuery(query, parameters);

                return new JsonResult("Loan Transaction saved successfully");
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

        [HttpPost]
        [Route("SaveLoanTransactionDetail")]
        public JsonResult SaveLoanTransactionDetail([FromBody] LoanTransactionDetail loanTransactionDetail)
        {
            ResponseDto response = new ResponseDto();
            try
            {
                string query = @"
                            DECLARE @TotalDebtMustPay DECIMAL(18, 2)

                            -- Retrieve totalDebtMustPay from LoanTransaction table
                            SELECT @TotalDebtMustPay = totalDebtMustPay 
                            FROM LoanTransaction 
                            WHERE id = @LoanHeaderId

                            -- Calculate debt remaining
                            DECLARE @DebtRemaining DECIMAL(18, 2)

                            -- Check if there are existing transactions for the same loanHeaderId
                            DECLARE @LatestDebtRemaining DECIMAL(18, 2)

                            SELECT TOP 1 @LatestDebtRemaining = debtRemaining
                            FROM LoanTransactionDetail
                            WHERE loanHeaderId = @LoanHeaderId
                            ORDER BY transactionDate DESC

                            -- If there are existing transactions, calculate @DebtRemaining based on the latest debtRemaining
                            IF (@LatestDebtRemaining IS NOT NULL)
                                SET @DebtRemaining = @LatestDebtRemaining - @PaidAmount
                            ELSE
                                SET @DebtRemaining = @TotalDebtMustPay - @PaidAmount

                            -- Insert into LoanTransactionDetail
                            INSERT INTO LoanTransactionDetail (loanHeaderId, paidAmount, debtRemaining, transactionDate)
                            VALUES (@LoanHeaderId, @PaidAmount, @DebtRemaining, GETDATE())

                            IF (@DebtRemaining <= 0)
                            UPDATE LoanTransaction 
                            SET status = 1 -- Assuming 'status' is a bit field
                            WHERE id = @LoanHeaderId


                            ";

                SqlParameter[] parameters =
                {
                    
                    new SqlParameter("@LoanHeaderId", loanTransactionDetail.LoanHeaderId),
                    new SqlParameter("@PaidAmount", loanTransactionDetail.PaidAmount),
                    
                    
                };

                _exQuery.ExecuteRawQuery(query, parameters);

                return new JsonResult("Loan Transaction Detail saved successfully");
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
        [Route("SearchLoanTransaction")]
        public JsonResult SearchLoanTransaction(string searchQuery)
        {
            ResponseDto response = new ResponseDto();
            try
            {
                string query = "SELECT * " +
                           "FROM LoanTransaction " +
                           "WHERE accountId LIKE @searchQuery OR " +
                           "contractId LIKE @searchQuery OR " +
                           "loanTerm LIKE @searchQuery OR " +
                           "debt LIKE @searchQuery OR " +
                           "debtPayPerMonth LIKE @searchQuery OR " +
                           "loanRate LIKE @searchQuery OR " +
                           "totalDebtMustPay LIKE @searchQuery";

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
        [Route("FilterLoanTransaction")]
        public JsonResult FilterLoanTransaction([FromQuery] bool? isActive = null, [FromQuery] DateTime? startDate = null, [FromQuery] DateTime? endDate = null)
        {
            ResponseDto response = new ResponseDto();
            try
            {
                string query = "SELECT * " +
                           "FROM LoanTransaction ";

                List<SqlParameter> parameters = new List<SqlParameter>();

                if (isActive.HasValue)
                {
                    query += (parameters.Count == 0 ? " WHERE " : " AND ") + "status = @isActive";
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