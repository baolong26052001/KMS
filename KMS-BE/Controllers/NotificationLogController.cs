using Microsoft.AspNetCore.Mvc;
using KMS.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace KMS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationLogController : ControllerBase
    {
        private readonly KioskManagementSystemContext _dbcontext;
        private IConfiguration _configuration;

        public NotificationLogController(IConfiguration configuration, KioskManagementSystemContext _context)
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
        [Route("ShowNotificationLog")]
        public JsonResult GetNotificationLog()
        {
            string query = "select n.id, n.sendType, n.memberId, n.title, n.content, n.status, n.dateCreated, n.isActive" +
                "\r\nfrom TNotificationLog n";
            DataTable table = ExecuteRawQuery(query);
            return new JsonResult(table);
        }

        [HttpGet]
        [Route("ShowNotificationLog/{id}")]
        public JsonResult GetNotificationLogById(int id)
        {
            string query = $"SELECT n.id, n.sendType, n.memberId, n.title, n.content, n.status, n.dateCreated, n.isActive" +
                           $"\r\nFROM TNotificationLog n" +
                           $"\r\nWHERE n.id = {id}";
            DataTable table = ExecuteRawQuery(query);
            return new JsonResult(table);
        }

        [HttpPost]
        [Route("AddNotificationLog")]
        public JsonResult AddNotificationLog(TnotificationLog newNotificationLog)
        {
            string query = "INSERT INTO TNotificationLog (type, sendType, memberId, title, content, status, dateCreated, isActive) " +
                           "VALUES (@type, @sendType, @memberId, @title, @content, @status, @dateCreated, @isActive)";

            SqlParameter[] parameters =
            {
                new SqlParameter("@type", newNotificationLog.Type),
                new SqlParameter("@sendType", newNotificationLog.SendType),
                new SqlParameter("@memberId", newNotificationLog.MemberId),
                new SqlParameter("@title", newNotificationLog.Title),
                new SqlParameter("@content", newNotificationLog.Content),
                new SqlParameter("@status", newNotificationLog.Status),
                new SqlParameter("@dateCreated", newNotificationLog.DateCreated),
                new SqlParameter("@isActive", newNotificationLog.IsActive),
            };

            ExecuteRawQuery(query, parameters);

            return new JsonResult("NotificationLog added successfully");
        }

        [HttpPut]
        [Route("UpdateNotificationLog/{id}")]
        public JsonResult UpdateNotificationLog(int id, TnotificationLog updatedNotificationLog)
        {
            string query = "UPDATE TNotificationLog SET " +
                           "type=@type, sendType = @sendType, memberId = @memberId, title = @title, content = @content, " +
                           "status = @status, dateCreated = @dateCreated, isActive = @isActive " +
                           "WHERE id = @id";

            SqlParameter[] parameters =
            {
                new SqlParameter("@id", id),
                new SqlParameter("@type", updatedNotificationLog.Type),
                new SqlParameter("@sendType", updatedNotificationLog.SendType),
                new SqlParameter("@memberId", updatedNotificationLog.MemberId),
                new SqlParameter("@title", updatedNotificationLog.Title),
                new SqlParameter("@content", updatedNotificationLog.Content),
                new SqlParameter("@status", updatedNotificationLog.Status),
                new SqlParameter("@dateCreated", updatedNotificationLog.DateCreated),
                new SqlParameter("@isActive", updatedNotificationLog.IsActive),
            };

            ExecuteRawQuery(query, parameters);

            return new JsonResult("NotificationLog updated successfully");
        }

        [HttpDelete]
        [Route("DeleteNotificationLog/{id}")]
        public JsonResult DeleteNotificationLog(int id)
        {
            string query = "DELETE FROM TNotificationLog WHERE id = @id";

            SqlParameter[] parameters =
            {
                new SqlParameter("@id", id),
            };

            ExecuteRawQuery(query, parameters);

            return new JsonResult("NotificationLog deleted successfully");
        }

        [HttpGet]
        [Route("SearchNotificationLog")]
        public JsonResult SearchNotificationLog(string searchQuery)
        {
            string query = "SELECT id, sendType, memberId, title, content, status, dateCreated, isActive " +
                           "FROM TNotificationLog " +
                           "WHERE sendType LIKE @searchQuery OR " +
                           "memberId LIKE @searchQuery OR " +
                           "title LIKE @searchQuery OR " +
                           "content LIKE @searchQuery OR " +
                           "status LIKE @searchQuery OR " +
                           "CONVERT(VARCHAR(10), dateCreated, 120) LIKE @searchQuery OR " +
                           "CAST(isActive AS VARCHAR) LIKE @searchQuery";

            SqlParameter parameter = new SqlParameter("@searchQuery", "%" + searchQuery + "%");
            DataTable table = ExecuteRawQuery(query, new[] { parameter });

            return new JsonResult(table);
        }

    }
}
