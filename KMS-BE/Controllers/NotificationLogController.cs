using Microsoft.AspNetCore.Mvc;
using KMS.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Text;

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

        [HttpGet]
        [Route("FilterNotificationLog")]
        public JsonResult FilterNotificationLog([FromQuery] bool? isActive = null, [FromQuery] DateTime? startDate = null, [FromQuery] DateTime? endDate = null)
        {
            string query = "SELECT n.id, n.sendType, n.memberId, n.title, n.content, n.status, n.dateCreated, n.isActive " +
                           "FROM TNotificationLog n ";

            List<SqlParameter> parameters = new List<SqlParameter>();

            if (isActive.HasValue)
            {
                query += (parameters.Count == 0 ? " WHERE " : " AND ") + "isActive = @isActive";
                parameters.Add(new SqlParameter("@isActive", isActive.Value));
            }

            if (startDate.HasValue && endDate.HasValue)
            {
                query += (parameters.Count == 0 ? " WHERE " : " AND ") + "dateCreated BETWEEN @startDate AND @endDate";
                parameters.Add(new SqlParameter("@startDate", startDate.Value));
                parameters.Add(new SqlParameter("@endDate", endDate.Value));
            }

            DataTable table = ExecuteRawQuery(query, parameters.ToArray());

            return new JsonResult(table);
        }

        [HttpPost]
        [Route("AddNotificationLog")]
        public JsonResult AddNotificationLog(TnotificationLog newNotificationLog)
        {
            string query = "INSERT INTO TNotificationLog (type, sendType, memberId, title, content, status, dateCreated, isActive) " +
                           "VALUES (@type, @sendType, @memberId, @title, @content, @status, GETDATE(), @isActive)";

            SqlParameter[] parameters =
            {
                new SqlParameter("@type", newNotificationLog.Type),
                new SqlParameter("@sendType", newNotificationLog.SendType),
                new SqlParameter("@memberId", newNotificationLog.MemberId),
                new SqlParameter("@title", newNotificationLog.Title),
                new SqlParameter("@content", newNotificationLog.Content),
                new SqlParameter("@status", newNotificationLog.Status),
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
                           "status = @status, dateCreated = GETDATE(), isActive = @isActive " +
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
                new SqlParameter("@isActive", updatedNotificationLog.IsActive),
            };

            ExecuteRawQuery(query, parameters);

            return new JsonResult("NotificationLog updated successfully");
        }

        [HttpDelete]
        [Route("DeleteNotificationLog")]
        public JsonResult DeleteNotificationLog([FromBody] List<int> notificationLogIds)
        {
            if (notificationLogIds == null || notificationLogIds.Count == 0)
            {
                return new JsonResult("No notification log IDs provided for deletion");
            }

            StringBuilder deleteQuery = new StringBuilder("DELETE FROM TNotificationLog WHERE id IN (");


            List<SqlParameter> parameters = new List<SqlParameter>();
            for (int i = 0; i < notificationLogIds.Count; i++)
            {
                string parameterName = "@NotificationLogId" + i;
                deleteQuery.Append(parameterName);

                if (i < notificationLogIds.Count - 1)
                {
                    deleteQuery.Append(", ");
                }

                parameters.Add(new SqlParameter(parameterName, notificationLogIds[i]));
            }

            deleteQuery.Append(");");

            ExecuteRawQuery(deleteQuery.ToString(), parameters.ToArray());

            return new JsonResult("Notification log deleted successfully");
        }

        [HttpGet]
        [Route("SearchNotificationLog")]
        public JsonResult SearchNotificationLog(string searchQuery)
        {
            string query = "SELECT id, sendType, memberId, title, content, status, dateCreated, isActive " +
                           "FROM TNotificationLog " +
                           "WHERE id LIKE @searchQuery OR " +
                           "sendType LIKE @searchQuery OR " +
                           "memberId LIKE @searchQuery OR " +
                           "title LIKE @searchQuery OR " +
                           "content LIKE @searchQuery OR " +
                           "status LIKE @searchQuery";

            SqlParameter parameter = new SqlParameter("@searchQuery", "%" + searchQuery + "%");
            DataTable table = ExecuteRawQuery(query, new[] { parameter });

            return new JsonResult(table);
        }

    }
}
