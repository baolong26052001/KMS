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
    public class ActivityLogController : ControllerBase
    {
        private readonly KioskManagementSystemContext _dbcontext;
        private IConfiguration _configuration;
        private readonly ExecuteQuery _exQuery;

        public ActivityLogController(IConfiguration configuration, KioskManagementSystemContext _context, ExecuteQuery exQuery)
        {
            _dbcontext = _context;
            _configuration = configuration;
            _exQuery = exQuery;
        }

        
        [HttpGet]
        [Route("ShowActivityLog")]
        //[Authorize]
        public IActionResult GetActivityLog()
        {
            ResponseDto response = new ResponseDto();
            try
            {
                //List<string> authorizedGroupNames = _exQuery.GetAuthorizedGroupNames("activitylogs", "canView");

                //var userRoles = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);
                //if (!_exQuery.IsUserAuthorized(authorizedGroupNames, userRoles))
                //{
                //    return Unauthorized();
                //}

                string query = "select ac.id, ac.kioskId, ac.hardwareName, ac.status, ac.stationId, ac.dateModified, ac.dateCreated, ac.isActive" +
                "\r\nfrom TActivityLog ac";
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
        [Route("ShowActivityLog/{id}")]
        public JsonResult GetActivityLogById(int id)
        {
            ResponseDto response = new ResponseDto();
            try
            {
                string query = $"select ac.id, ac.kioskId, ac.hardwareName, ac.status, ac.stationId, ac.dateModified, ac.dateCreated, ac.isActive" +
                           $"\r\nFROM TActivityLog ac" +
                           $"\r\nWHERE ac.id = {id}";
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
        [Route("FilterActivityLog")]
        public JsonResult FilterActivityLog([FromQuery] bool? isActive = null, [FromQuery] DateTime? startDate = null, [FromQuery] DateTime? endDate = null)
        {
            ResponseDto response = new ResponseDto();
            try
            {
                string query = "SELECT id, kioskId, hardwareName, status, stationId, dateModified, dateCreated, isActive " +
                           "FROM TActivityLog ";

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
        [Route("SaveActivityLog")]
        public JsonResult SaveActivityLog([FromBody] TactivityLog activityLog)
        {
            ResponseDto response = new ResponseDto();
            try
            {
                string query = "INSERT INTO TActivityLog (kioskId, hardwareName, status, stationId, dateModified, dateCreated, isActive) " +
                           "VALUES (@KioskId, @HardwareName, @Status, @StationId, GETDATE(), GETDATE(), @IsActive)";

                SqlParameter[] parameters =
                {
                    new SqlParameter("@KioskId", activityLog.KioskId),
                    new SqlParameter("@HardwareName", activityLog.HardwareName),
                    new SqlParameter("@Status", activityLog.Status),
                    new SqlParameter("@StationId", activityLog.StationId),
                    new SqlParameter("@IsActive", activityLog.IsActive),
                };

                _exQuery.ExecuteRawQuery(query, parameters);
                return new JsonResult("ActivityLog saved successfully");
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
        [Route("UpdateActivityLog")]
        public JsonResult UpdateActivityLog([FromBody] TactivityLog activityLog)
        {
            ResponseDto response = new ResponseDto();
            try
            {
                string query = "UPDATE TActivityLog SET kioskId = @KioskId, hardwareName = @HardwareName, status = @Status, " +
                           "stationId = @StationId, dateModified = GETDATE(), dateCreated = GETDATE(), isActive = @IsActive " +
                           "WHERE id = @Id";

                SqlParameter[] parameters =
                {
                    new SqlParameter("@Id", activityLog.Id),
                    new SqlParameter("@KioskId", activityLog.KioskId),
                    new SqlParameter("@HardwareName", activityLog.HardwareName),
                    new SqlParameter("@Status", activityLog.Status),
                    new SqlParameter("@StationId", activityLog.StationId),
                    new SqlParameter("@IsActive", activityLog.IsActive),
                };

                _exQuery.ExecuteRawQuery(query, parameters);
                return new JsonResult("ActivityLog updated successfully");
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
        [Route("SearchActivityLog")]
        public JsonResult SearchActivityLog(string searchQuery)
        {
            ResponseDto response = new ResponseDto();
            try
            {
                string query = "SELECT id, kioskId, hardwareName, status, stationId, dateModified, dateCreated, isActive " +
                           "FROM TActivityLog " +
                           "WHERE id LIKE @searchQuery OR " +
                           "kioskId LIKE @searchQuery OR " +
                           "hardwareName LIKE @searchQuery OR " +
                           "status LIKE @searchQuery OR " +
                           "stationId LIKE @searchQuery";

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
