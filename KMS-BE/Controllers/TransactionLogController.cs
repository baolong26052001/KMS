using Microsoft.AspNetCore.Mvc;
using KMS.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace KMS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionLogController : ControllerBase
    {
        private readonly KioskManagementSystemContext _dbcontext;
        private IConfiguration _configuration;

        public TransactionLogController(IConfiguration configuration, KioskManagementSystemContext _context)
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
        [Route("ShowTransactionLog")]
        public JsonResult GetTransactionLog()
        {
            string query = "select tl.transactionDate, tl.kioskId, tl.memberId, tl.transactionId, st.stationName, tl.transactionType, tl.status " +
                "from LTransactionLog tl, TKiosk k, LMember m, LAccount a, TStation st " +
                "where tl.kioskId = k.id and tl.memberId = m.id and tl.accountId = a.id and tl.stationId = st.id";
            DataTable table = ExecuteRawQuery(query);
            return new JsonResult(table);
        }

        

    }
}
