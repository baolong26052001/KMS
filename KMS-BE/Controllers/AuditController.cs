using Microsoft.AspNetCore.Mvc;
using KMS.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Text;
using KMS.Tools;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace KMS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuditController : ControllerBase
    {
        private readonly KioskManagementSystemContext _dbcontext;
        private IConfiguration _configuration;
        private readonly ExecuteQuery _exQuery;

        public AuditController(IConfiguration configuration, KioskManagementSystemContext _context, ExecuteQuery exQuery)
        {
            _dbcontext = _context;
            _configuration = configuration;
            _exQuery = exQuery;
        }

        [HttpGet]
        [Route("ShowAudit")]
        //[Authorize]
        public JsonResult GetAudit()
        {
            ResponseDto response = new ResponseDto();
            try
            {
                //List<string> authorizedGroupNames = _exQuery.GetAuthorizedGroupNames("audit", "canView");

                //var userRoles = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);
                //if (!_exQuery.IsUserAuthorized(authorizedGroupNames, userRoles))
                //{
                //    return Unauthorized();
                //}

                string query = "select au.id, au.kioskId, au.userId,au.action,au.script,au.field, au.tableName, au.ipAddress, au.macAddress, au.dateCreated, au.isActive " +
                "\r\nfrom TAudit au";
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
        [Route("ShowAudit/{id}")]
        public JsonResult GetAuditById(int id)
        {
            ResponseDto response = new ResponseDto();
            try
            {
                string query = "SELECT id, kioskId, userId, action, script, field, tableName, ipAddress, macAddress, dateCreated, isActive " +
                           "FROM TAudit WHERE id = @id";

                SqlParameter parameter = new SqlParameter("@id", id);
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

        [HttpGet]
        [Route("FilterAudit")]
        public JsonResult FilterAudit([FromQuery] bool? isActive = null, [FromQuery] DateTime? startDate = null, [FromQuery] DateTime? endDate = null)
        {
            ResponseDto response = new ResponseDto();
            try
            {
                string query = "SELECT id, kioskId, userId, action, script, field, tableName, ipAddress, macAddress, dateCreated, isActive " +
                           "FROM TAudit";

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
        [Route("AddAudit")]
        public JsonResult AddAudit(Taudit audit)
        {
            ResponseDto response = new ResponseDto();
            try
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

                _exQuery.ExecuteRawQuery(query, parameters);
                return new JsonResult("Audit record added successfully");
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
        [Route("UpdateAudit/{id}")]
        public JsonResult UpdateAudit(int id, Taudit audit)
        {
            string query = "UPDATE TAudit SET kioskId = @kioskId, userId = @userId, action = @action, script = @script, field = @field, " +
                           "tableName = @tableName, ipAddress = @ipAddress, macAddress = @macAddress, dateModified = GETDATE(), " +
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
                new SqlParameter("@isActive", audit.IsActive)
            };

            _exQuery.ExecuteRawQuery(query, parameters);
            return new JsonResult("Audit record updated successfully");
        }

        

        [HttpGet]
        [Route("SearchAudit")]
        public JsonResult SearchAudit(string searchQuery)
        {
            ResponseDto response = new ResponseDto();
            try
            {
                string query = "SELECT id, kioskId, userId, action, script, field, tableName, ipAddress, macAddress, dateCreated, isActive " +
                           "FROM TAudit " +
                           "WHERE id LIKE @searchQuery OR " +
                           "kioskId LIKE @searchQuery OR " +
                           "userId LIKE @searchQuery OR " +
                           "action LIKE @searchQuery OR " +
                           "script LIKE @searchQuery OR " +
                           "field LIKE @searchQuery OR " +
                           "tableName LIKE @searchQuery OR " +
                           "ipAddress LIKE @searchQuery OR " +
                           "macAddress LIKE @searchQuery";

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
