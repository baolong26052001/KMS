using Microsoft.AspNetCore.Mvc;
using KMS.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Text;
using KMS.Tools;
using System.Diagnostics.Contracts;

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
                string query = "select * from LoanTransaction order by id desc";
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
        [Route("ShowLoanTransaction/{loanId}")] 
        public JsonResult GetLoanTransactionById(int loanId)
        {
            ResponseDto response = new ResponseDto();
            try
            {
                string query = "select * from LoanTransaction where loanId=@loanId";

                SqlParameter parameter = new SqlParameter("@loanId", loanId);
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
        [Route("ShowLoanTransactionByMemberId/{memberId}")]
        public JsonResult ShowLoanTransactionByMemberId(int memberId)
        {
            ResponseDto response = new ResponseDto();
            try
            {
                string query = "select * from LoanTransaction where memberId=@memberId";

                SqlParameter parameter = new SqlParameter("@memberId", memberId);
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
        [Route("ShowLoanTransactionByMemberIdAndStatus/{memberId}/{status}")]
        public JsonResult ShowLoanTransactionByMemberIdAndStatus(int memberId, int status)
        {
            ResponseDto response = new ResponseDto();
            try
            {
                string query = "select * from LoanTransaction where memberId=@memberId and status=@status";

                SqlParameter[] parameters =
                {
                    new SqlParameter("@memberId", memberId),
                    new SqlParameter("@status", status),
                };

                DataTable table = _exQuery.ExecuteRawQuery(query, parameters);

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
        [Route("ShowPaybackByLoanId/{loanId}")]
        public JsonResult ShowPaybackByLoanId(int loanId)
        {
            ResponseDto response = new ResponseDto();
            try
            {
                string query = @"select * from Payback p where p.loanId = @loanId";

                SqlParameter parameter = new SqlParameter("@loanId", loanId);
                DataTable table = _exQuery.ExecuteRawQuery(query, new[] { parameter });

                if (table.Rows.Count > 0)
                {
                    return new JsonResult(table);
                }
                else
                {
                    return new JsonResult("Payback data not found");
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
                int contractId;
                
                do
                {
                    contractId = random.Next(10000000, 99999999);
                } while (CheckIfContractIdExists(contractId));

                string queryA = @"INSERT INTO LTransactionLog (memberId, transactionId, transactionDate, transactionType, status) 
                                  VALUES (@MemberId, @ContractId, GETDATE(), 'Loan', 3)";

                SqlParameter[] parametersA =
                {
                    new SqlParameter("@MemberId", loanTransaction.MemberId),
                    new SqlParameter("@ContractId", contractId),
                };

                _exQuery.ExecuteRawQuery(queryA, parametersA);

                //

                string getMaxLoanIdQuery = "SELECT ISNULL(MAX(loanId), 0) FROM LoanTransaction";
                int maxLoanId;

                maxLoanId = _exQuery.ExecuteScalar<int>(getMaxLoanIdQuery, null);

                int newLoanId = maxLoanId + 1;

                string query = @"
                
                DECLARE @DueDate AS DATE
                SET @DueDate = DATEADD(MONTH, @LoanTerm, GETDATE())
                
                INSERT INTO LoanTransaction (memberId, contractId, loanId, loanTerm, debt, totalDebtMustPay, loanRate, transactionDate, dueDate, status)
                VALUES (@MemberId, @ContractId, @LoanId, @LoanTerm, @Debt, @TotalDebtMustPay, @LoanRate, GETDATE(), @DueDate, @Status)
                

                ";

                SqlParameter[] parameters =
                {
                    new SqlParameter("@MemberId", loanTransaction.MemberId),
                    new SqlParameter("@ContractId", contractId),
                    new SqlParameter("@LoanId", newLoanId),
                    new SqlParameter("@LoanTerm", loanTransaction.LoanTerm),
                    new SqlParameter("@Debt", loanTransaction.Debt),
                    new SqlParameter("@TotalDebtMustPay", loanTransaction.Debt + (int)((double)loanTransaction.Debt * loanTransaction.LoanRate)),
                    new SqlParameter("@LoanRate", loanTransaction.LoanRate),
                    new SqlParameter("@Status", loanTransaction.Status)
                };


                _exQuery.ExecuteRawQuery(query, parameters);

                return new JsonResult(new
                {
                    Code = 200,
                    Message = "Save loan transaction successfully",
                    contractId = contractId,
                    loanId = newLoanId
                });
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
        [Route("SavePayback")]
        public JsonResult SavePayback([FromBody] PayBack payback)
        {
            ResponseDto response = new ResponseDto();
            try
            {
                string queryA = @"INSERT INTO LTransactionLog (memberId, transactionId, transactionDate, transactionType, status) 
                                  VALUES (@MemberId, @ContractId, GETDATE(), 'Payback', 3)";

                SqlParameter[] parametersA =
                {
                    new SqlParameter("@MemberId", payback.MemberId),
                    new SqlParameter("@ContractId", payback.ContractId),
                };

                _exQuery.ExecuteRawQuery(queryA, parametersA);

                string query = @"
                            DECLARE @TotalDebtMustPay DECIMAL(18, 2)

                            -- Retrieve totalDebtMustPay from LoanTransaction table
                            SELECT @TotalDebtMustPay = totalDebtMustPay 
                            FROM LoanTransaction 
                            WHERE loanId = @LoanId

                            -- Calculate debt remaining
                            DECLARE @DebtRemaining DECIMAL(18, 2)

                            -- Check if there are existing transactions for the same LoanId
                            DECLARE @LatestDebtRemaining DECIMAL(18, 2)

                            SELECT TOP 1 @LatestDebtRemaining = indebt
                            FROM Payback
                            WHERE loanId = @LoanId
                            ORDER BY transactionDate DESC

                            -- If there are existing transactions, calculate @DebtRemaining based on the latest debtRemaining
                            IF (@LatestDebtRemaining IS NOT NULL)
                                SET @DebtRemaining = @LatestDebtRemaining - @Payback
                            ELSE
                                SET @DebtRemaining = @TotalDebtMustPay - @Payback

                            -- Insert into Payback
                            INSERT INTO Payback (contractId, memberId, loanId, payback, indebt, transactionDate)
                            VALUES (@ContractId, @MemberId, @LoanId, @Payback, @DebtRemaining, GETDATE())

                            IF (@DebtRemaining <= 0)
                            UPDATE LoanTransaction 
                            SET status = 0 -- Assuming 'status' is a bit field
                            WHERE loanId = @LoanId";

                SqlParameter[] parameters =
                {
                    new SqlParameter("@ContractId", payback.ContractId),
                    new SqlParameter("@MemberId", payback.MemberId),
                    new SqlParameter("@LoanId", payback.LoanId),
                    new SqlParameter("@Payback", payback.Payback),
                };

                _exQuery.ExecuteRawQuery(query, parameters);

                return new JsonResult(new
                {
                    Code = 200,
                    Message = "Save payback successfully",
                });
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
                           "WHERE memberId LIKE @searchQuery OR " +
                           "contractId LIKE @searchQuery OR " +
                           "loanTerm LIKE @searchQuery OR " +
                           "debt LIKE @searchQuery OR " +
                           "loanId LIKE @searchQuery OR " +
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
        public JsonResult FilterLoanTransaction([FromQuery] bool? status = null, [FromQuery] DateTime? startDate = null, [FromQuery] DateTime? endDate = null)
        {
            ResponseDto response = new ResponseDto();
            try
            {
                string query = "SELECT * " +
                           "FROM LoanTransaction ";

                List<SqlParameter> parameters = new List<SqlParameter>();

                if (status.HasValue)
                {
                    query += (parameters.Count == 0 ? " WHERE " : " AND ") + "status = @Status";
                    parameters.Add(new SqlParameter("@Status", status.Value));
                }

                if (startDate.HasValue && endDate.HasValue)
                {

                    startDate = startDate.Value.Date.AddHours(0).AddMinutes(0).AddSeconds(0);
                    endDate = endDate.Value.Date.AddHours(23).AddMinutes(59).AddSeconds(59);

                    query += (parameters.Count == 0 ? " WHERE " : " AND ") + "transactionDate >= @startDate AND transactionDate <= @endDate";
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

        //[HttpGet]
        //[Route("FilterLoanTransactionByStatus")]
        //public JsonResult FilterLoanTransactionByStatus([FromQuery] bool? status = null)
        //{
        //    ResponseDto response = new ResponseDto();
        //    try
        //    {
        //        string query = "SELECT * " +
        //                   "FROM LoanTransaction ";

        //        List<SqlParameter> parameters = new List<SqlParameter>();

        //        if (status.HasValue)
        //        {
        //            query += (parameters.Count == 0 ? " WHERE " : " AND ") + "status = @Status";
        //            parameters.Add(new SqlParameter("@Status", status.Value));
        //        }



        //        DataTable table = _exQuery.ExecuteRawQuery(query, parameters.ToArray());

        //        return new JsonResult(table);
        //    }
        //    catch (Exception ex)
        //    {
        //        response.Code = -1;
        //        response.Message = ex.Message;
        //        response.Exception = ex.ToString();
        //        response.Data = null;
        //    }
        //    return new JsonResult(response);

        //}

        bool CheckIfContractIdExists(int contractId)
        {
            // Check if contractId exists in LoanTransaction table
            string loanQuery = "SELECT COUNT(*) FROM LoanTransaction WHERE contractId = @ContractId";
            SqlParameter[] loanParameters = { new SqlParameter("@ContractId", contractId) };
            int loanCount = _exQuery.ExecuteScalar<int>(loanQuery, loanParameters);

            // Check if contractId exists in SavingTransaction table
            string savingQuery = "SELECT COUNT(*) FROM SavingTransaction WHERE contractId = @ContractId";
            SqlParameter[] savingParameters = { new SqlParameter("@ContractId", contractId) };
            int savingCount = _exQuery.ExecuteScalar<int>(savingQuery, savingParameters);

            // Check if contractId exists in InsuranceTransaction table
            string insuranceQuery = "SELECT COUNT(*) FROM InsuranceTransaction WHERE contractId = @ContractId";
            SqlParameter[] insuranceParameters = { new SqlParameter("@ContractId", contractId) };
            int insuranceCount = _exQuery.ExecuteScalar<int>(insuranceQuery, insuranceParameters);

            // If any count is greater than 0, contractId already exists
            return loanCount > 0 || savingCount > 0 || insuranceCount > 0;
        }

    }
}