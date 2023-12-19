﻿using Microsoft.AspNetCore.Mvc;
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

    }
}