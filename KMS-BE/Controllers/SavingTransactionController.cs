﻿using Microsoft.AspNetCore.Mvc;
using KMS.Models;
using KMS.Tools;
using System.Data;
using Microsoft.Data.SqlClient;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.Diagnostics.Contracts;

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
                string query = "select * from SavingTransaction ORDER BY id DESC";

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
        [Route("ShowSavingTransaction/{savingId}")] 
        public JsonResult GetSavingTransactionById(int savingId)
        {
            ResponseDto response = new ResponseDto();
            try
            {
                string query = "select * from SavingTransaction where savingId=@savingId";

                SqlParameter parameter = new SqlParameter("@savingId", savingId);
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
        [Route("ShowSavingTransactionByMemberId/{memberId}")]
        public JsonResult ShowSavingTransactionByMemberId(int memberId)
        {
            ResponseDto response = new ResponseDto();
            try
            {
                string query = "select * from SavingTransaction where memberId=@memberId";

                SqlParameter parameter = new SqlParameter("@memberId", memberId);
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
        [Route("ShowSavingTransactionByMemberIdAndStatus/{memberId}/{status}")]
        public JsonResult ShowSavingTransactionByMemberIdAndStatus(int memberId, int status)
        {
            ResponseDto response = new ResponseDto();
            try
            {
                string query = "select * from SavingTransaction where memberId=@memberId and status=@status";

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
        [Route("ShowSavingTransactionByMemberIdAndStatusAndSavingId/{memberId}/{status}/{savingId}")]
        public JsonResult ShowSavingTransactionByMemberIdAndStatusAndSavingId(int memberId, int status, int savingId)
        {
            ResponseDto response = new ResponseDto();
            try
            {
                string query = "select * from SavingTransaction where memberId=@memberId and status=@status and savingId=@savingId";

                SqlParameter[] parameters =
                {
                    new SqlParameter("@memberId", memberId),
                    new SqlParameter("@status", status),
                    new SqlParameter("@savingId", savingId)
                };

                DataTable table = _exQuery.ExecuteRawQuery(query, parameters);

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
        [Route("ShowWithdrawBySavingId/{savingId}")]
        public JsonResult ShowWithdrawBySavingId(int savingId)
        {
            ResponseDto response = new ResponseDto();
            try
            {
                string query = @"select * 
                from Withdraw wd
                where wd.savingId=@savingId";

                SqlParameter parameter = new SqlParameter("@savingId", savingId);
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
                int contractId;

                do
                {
                    contractId = random.Next(10000000, 99999999);
                } while (CheckIfContractIdExists(contractId));

                string queryA = @"INSERT INTO LTransactionLog (memberId, transactionId, transactionDate, transactionType, status) 
                                  VALUES (@MemberId, @ContractId, GETDATE(), 'Saving', 3)";

                SqlParameter[] parametersA =
                {
                    new SqlParameter("@MemberId", savingTransaction.MemberId),
                    new SqlParameter("@ContractId", contractId),
                };

                _exQuery.ExecuteRawQuery(queryA, parametersA);

                //

                string getMaxSavingIdQuery = "SELECT ISNULL(MAX(savingId), 0) FROM SavingTransaction";
                int maxSavingId;

                maxSavingId = _exQuery.ExecuteScalar<int>(getMaxSavingIdQuery, null);
                
                int newSavingId = maxSavingId + 1;
                int balance = (int)savingTransaction.TopUp;

                string query = @"INSERT INTO SavingTransaction (memberId, savingId, balance, contractId, savingTerm, topUp, savingRate, transactionDate, dueDate, status)
                VALUES (@MemberId, @SavingId, @Balance, @ContractId, @SavingTerm, @TopUp, @SavingRate, GETDATE(), CAST(DATEADD(month, @SavingTerm, GETDATE()) AS DATE), @Status)";

                SqlParameter[] parameters =
                {
                    new SqlParameter("@MemberId", savingTransaction.MemberId),
                    new SqlParameter("@SavingId", newSavingId),
                    new SqlParameter("@Balance", balance),
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
                    contractId = contractId,
                    savingId = newSavingId
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
        [Route("SaveWithdraw")]
        public JsonResult SaveWithdraw([FromBody] WithDraw withDraw)
        {
            ResponseDto response = new ResponseDto();
            try
            {
                string queryA = @"INSERT INTO LTransactionLog (memberId, transactionId, transactionDate, transactionType, status) 
                                  VALUES (@MemberId, @ContractId, GETDATE(), 'Withdraw', 3)";

                SqlParameter[] parametersA =
                {
                    new SqlParameter("@MemberId", withDraw.MemberId),
                    new SqlParameter("@ContractId", withDraw.ContractId),
                };

                _exQuery.ExecuteRawQuery(queryA, parametersA);

                string query = @"
                DECLARE @SavingTerm INT, @SavingRate FLOAT, @DueDate DATE;

                SELECT @SavingTerm = savingTerm, @SavingRate = savingRate, @DueDate = dueDate
                FROM SavingTransaction
                WHERE contractId = @ContractId;

                INSERT INTO Withdraw (contractId, memberId, savingId, withdraw, balance, transactionDate)
                VALUES (@ContractId, @MemberId, @SavingId, @Withdraw, 0, GETDATE());

                INSERT INTO SavingTransaction (memberId, savingId, balance, contractId, savingTerm, topUp, savingRate, transactionDate, dueDate, status)
                VALUES (@MemberId, @SavingId, 0, @ContractId, @SavingTerm, 0, @SavingRate, GETDATE(), @DueDate, 2);
                ";

                SqlParameter[] parameters =
                {
                    new SqlParameter("@ContractId", withDraw.ContractId),
                    new SqlParameter("@MemberId", withDraw.MemberId),
                    new SqlParameter("@SavingId", withDraw.SavingId),
                    new SqlParameter("@Withdraw", withDraw.Withdraw),
                };

                _exQuery.ExecuteRawQuery(query, parameters);

                return new JsonResult(new
                {
                    Code = 200,
                    Message = "Save withdraw successfully",
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
                           "contractId LIKE @searchQuery OR " +
                           "savingTerm LIKE @searchQuery";

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
                    query += (parameters.Count == 0 ? " WHERE " : " AND ") + "status = @isActive";
                    parameters.Add(new SqlParameter("@isActive", isActive.Value));
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