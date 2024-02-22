using Microsoft.AspNetCore.Mvc;
using KMS.Models;
using KMS.Tools;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Text;
using System.Net.Sockets;
using System.Net;

namespace KMS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AgeRangeController : ControllerBase
    {
        private readonly KioskManagementSystemContext _dbcontext;
        private IConfiguration _configuration;
        private readonly ExecuteQuery _exQuery;

        public AgeRangeController(IConfiguration configuration, KioskManagementSystemContext _context, ExecuteQuery exQuery)
        {
            _dbcontext = _context;
            _configuration = configuration;
            _exQuery = exQuery;
        }

        [HttpGet]
        [Route("CheckAgeRange/{id}")] // id của member
        public JsonResult CheckAgeRange(int id)
        {
            ResponseDto response = new ResponseDto();
            try
            {
                string query = "SELECT lm.id, lm.fullName, DATEDIFF(DAY, lm.birthday, GETDATE()) / 365.25 AS age, " +
                "ar.id AS ageRangeId, N'Từ ' + CAST(ar.startAge AS NVARCHAR) + N' đến ' + CAST(ar.endAge AS NVARCHAR) + N' tuổi' AS description " +
                "FROM LMember lm " +
                "JOIN AgeRange ar ON ar.startAge <= (DATEDIFF(DAY, lm.birthday, GETDATE()) / 365.25) " +
                "and DATEDIFF(DAY, lm.birthday, GETDATE()) / 365.25 < ar.endAge + 1 and lm.id=@Id";

                SqlParameter parameter = new SqlParameter("@Id", id);
                DataTable table = _exQuery.ExecuteRawQuery(query, new[] { parameter });

                if (table.Rows.Count > 0)
                {
                    return new JsonResult(table);
                }
                else
                {
                    response.Code = 404;
                    response.Message = "Member ID not found, cannot check age range";
                    response.Exception = "";
                    response.Data = null;
                }
            }
            catch (Exception ex)
            {
                response.Code = 404;
                response.Message = ex.Message;
                response.Exception = "";
                response.Data = null;
            }
            return new JsonResult(response);
        }

        [HttpGet]
        [Route("ShowAgeRange")]
        public JsonResult GetAgeRange()
        {
            ResponseDto response = new ResponseDto();
            try
            {
                string query = "SELECT a.*, N'Từ ' + CAST(a.startAge AS NVARCHAR) + N' đến ' + CAST(a.endAge AS NVARCHAR) + N' tuổi' AS description FROM AgeRange a";

                DataTable table = _exQuery.ExecuteRawQuery(query);
                return new JsonResult(table);
            }
            catch (Exception ex)
            {
                response.Code = 404;
                response.Message = ex.Message;
                response.Exception = "";
                response.Data = null;
            }
            return new JsonResult(response);
        }

        [HttpGet]
        [Route("ShowAgeRange/{id}")]
        public JsonResult GetAgeRangeById(int id)
        {
            ResponseDto response = new ResponseDto();
            try
            {
                string query = "SELECT a.*,N'Từ ' + CAST(a.startAge AS NVARCHAR) + N' đến ' + CAST(a.endAge AS NVARCHAR) + N' tuổi' AS description FROM AgeRange a where a.id=@Id";

                SqlParameter parameter = new SqlParameter("@Id", id);
                DataTable table = _exQuery.ExecuteRawQuery(query, new[] { parameter });

                if (table.Rows.Count > 0)
                {
                    return new JsonResult(table);
                }
                else
                {
                    return new JsonResult("Age Range ID not found");
                }
            }
            catch (Exception ex)
            {
                response.Code = 404;
                response.Message = ex.Message;
                response.Exception = "";
                response.Data = null;
            }
            return new JsonResult(response);
        }

        [HttpPost]
        [Route("AddAgeRange")]
        public JsonResult AddAgeRange([FromBody] AgeRangeModel ageRange)
        {
            ResponseDto response = new ResponseDto();
            try
            {
                var ipAddress = HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault() ?? HttpContext.Connection.RemoteIpAddress.ToString();
                string host = Dns.GetHostName();
                IPHostEntry ip = Dns.GetHostEntry(host);
                IPAddress ipv4 = null;
                IPAddress ipv6 = null;

                foreach (var address in ip.AddressList)
                {
                    if (address.AddressFamily == AddressFamily.InterNetwork)
                    {
                        ipv4 = address;
                    }
                    else if (address.AddressFamily == AddressFamily.InterNetworkV6)
                    {
                        ipv6 = address;
                    }
                }

                string query = "INSERT INTO AgeRange (startAge, endAge, dateCreated, dateModified) " +
                           "VALUES (@StartAge, @EndAge, GETDATE(), GETDATE())";
                string query2 = "INSERT INTO TAudit (userId, ipAddress, macAddress, action, tableName, dateModified, dateCreated, isActive) " +
                           "VALUES (@UserId, @IpAddress, @Ipv6, 'Add', 'AgeRange', GETDATE(), GETDATE(), 1)";

                SqlParameter[] parameters =
                {
                    new SqlParameter("@StartAge", ageRange.StartAge),
                    new SqlParameter("@EndAge", ageRange.EndAge),
                };
                SqlParameter[] parameters2 =
                {
                    new SqlParameter("@IpAddress", ipv4?.ToString()),
                    new SqlParameter("@Ipv6", ipv6?.ToString()),
                    new SqlParameter("@UserId", (object)ageRange.UserId ?? DBNull.Value),
                };

                _exQuery.ExecuteRawQuery(query, parameters);
                _exQuery.ExecuteRawQuery(query2, parameters2);

                return new JsonResult("Age Range added successfully");
            }
            catch (Exception ex)
            {
                response.Code = 404;
                response.Message = ex.Message;
                response.Exception = "";
                response.Data = null;
            }
            return new JsonResult(response);
        }

        [HttpPut]
        [Route("EditAgeRange/{id}")]
        public JsonResult EditAgeRange(int id, [FromBody] AgeRangeModel ageRange)
        {
            ResponseDto response = new ResponseDto();
            try
            {
                var ipAddress = HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault() ?? HttpContext.Connection.RemoteIpAddress.ToString();
                string host = Dns.GetHostName();
                IPHostEntry ip = Dns.GetHostEntry(host);
                IPAddress ipv4 = null;
                IPAddress ipv6 = null;

                foreach (var address in ip.AddressList)
                {
                    if (address.AddressFamily == AddressFamily.InterNetwork)
                    {
                        ipv4 = address;
                    }
                    else if (address.AddressFamily == AddressFamily.InterNetworkV6)
                    {
                        ipv6 = address;
                    }
                }

                string query = "UPDATE AgeRange " +
                           "SET startAge = @StartAge, endAge = @EndAge, dateModified = GETDATE() " +
                           "WHERE id = @Id";
                string query2 = "INSERT INTO TAudit (userId, ipAddress, macAddress, action, tableName, dateModified, dateCreated, isActive) " +
                           "VALUES (@UserId, @IpAddress, @Ipv6, 'Update', 'AgeRange', GETDATE(), GETDATE(), 1)";

                SqlParameter[] parameters =
                {
                    new SqlParameter("@Id", id),
                    new SqlParameter("@StartAge", ageRange.StartAge),
                    new SqlParameter("@EndAge", ageRange.EndAge),
                };
                SqlParameter[] parameters2 =
                {
                    new SqlParameter("@IpAddress", ipv4?.ToString()),
                    new SqlParameter("@Ipv6", ipv6?.ToString()),
                    new SqlParameter("@UserId", (object)ageRange.UserId ?? DBNull.Value),
                };


                _exQuery.ExecuteRawQuery(query, parameters);
                _exQuery.ExecuteRawQuery(query2, parameters2);


                return new JsonResult("Age Range updated successfully");
            }
            catch (Exception ex)
            {
                response.Code = 404;
                response.Message = ex.Message;
                response.Exception = "";
                response.Data = null;
            }
            return new JsonResult(response);
        }

        [HttpDelete]
        [Route("DeleteAgeRange")]
        public JsonResult DeleteAgeRange([FromBody] List<int> ageRangeIds)
        {
            ResponseDto response = new ResponseDto();
            try
            {
                if (ageRangeIds == null || ageRangeIds.Count == 0)
                {
                    return new JsonResult("No age range IDs provided for deletion");
                }

                StringBuilder deleteQuery = new StringBuilder("DELETE FROM AgeRange WHERE id IN (");
                string query2 = "INSERT INTO TAudit (action, tableName, dateModified, dateCreated, isActive) VALUES ('Delete', 'AgeRange', GETDATE(), GETDATE(), 1)";

                List<SqlParameter> parameters = new List<SqlParameter>();
                SqlParameter[] parameters2 = { };
                for (int i = 0; i < ageRangeIds.Count; i++)
                {
                    string parameterName = "@AgeRangeId" + i;
                    deleteQuery.Append(parameterName);

                    if (i < ageRangeIds.Count - 1)
                    {
                        deleteQuery.Append(", ");
                    }

                    parameters.Add(new SqlParameter(parameterName, ageRangeIds[i]));
                }

                deleteQuery.Append(");");

                _exQuery.ExecuteRawQuery(deleteQuery.ToString(), parameters.ToArray());
                _exQuery.ExecuteRawQuery(query2, parameters2);

                return new JsonResult("Age Range deleted successfully");
            }
            catch (Exception ex)
            {
                response.Code = 404;
                response.Message = ex.Message;
                response.Exception = "";
                response.Data = null;
            }
            return new JsonResult(response);
        }

    }
}
