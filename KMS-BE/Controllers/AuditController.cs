using Microsoft.AspNetCore.Mvc;
using KMS.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace KMS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuditController : ControllerBase
    {
        private readonly KioskManagementSystemContext _dbcontext;
        private IConfiguration _configuration;

        public AuditController(IConfiguration configuration, KioskManagementSystemContext _context)
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
        [Route("ShowAudit")]
        public JsonResult GetAudit()
        {
            string query = "select au.id, au.kioskId, au.userId,au.action,au.script,au.field, au.tableName, au.ipAddress, au.macAddress, au.dateCreated, au.isActive " +
                "\r\nfrom TAudit au";
            DataTable table = ExecuteRawQuery(query);
            return new JsonResult(table);
        }

        [HttpGet]
        [Route("ShowAudit/{id}")]
        public JsonResult GetAuditById(int id)
        {
            string query = "SELECT id, kioskId, userId, action, script, field, tableName, ipAddress, macAddress, dateCreated, isActive " +
                           "FROM TAudit WHERE id = @id";

            SqlParameter parameter = new SqlParameter("@id", id);
            DataTable table = ExecuteRawQuery(query, new[] { parameter });
            return new JsonResult(table);
        }


        [HttpPost]
        [Route("AddAudit")]
        public JsonResult AddAudit(Taudit audit)
        {
            string query = "INSERT INTO TAudit (kioskId, userId, action, script, field, tableName, ipAddress, macAddress, dateCreated, isActive) " +
                           "VALUES (@kioskId, @userId, @action, @script, @field, @tableName, @ipAddress, @macAddress, DATEADD(HOUR, 7, GETDATE()), @isActive)";

            SqlParameter[] parameters =
            {
                new SqlParameter("@kioskId", audit.KioskId),
                new SqlParameter("@userId", audit.UserId),
                new SqlParameter("@action", audit.Action),
                new SqlParameter("@script", audit.Script),
                new SqlParameter("@field", audit.Field),
                new SqlParameter("@tableName", audit.TableName),
                new SqlParameter("@ipAddress", audit.IpAddress),
                new SqlParameter("@macAddress", audit.MacAddress),
                
                new SqlParameter("@isActive", audit.IsActive)
            };

            ExecuteRawQuery(query, parameters);
            return new JsonResult("Audit record added successfully");
        }

        [HttpPut]
        [Route("UpdateAudit/{id}")]
        public JsonResult UpdateAudit(int id, Taudit audit)
        {
            string query = "UPDATE TAudit SET kioskId = @kioskId, userId = @userId, action = @action, script = @script, field = @field, " +
                           "tableName = @tableName, ipAddress = @ipAddress, macAddress = @macAddress, dateModified = @dateModified, " +
                           "isActive = @isActive WHERE id = @id";

            SqlParameter[] parameters =
            {
                new SqlParameter("@id", id),
                new SqlParameter("@kioskId", audit.KioskId),
                new SqlParameter("@userId", audit.UserId),
                new SqlParameter("@action", audit.Action),
                new SqlParameter("@script", audit.Script),
                new SqlParameter("@field", audit.Field),
                new SqlParameter("@tableName", audit.TableName),
                new SqlParameter("@ipAddress", audit.IpAddress),
                new SqlParameter("@macAddress", audit.MacAddress),
                new SqlParameter("@dateModified", audit.DateModified),
                new SqlParameter("@isActive", audit.IsActive)
            };

            ExecuteRawQuery(query, parameters);
            return new JsonResult("Audit record updated successfully");
        }

        [HttpDelete]
        [Route("DeleteAudit/{id}")]
        public JsonResult DeleteAudit(int id)
        {
            string query = "DELETE FROM TAudit WHERE id = @id";

            SqlParameter parameter = new SqlParameter("@id", id);

            ExecuteRawQuery(query, new SqlParameter[] { parameter });
            return new JsonResult("Audit record deleted successfully");
        }


    }
}
