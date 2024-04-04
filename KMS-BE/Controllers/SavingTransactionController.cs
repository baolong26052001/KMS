using Microsoft.AspNetCore.Mvc;
using KMS.Models;
using KMS.Tools;
using System.Data;
using Microsoft.Data.SqlClient;

namespace KMS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SavingTransactionController : ControllerBase
    {
        private readonly KioskManagementSystemContext _dbcontext;
        private IConfiguration _configuration;
        private readonly ExecuteQuery _exQuery;

        public SavingTransactionController(IConfiguration configuration, KioskManagementSystemContext _context, ExecuteQuery exQuery)
        {
            _dbcontext = _context;
            _configuration = configuration;
            _exQuery = exQuery;
        }

        [HttpGet]
        [Route("ShowSavingTransaction")]
        public JsonResult GetSavingTransaction()
        {
            ResponseDto response = new ResponseDto();
            try
            {
                string query = "select * from SavingTransaction";

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
        [Route("ShowSavingTransaction/{id}")] // use for VIEW in KMS
        public JsonResult GetSavingTransactionById(int id)
        {
            ResponseDto response = new ResponseDto();
            try
            {
                string query = "select * from SavingTransaction where savingId=@id";

                SqlParameter parameter = new SqlParameter("@id", id);
                DataTable table = _exQuery.ExecuteRawQuery(query, new[] { parameter });

                if (table.Rows.Count > 0)
                {
                    return new JsonResult(table);
                }
                else
                {
                    return new JsonResult("Saving transaction not found");
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
        [Route("ShowSavingTransactionByMemberId/{id}")]
        public JsonResult ShowSavingTransactionByMemberId(int id)
        {
            ResponseDto response = new ResponseDto();
            try
            {
                string query = "select * from SavingTransaction where memberId=@id";

                SqlParameter parameter = new SqlParameter("@id", id);
                DataTable table = _exQuery.ExecuteRawQuery(query, new[] { parameter });

                if (table.Rows.Count > 0)
                {
                    return new JsonResult(table);
                }
                else
                {
                    return new JsonResult("Saving transaction not found");
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
        [Route("ShowWithdrawBySavingId/{id}")]
        public JsonResult ShowWithdrawBySavingId(int id)
        {
            ResponseDto response = new ResponseDto();
            try
            {
                string query = @"select * 
                from Withdraw wd
                where wd.savingId=@id";

                SqlParameter parameter = new SqlParameter("@id", id);
                DataTable table = _exQuery.ExecuteRawQuery(query, new[] { parameter });

                if (table.Rows.Count > 0)
                {
                    return new JsonResult(table);
                }
                else
                {
                    return new JsonResult("Withdraw data not found");
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
        [Route("SaveSavingTransaction")]
        public JsonResult SaveSavingTransaction([FromBody] SavingTransaction savingTransaction)
        {
            ResponseDto response = new ResponseDto();
            try
            {
                Random random = new Random();
                int contractId = random.Next(10000000, 99999999);

                string queryA = @"
                                DECLARE @InsertedId INT;
                                INSERT INTO LTransactionLog (memberId, transactionId, transactionDate, transactionType) 
                                VALUES (@MemberId, NULL, GETDATE(), 'Saving');
                                SET @InsertedId = SCOPE_IDENTITY();
                                UPDATE LTransactionLog SET transactionId = @InsertedId WHERE id = @InsertedId;
                            ";

                SqlParameter[] parametersA =
                {
                    new SqlParameter("@MemberId", savingTransaction.MemberId),
                };
                _exQuery.ExecuteRawQuery(queryA, parametersA);

                string queryB = @"SELECT MAX(id) FROM LTransactionLog; ";
                SqlParameter[] parametersB = { };

                int insertedId = _exQuery.ExecuteScalar<int>(queryB, parametersB);

                string query = @"INSERT INTO SavingTransaction (transactionId, memberId, contractId, savingTerm, topUp, savingRate, transactionDate, dueDate, status)
                VALUES (@TransactionId, @MemberId, @ContractId, @SavingTerm, @TopUp, @SavingRate, GETDATE(), DATEADD(year, 1, GETDATE()), @Status)";

                SqlParameter[] parameters =
                {
                    new SqlParameter("@TransactionId", insertedId),
                    new SqlParameter("@MemberId", savingTransaction.MemberId),
                    new SqlParameter("@ContractId", contractId),
                    new SqlParameter("@SavingTerm", savingTransaction.SavingTerm),
                    new SqlParameter("@TopUp", savingTransaction.TopUp),
                   
                    
                    new SqlParameter("@SavingRate", savingTransaction.SavingRate),
                    new SqlParameter("@Status", savingTransaction.Status)
                };


                _exQuery.ExecuteRawQuery(query, parameters);

                return new JsonResult(new
                {
                    Code = 200,
                    Message = "Save saving transaction successfully",
                    transactionId = insertedId
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
        [Route("SearchSavingTransaction")]
        public JsonResult SearchSavingTransaction(string searchQuery)
        {
            ResponseDto response = new ResponseDto();
            try
            {
                string query = "SELECT * " +
                           "FROM SavingTransaction " +
                           "WHERE memberId LIKE @searchQuery OR " +
                           "accountId LIKE @searchQuery OR " +
                           "savingTerm LIKE @searchQuery OR " +
                           "annualRate LIKE @searchQuery";

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
        [Route("FilterSavingTransaction")]
        public JsonResult FilterSavingTransaction([FromQuery] bool? isActive = null, [FromQuery] DateTime? startDate = null, [FromQuery] DateTime? endDate = null)
        {
            ResponseDto response = new ResponseDto();
            try
            {
                string query = "SELECT * " +
                           "FROM SavingTransaction ";

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

                    query += (parameters.Count == 0 ? " WHERE " : " AND ") + "dateCreated >= @startDate AND dateCreated <= @endDate";
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