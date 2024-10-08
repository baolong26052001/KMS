﻿using Microsoft.AspNetCore.Mvc;
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
            ResponseDto response = new ResponseDto();
            try
            {
                string query = "SELECT tl.id, tl.transactionDate, tl.kioskId, k.kioskName, tl.memberId, m.fullName, tl.transactionId, st.stationName, tl.transactionType, tl.kioskRemainingMoney, tl.status " +
               "FROM LTransactionLog tl " +
               "LEFT JOIN TKiosk k ON tl.kioskId = k.id " +
               "LEFT JOIN LMember m ON tl.memberId = m.id " +
               "LEFT JOIN LAccount a ON tl.accountId = a.id " +
               "LEFT JOIN TStation st ON tl.stationId = st.id where m.fullName is not null " +
               "ORDER BY tl.transactionDate DESC";

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
        [Route("ShowTransactionLog/{id}")]
        public JsonResult GetTransactionLogById(int id)
        {
            ResponseDto response = new ResponseDto();
            try
            {
                string query = "SELECT tl.id, tl.transactionDate, tl.kioskId, k.kioskName, tl.memberId, m.fullName, tl.transactionId, st.stationName, tl.transactionType, tl.kioskRemainingMoney, tl.status " +
                "FROM LTransactionLog tl, TKiosk k, LMember m, LAccount a, TStation st " +
                "WHERE tl.kioskId = k.id AND tl.memberId = m.id AND tl.accountId = a.id AND tl.stationId = st.id AND tl.transactionId = @transactionId";

                SqlParameter parameter = new SqlParameter("@transactionId", id);
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
        [Route("FilterTransactionLog")]
        public JsonResult FilterTransactionLog([FromQuery] DateTime? startDate = null, [FromQuery] DateTime? endDate = null)
        {
            ResponseDto response = new ResponseDto();
            try
            {
                string query = "SELECT tl.id, tl.transactionDate, tl.kioskId, k.kioskName, tl.memberId, m.fullName, tl.transactionId, st.stationName, tl.transactionType, tl.kioskRemainingMoney, tl.status " +
               "FROM LTransactionLog tl " +
               "LEFT JOIN TKiosk k ON tl.kioskId = k.id " +
               "LEFT JOIN LMember m ON tl.memberId = m.id " +
               "LEFT JOIN LAccount a ON tl.accountId = a.id " +
               "LEFT JOIN TStation st ON tl.stationId = st.id ";

                List<SqlParameter> parameters = new List<SqlParameter>();

                //if (!string.IsNullOrEmpty(transactionType))
                //{
                //    query += " AND " + " tl.transactionType LIKE @transactionType ";
                //    parameters.Add(new SqlParameter("@transactionType", "%" + transactionType + "%"));
                //}

                //if (!string.IsNullOrEmpty(stationName))
                //{
                //    query += (parameters.Count == 0 ? " " : " AND ") + " st.stationName LIKE @stationName ";
                //    parameters.Add(new SqlParameter("@stationName", "%" + stationName + "%"));
                //}

                if (startDate.HasValue && endDate.HasValue)
                {

                    startDate = startDate.Value.Date.AddHours(0).AddMinutes(0).AddSeconds(0);
                    endDate = endDate.Value.Date.AddHours(23).AddMinutes(59).AddSeconds(59);

                    query += (parameters.Count == 0 ? " WHERE " : " AND ") + "tl.transactionDate >= @startDate AND tl.transactionDate <= @endDate";
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

        [HttpGet]
        [Route("SearchTransactionLog")]
        public JsonResult SearchTransactionLog(string searchQuery)
        {
            ResponseDto response = new ResponseDto();
            try
            {
                string query = "SELECT tl.id, tl.transactionDate, tl.kioskId, k.kioskName, tl.memberId, m.fullName, tl.transactionId, st.stationName, tl.transactionType, tl.kioskRemainingMoney, tl.status " +
               "FROM LTransactionLog tl " +
               "LEFT JOIN TKiosk k ON tl.kioskId = k.id " +
               "LEFT JOIN LMember m ON tl.memberId = m.id " +
               "LEFT JOIN LAccount a ON tl.accountId = a.id " +
               "LEFT JOIN TStation st ON tl.stationId = st.id " +
                "WHERE tl.kioskId LIKE @searchQuery OR " +
                "tl.memberId LIKE @searchQuery OR " +
                "k.kioskName LIKE @searchQuery OR " +
                "m.fullName LIKE @searchQuery OR " +
                "tl.transactionId LIKE @searchQuery OR " +
                "st.stationName LIKE @searchQuery OR " +
                "tl.transactionType LIKE @searchQuery OR " +
                "tl.status LIKE @searchQuery " +
                "ORDER BY tl.transactionDate DESC";

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



    }
}
