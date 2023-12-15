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
        [Route("ShowTotalNumbers")]
        public JsonResult ShowTotalNumbers()
        {
            string query = @"
        SELECT
            (SELECT COUNT(*) FROM TKiosk) AS TotalKiosk,
            (SELECT COUNT(*) FROM TKiosk WHERE kioskStatus = 1) AS TotalKioskOnline,
            (SELECT COUNT(*) FROM TKiosk WHERE kioskStatus = 0) AS TotalKioskOffline,
            (SELECT COUNT(*) FROM LTransactionLog) AS TotalTransaction";

            DataTable table = ExecuteRawQuery(query);
            return new JsonResult(table);
        }


    }
}