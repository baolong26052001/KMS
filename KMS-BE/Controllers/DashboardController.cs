using Microsoft.AspNetCore.Mvc;
using KMS.Models;
using Microsoft.Data.SqlClient;
using System.Data;
using KMS.Tools;

namespace KMS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController : ControllerBase
    {
        private readonly KioskManagementSystemContext _dbcontext;
        private IConfiguration _configuration;
        private readonly ExecuteQuery _exQuery;

        public DashboardController(IConfiguration configuration, KioskManagementSystemContext _context, ExecuteQuery exQuery)
        {
            _dbcontext = _context;
            _configuration = configuration;
            _exQuery = exQuery;
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
        (SELECT COUNT(*) FROM LoanTransaction) +
        (SELECT COUNT(*) FROM SavingTransaction) +
        (SELECT COUNT(*) FROM InsuranceTransaction) AS TotalTransaction";

            DataTable table = _exQuery.ExecuteRawQuery(query);
            return new JsonResult(table);
        }


    }
}