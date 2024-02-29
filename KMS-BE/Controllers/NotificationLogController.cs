using Microsoft.AspNetCore.Mvc;
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
    public class NotificationLogController : ControllerBase
    {
        private readonly KioskManagementSystemContext _dbcontext;
        private IConfiguration _configuration;
        private readonly ExecuteQuery _exQuery;

        public NotificationLogController(IConfiguration configuration, KioskManagementSystemContext _context, ExecuteQuery exQuery)
        {
            _dbcontext = _context;
            _configuration = configuration;
            _exQuery = exQuery;
        }

        [HttpGet]
        [Route("ShowNotificationLog")]
        public JsonResult GetNotificationLog()
        {
            ResponseDto response = new ResponseDto();
            try
            {
                string query = "select n.id, n.sendType, n.memberId, n.title, n.content, n.status, n.dateCreated, n.isActive" +
                "\r\nfrom TNotificationLog n";
                DataTable table = _exQuery.ExecuteRawQuery(query);
                return new JsonResult(table);
            }
            catch (Exception ex)
            {
                response.Code = -1;
                response.Message = ex.Message;
                response.Exception = ex.ToString();
                response.Data = null;
            }
            return new JsonResult(response);
            
        }

        [HttpGet]
        [Route("ShowNotificationLog/{id}")]
        public JsonResult GetNotificationLogById(int id)
        {
            ResponseDto response = new ResponseDto();
            try
            {
                string query = $"SELECT n.id, n.sendType, n.memberId, n.title, n.content, n.status, n.dateCreated, n.isActive" +
                           $"\r\nFROM TNotificationLog n" +
                           $"\r\nWHERE n.id = {id}";
                DataTable table = _exQuery.ExecuteRawQuery(query);
                return new JsonResult(table);
            }
            catch (Exception ex)
            {
                response.Code = -1;
                response.Message = ex.Message;
                response.Exception = ex.ToString();
                response.Data = null;
            }
            return new JsonResult(response);
            
        }

        [HttpGet]
        [Route("FilterNotificationLog")]
        public JsonResult FilterNotificationLog([FromQuery] bool? isActive = null, [FromQuery] DateTime? startDate = null, [FromQuery] DateTime? endDate = null)
        {
            ResponseDto response = new ResponseDto();
            try
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
                    startDate = startDate.Value.Date.AddHours(0).AddMinutes(0).AddSeconds(0);
                    endDate = endDate.Value.Date.AddHours(23).AddMinutes(59).AddSeconds(59);

                    query += (parameters.Count == 0 ? " WHERE " : " AND ") + "dateCreated BETWEEN @startDate AND @endDate";
                    parameters.Add(new SqlParameter("@startDate", startDate.Value));
                    parameters.Add(new SqlParameter("@endDate", endDate.Value));
                }

                DataTable table = _exQuery.ExecuteRawQuery(query, parameters.ToArray());

                return new JsonResult(table);
            }
            catch (Exception ex)
            {
                response.Code = -1;
                response.Message = ex.Message;
                response.Exception = ex.ToString();
                response.Data = null;
            }
            return new JsonResult(response);
            
        }

        [HttpPost]
        [Route("SaveNotificationLog")]
        public JsonResult SaveNotificationLog(TnotificationLogModel newNotificationLog)
        {
            ResponseDto response = new ResponseDto();
            try
            {
                string query = "INSERT INTO TNotificationLog (sendType, memberId, title, content, status, dateCreated, isActive) " +
                           "VALUES (@sendType, @memberId, @title, @content, @status, GETDATE(), @isActive)";

                SqlParameter[] parameters =
                {
                    
                    new SqlParameter("@sendType", newNotificationLog.SendType),
                    new SqlParameter("@memberId", newNotificationLog.MemberId),
                    new SqlParameter("@title", newNotificationLog.Title),
                    new SqlParameter("@content", newNotificationLog.Content),
                    new SqlParameter("@status", newNotificationLog.Status),
                    new SqlParameter("@isActive", newNotificationLog.IsActive),
                };

                _exQuery.ExecuteRawQuery(query, parameters);

                return new JsonResult("NotificationLog saved successfully");
            }
            catch (Exception ex)
            {
                response.Code = -1;
                response.Message = ex.Message;
                response.Exception = ex.ToString();
                response.Data = null;
            }
            return new JsonResult(response);
            
        }

        [HttpPut]
        [Route("UpdateNotificationLog/{id}")]
        public JsonResult UpdateNotificationLog(int id, TnotificationLog updatedNotificationLog)
        {
            ResponseDto response = new ResponseDto();
            try
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

                _exQuery.ExecuteRawQuery(query, parameters);

                return new JsonResult("NotificationLog updated successfully");
            }
            catch (Exception ex)
            {
                response.Code = -1;
                response.Message = ex.Message;
                response.Exception = ex.ToString();
                response.Data = null;
            }
            return new JsonResult(response);
            
        }

        

        [HttpGet]
        [Route("SearchNotificationLog")]
        public JsonResult SearchNotificationLog(string searchQuery)
        {
            ResponseDto response = new ResponseDto();
            try
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
                DataTable table = _exQuery.ExecuteRawQuery(query, new[] { parameter });

                return new JsonResult(table);
            }
            catch (Exception ex)
            {
                response.Code = -1;
                response.Message = ex.Message;
                response.Exception = ex.ToString();
                response.Data = null;
            }
            return new JsonResult(response);
            
        }

    }
}
