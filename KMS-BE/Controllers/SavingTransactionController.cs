﻿using Microsoft.AspNetCore.Mvc;
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
            string query = "select * from SavingTransaction";

            DataTable table = _exQuery.ExecuteRawQuery(query);
            return new JsonResult(table);
        }

        [HttpGet]
        [Route("ShowSavingTransaction/{id}")]
        public JsonResult GetSavingTransactionById(int id)
        {
            string query = "select * from SavingTransaction where id=@id";

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

        [HttpGet]
        [Route("ShowSavingTransactionDetail/{id}")]
        public JsonResult GetSavingTransactionDetail(int id)
        {
            string query = "select * " +
                "from SavingTransactionDetail std, SavingTransaction st " +
                "where std.savingId = st.id and st.id=@id";

            SqlParameter parameter = new SqlParameter("@id", id);
            DataTable table = _exQuery.ExecuteRawQuery(query, new[] { parameter });

            if (table.Rows.Count > 0)
            {
                return new JsonResult(table);
            }
            else
            {
                return new JsonResult("Saving transaction detail not found");
            }
        }

        [HttpGet]
        [Route("FilterSavingTransaction")]
        public JsonResult FilterSavingTransaction([FromQuery] bool? isActive = null, [FromQuery] DateTime? startDate = null, [FromQuery] DateTime? endDate = null)
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

    }
}