﻿using Microsoft.AspNetCore.Mvc;
using KMS.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Security.Principal;
using System.Text;
using KMS.Tools;
using Microsoft.AspNetCore.Authorization;

namespace KMS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly KioskManagementSystemContext _dbcontext;
        private IConfiguration _configuration;
        private readonly ExecuteQuery _exQuery;

        public AccountController(IConfiguration configuration, KioskManagementSystemContext _context, ExecuteQuery exQuery)
        {
            _dbcontext = _context;
            _configuration = configuration;
            _exQuery = exQuery;
        }

        

        [HttpGet]
        [Route("ShowAccount")]
        public JsonResult GetAccount()
        {
            ResponseDto response = new ResponseDto();

            try
            {
                string query = "select a.id, a.memberId, m.fullname, a.contractId, m.phone, m.email, m.idenNumber, a.balance, m.bankName, m.department, m.companyName, m.address1, m.isActive, m.dateCreated " +
                "from LAccount a, LMember m " +
                "where a.memberId = m.id";

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
        [Route("ShowAccount/{id}")]
        public JsonResult GetAccountById(int id)
        {
            ResponseDto response = new ResponseDto();
            try
            {
                string query = "select a.id, a.memberId, m.fullname, a.contractId, m.phone, m.email, m.idenNumber, a.balance, m.bankName, m.department, m.companyName, m.address1, m.isActive, m.dateCreated " +
                "from LAccount a, LMember m " +
                "where a.memberId = m.id and a.id=@Id";

                SqlParameter parameter = new SqlParameter("@Id", id);
                DataTable table = _exQuery.ExecuteRawQuery(query, new[] { parameter });

                if (table.Rows.Count > 0)
                {
                    return new JsonResult(table);
                }
                else
                {
                    response.Code = 404;
                    response.Message = "Account ID not found";
                    response.Exception = "";
                    response.Data = null;
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

        [HttpPut]
        [Route("UpdateBalance/{id}")]
        public JsonResult UpdateBalance(int id, int money)
        {
            ResponseDto response = new ResponseDto();
            try
            {
                string query = "UPDATE LAccount " +
                           "SET balance = balance + @Money " +
                           "WHERE id = @Id";

                SqlParameter[] parameters =
                {
                    new SqlParameter("@Id", id),
                    new SqlParameter("@Money", money),

                };

                _exQuery.ExecuteRawQuery(query, parameters);

                return new JsonResult("Balance updated successfully.");
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
        [Route("FilterAccount")]
        public JsonResult FilterAccount([FromQuery] int? status = null, [FromQuery] DateTime? startDate = null, [FromQuery] DateTime? endDate = null)
        {
            ResponseDto response = new ResponseDto();
            try
            {
                string query = "select a.id, a.memberId, m.fullname, a.contractId, m.phone, m.email, m.idenNumber, a.balance, m.bankName, m.department, m.companyName, m.address1, m.isActive, m.dateCreated " +
                "FROM LAccount a LEFT JOIN LMember m ON a.memberId = m.id ";

                List<SqlParameter> parameters = new List<SqlParameter>();

                if (status != null)
                {
                    query += (parameters.Count == 0 ? " WHERE " : " AND ") + "a.status = @status";
                    parameters.Add(new SqlParameter("@status", status));
                }

                if (startDate.HasValue && endDate.HasValue)
                {
                    startDate = startDate.Value.Date.AddHours(0).AddMinutes(0).AddSeconds(0);
                    endDate = endDate.Value.Date.AddHours(23).AddMinutes(59).AddSeconds(59);

                    query += (parameters.Count == 0 ? " WHERE " : " AND ") + "a.dateCreated >= @startDate AND a.dateCreated <= @endDate";
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



        //[HttpPost]
        //[Route("AddAccount")]
        //public JsonResult AddAccount([FromBody] Laccount account)
        //{
        //    string query = "INSERT INTO LAccount (contractId, memberId, accountName, accountType, balance, rate, dateDue, status, dateModified, dateCreated, isActive) " +
        //                   "VALUES (@ContractId, @MemberId, @AccountName, @AccountType, @Balance, @Rate, @DateDue, @Status, GETDATE(), GETDATE(), @IsActive)";

        //    SqlParameter[] parameters =
        //    {
        //        new SqlParameter("@ContractId", account.ContractId),
        //        new SqlParameter("@MemberId", account.MemberId),
        //        new SqlParameter("@AccountName", account.AccountName),
        //        new SqlParameter("@AccountType", account.AccountType),
        //        new SqlParameter("@Balance", account.Balance),
        //        new SqlParameter("@Rate", account.Rate),
        //        new SqlParameter("@DateDue", account.DateDue),
        //        new SqlParameter("@Status", account.Status),
        //        new SqlParameter("@IsActive", account.IsActive)
        //    };

        //    _exQuery.ExecuteRawQuery(query, parameters);

        //    return new JsonResult("Account added successfully");
        //}

        //[HttpPut]
        //[Route("UpdateAccount/{id}")]
        //public JsonResult UpdateAccount(int id,[FromBody] Laccount updatedAccount)
        //{
        //    string query = "UPDATE LAccount " +
        //                   "SET contractId = @ContractId, memberId = @MemberId, accountName = @AccountName, accountType = @AccountType, " +
        //                   "balance = @Balance, rate = @Rate, dateDue = @DateDue, status = @Status, dateModified = GETDATE(), " +
        //                   "dateCreated = GETDATE(), isActive = @IsActive " +
        //                   "WHERE id = @Id";

        //    SqlParameter[] parameters =
        //    {
        //        new SqlParameter("@Id", id),
        //        new SqlParameter("@ContractId", updatedAccount.ContractId),
        //        new SqlParameter("@MemberId", updatedAccount.MemberId),
        //        new SqlParameter("@AccountName", updatedAccount.AccountName),
        //        new SqlParameter("@AccountType", updatedAccount.AccountType),
        //        new SqlParameter("@Balance", updatedAccount.Balance),
        //        new SqlParameter("@Rate", updatedAccount.Rate),
        //        new SqlParameter("@DateDue", updatedAccount.DateDue),
        //        new SqlParameter("@Status", updatedAccount.Status),
        //        new SqlParameter("@IsActive", updatedAccount.IsActive)
        //    };

        //    _exQuery.ExecuteRawQuery(query, parameters);

        //    return new JsonResult("Account updated successfully");
        //}

        

        [HttpGet]
        [Route("SearchAccount")]
        public JsonResult SearchAccount(string searchQuery)
        {
            string query = "select a.id, a.memberId, m.fullname, a.contractId, m.phone, m.email, m.idenNumber, a.balance, m.bankName, m.department, m.companyName, m.address1, m.isActive, m.dateCreated " +
                           "FROM LAccount a " +
                           "LEFT JOIN LMember m ON a.memberId = m.id " +
                           "WHERE a.id LIKE @searchQuery OR " +
                           "a.memberId LIKE @searchQuery OR " +
                           "a.contractId LIKE @searchQuery OR " +
                           "a.accountName LIKE @searchQuery OR " +
                           "m.fullName LIKE @searchQuery OR " +
                           "a.accountType LIKE @searchQuery OR " +
                           "CONVERT(VARCHAR(20), a.balance) LIKE @searchQuery OR " +
                           "CONVERT(VARCHAR(20), a.rate) LIKE @searchQuery OR " +
                           "a.status LIKE @searchQuery OR " +
                           "m.phone LIKE @searchQuery OR " +
                           "m.email LIKE @searchQuery OR " +
                           "m.idenNumber LIKE @searchQuery OR " +
                           "m.bankName LIKE @searchQuery OR " +
                           "m.department LIKE @searchQuery OR " +
                           "m.companyName LIKE @searchQuery OR " +
                           "m.address1 LIKE @searchQuery";

            SqlParameter parameter = new SqlParameter("@searchQuery", "%" + searchQuery + "%");
            DataTable table = _exQuery.ExecuteRawQuery(query, new[] { parameter });

            return new JsonResult(table);
        }


    }
}
