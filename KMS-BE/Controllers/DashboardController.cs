using Microsoft.AspNetCore.Mvc;
using KMS.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace KMS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController : ControllerBase
    {
        private readonly KioskManagementSystemContext _dbcontext;
        private IConfiguration _configuration;

        public DashboardController(IConfiguration configuration, KioskManagementSystemContext _context)
        {
            _dbcontext = _context;
            _configuration = configuration;
        }

        private DataTable ExecuteRawQuery(string query, SqlParameter[] parameters = null)
        {
            DataTable table = new DataTable();

            using (SqlConnection myCon = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                myCon.Open();

                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    if (parameters != null)
                    {
                        myCommand.Parameters.AddRange(parameters);
                    }

                    SqlDataReader myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                }
            }

            return table;
        }

        [HttpGet]
        [Route("ShowTotalNumberKiosk")]
        public JsonResult ShowTotalNumberKiosk()
        {
            string query = "select count(*) as TotalKiosk from TKiosk";
            DataTable table = ExecuteRawQuery(query);
            return new JsonResult(table);
        }

        [HttpGet]
        [Route("ShowTotalNumberKioskOnline")]
        public JsonResult ShowTotalNumberKioskOnline()
        {
            string query = "select count(*) as TotalKioskOnline from TKiosk k where k.kioskStatus = 1";
            DataTable table = ExecuteRawQuery(query);
            return new JsonResult(table);
        }

        [HttpGet]
        [Route("ShowTotalNumberKioskOffline")]
        public JsonResult ShowTotalNumberKioskOffline()
        {
            string query = "select count(*) as TotalKioskOffline from TKiosk k where k.kioskStatus = 0";
            DataTable table = ExecuteRawQuery(query);
            return new JsonResult(table);
        }

        [HttpGet]
        [Route("ShowTotalNumberTransaction")]
        public JsonResult ShowTotalNumberTransaction()
        {
            string query = "select count(*) as TotalTransaction from LTransactionLog";
            DataTable table = ExecuteRawQuery(query);
            return new JsonResult(table);
        }

    }
}