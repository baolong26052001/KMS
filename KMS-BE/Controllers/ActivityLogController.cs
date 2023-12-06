using Microsoft.AspNetCore.Mvc;
using KMS.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace KMS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ActivityLogController : ControllerBase
    {
        private readonly KioskManagementSystemContext _dbcontext;
        private IConfiguration _configuration;

        public ActivityLogController(IConfiguration configuration, KioskManagementSystemContext _context)
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
        [Route("ShowActivityLog")]
        public JsonResult GetActivityLog()
        {
            string query = "select ac.id, ac.kioskId, ac.hardwareName, ac.status, ac.stationId, ac.dateModified, ac.dateCreated, ac.isActive" +
                "\r\nfrom TActivityLog ac";
            DataTable table = ExecuteRawQuery(query);
            return new JsonResult(table);
        }

        [HttpGet]
        [Route("ShowActivityLog/{id}")]
        public JsonResult GetActivityLogById(int id)
        {
            string query = $"select ac.id, ac.kioskId, ac.hardwareName, ac.status, ac.stationId, ac.dateModified, ac.dateCreated, ac.isActive" +
                           $"\r\nFROM TActivityLog ac" +
                           $"\r\nWHERE ac.id = {id}";
            DataTable table = ExecuteRawQuery(query);
            return new JsonResult(table);
        }

        [HttpPost]
        [Route("AddActivityLog")]
        public JsonResult AddActivityLog([FromBody] TactivityLog activityLog)
        {
            // Assuming ActivityLog is the model for your TActivityLog table
            string query = "INSERT INTO TActivityLog (kioskId, hardwareName, status, stationId, dateModified, dateCreated, isActive) " +
                           "VALUES (@KioskId, @HardwareName, @Status, @StationId, @DateModified, @DateCreated, @IsActive)";

            SqlParameter[] parameters =
            {
                new SqlParameter("@KioskId", activityLog.KioskId),
                new SqlParameter("@HardwareName", activityLog.HardwareName),
                new SqlParameter("@Status", activityLog.Status),
                new SqlParameter("@StationId", activityLog.StationId),
                new SqlParameter("@DateModified", activityLog.DateModified),
                new SqlParameter("@DateCreated", activityLog.DateCreated),
                new SqlParameter("@IsActive", activityLog.IsActive),
            };

            ExecuteRawQuery(query, parameters);
            return new JsonResult("ActivityLog added successfully");
        }

        [HttpPut]
        [Route("UpdateActivityLog")]
        public JsonResult UpdateActivityLog([FromBody] TactivityLog activityLog)
        {
            string query = "UPDATE TActivityLog SET kioskId = @KioskId, hardwareName = @HardwareName, status = @Status, " +
                           "stationId = @StationId, dateModified = @DateModified, dateCreated = @DateCreated, isActive = @IsActive " +
                           "WHERE id = @Id";

            SqlParameter[] parameters =
            {
                new SqlParameter("@Id", activityLog.Id),
                new SqlParameter("@KioskId", activityLog.KioskId),
                new SqlParameter("@HardwareName", activityLog.HardwareName),
                new SqlParameter("@Status", activityLog.Status),
                new SqlParameter("@StationId", activityLog.StationId),
                new SqlParameter("@DateModified", activityLog.DateModified),
                new SqlParameter("@DateCreated", activityLog.DateCreated),
                new SqlParameter("@IsActive", activityLog.IsActive),
            };

            ExecuteRawQuery(query, parameters);
            return new JsonResult("ActivityLog updated successfully");
        }

        [HttpDelete]
        [Route("DeleteActivityLog")]
        public JsonResult DeleteActivityLog(int id)
        {
            string query = "DELETE FROM TActivityLog WHERE id = @Id";

            SqlParameter parameter = new SqlParameter("@Id", id);

            ExecuteRawQuery(query, new[] { parameter });
            return new JsonResult("ActivityLog deleted successfully");
        }

        [HttpGet]
        [Route("SearchActivityLog")]
        public JsonResult SearchActivityLog(string searchQuery)
        {
            string query = "SELECT id, kioskId, hardwareName, status, stationId, dateModified, dateCreated, isActive " +
                           "FROM TActivityLog " +
                           "WHERE id LIKE @searchQuery OR " +
                           "kioskId LIKE @searchQuery OR " +
                           "hardwareName LIKE @searchQuery OR " +
                           "status LIKE @searchQuery OR " +
                           "stationId LIKE @searchQuery OR " +
                           "CONVERT(VARCHAR(10), dateModified, 120) LIKE @searchQuery OR " +
                           "CONVERT(VARCHAR(10), dateCreated, 120) LIKE @searchQuery OR " +
                           "CAST(isActive AS VARCHAR) LIKE @searchQuery";

            SqlParameter parameter = new SqlParameter("@searchQuery", "%" + searchQuery + "%");
            DataTable table = ExecuteRawQuery(query, new[] { parameter });

            return new JsonResult(table);
        }

    }
}
