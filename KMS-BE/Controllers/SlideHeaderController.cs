using KMS.Models;
using KMS.Tools;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Text;
using System.Net.Sockets;
using Microsoft.Extensions.Configuration;
using System.Net;

namespace KMS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SlideHeaderController : ControllerBase
    {
        private readonly KioskManagementSystemContext _dbcontext;
        private IConfiguration _configuration;
        private readonly ExecuteQuery _exQuery;

        public SlideHeaderController(IConfiguration configuration, KioskManagementSystemContext _context, ExecuteQuery exQuery)
        {
            _dbcontext = _context;
            _configuration = configuration;
            _exQuery = exQuery;
        }

        [HttpGet]
        [Route("ShowSlideHeader")]
        public JsonResult GetSlideshow()
        {
            ResponseDto response = new ResponseDto();
            try
            {
                string query = "select sh.id, sh.description, sh.startDate, sh.endDate, sh.IsActive, sh.timeNext, sh.dateModified, sh.dateCreated" +
                "\r\nfrom TSlideHeader sh";
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
        [Route("ShowSlideHeader/{id}")]
        public JsonResult GetSlideshowById(int id)
        {
            ResponseDto response = new ResponseDto();
            try
            {
                string query = "SELECT * " +
                           "FROM TSlideHeader " +
                           "WHERE id = @Id";
                SqlParameter parameter = new SqlParameter("@Id", id);
                DataTable table = _exQuery.ExecuteRawQuery(query, new[] { parameter });

                if (table.Rows.Count > 0)
                {
                    return new JsonResult(table);
                }
                else
                {
                    return new JsonResult("Slideshow not found");
                }
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
        [Route("FilterSlideshow")]
        public JsonResult FilterSlideshow([FromQuery] DateTime? startDate = null, [FromQuery] DateTime? endDate = null)
        {
            ResponseDto response = new ResponseDto();
            try
            {
                string query = "SELECT sh.id, sh.description, sh.startDate, sh.endDate, sh.IsActive, sh.timeNext, sh.dateModified, sh.dateCreated " +
                           "FROM TSlideHeader sh ";

                List<SqlParameter> parameters = new List<SqlParameter>();


                if (startDate.HasValue && endDate.HasValue)
                {

                    startDate = startDate.Value.Date.AddHours(0).AddMinutes(0).AddSeconds(0);
                    endDate = endDate.Value.Date.AddHours(23).AddMinutes(59).AddSeconds(59);

                    query += (parameters.Count == 0 ? " WHERE " : " AND ") + "startDate >= @startDate AND endDate <= @endDate";
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
        [Route("AddSlideshow")]
        public JsonResult AddSlideshow([FromBody] TslideHeaderModel slideshow)
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

                string query = "INSERT INTO TSlideHeader (description, startDate, endDate, IsActive, timeNext, dateModified, dateCreated) " +
                           "VALUES (@Description, @StartDate, @EndDate, @IsActive, @TimeNext, GETDATE(), GETDATE())";
                string query2 = "INSERT INTO TAudit (userId, ipAddress, macAddress, action, tableName, dateModified, dateCreated, isActive) " +
                           "VALUES (@UserId, @IpAddress, @Ipv6, 'Add', 'TSlideHeader', GETDATE(), GETDATE(), 1)";

                DateTime startDate = (DateTime)slideshow.StartDate;
                DateTime endDate = (DateTime)slideshow.EndDate;


                if (slideshow.StartDate != null && slideshow.EndDate != null)
                {
                    startDate = slideshow.StartDate.Value.Date.AddHours(0).AddMinutes(0).AddSeconds(0);
                    endDate = slideshow.EndDate.Value.Date.AddHours(23).AddMinutes(59).AddSeconds(59);
                }

                SqlParameter[] parameters =
                {
                    new SqlParameter("@Description", slideshow.Description),
                    new SqlParameter("@StartDate", startDate),
                    new SqlParameter("@EndDate", endDate),
                    new SqlParameter("@IsActive", slideshow.IsActive),
                    new SqlParameter("@TimeNext", slideshow.TimeNext),
                };
                SqlParameter[] parameters2 =
                {
                    new SqlParameter("@IpAddress", ipv4?.ToString()),
                    new SqlParameter("@Ipv6", ipv6?.ToString()),
                    new SqlParameter("@UserId", (object)slideshow.UserId ?? DBNull.Value),
                };

                _exQuery.ExecuteRawQuery(query, parameters);
                _exQuery.ExecuteRawQuery(query2, parameters2);

                return new JsonResult("Slideshow added successfully");
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
        [Route("UpdateSlideshow/{id}")]
        public JsonResult UpdateSlideshow(int id, [FromBody] TslideHeaderModel slideshow)
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

                string query = "UPDATE TslideHeader SET description = @Description, dateModified = GETDATE(), IsActive = @IsActive, timeNext = @TimeNext, " +
                           "startDate = @StartDate, endDate = @EndDate " +
                           "WHERE id = @Id";
                string query2 = "INSERT INTO TAudit (userId, ipAddress, macAddress, action, tableName, dateModified, dateCreated, isActive) " +
                           "VALUES (@UserId, @IpAddress, @Ipv6, 'Update', 'TSlideHeader', GETDATE(), GETDATE(), 1)";

                DateTime startDate = (DateTime)slideshow.StartDate;
                DateTime endDate = (DateTime)slideshow.EndDate;


                if (slideshow.StartDate != null && slideshow.EndDate != null)
                {
                    startDate = slideshow.StartDate.Value.Date.AddHours(0).AddMinutes(0).AddSeconds(0);
                    endDate = slideshow.EndDate.Value.Date.AddHours(23).AddMinutes(59).AddSeconds(59);
                }

                SqlParameter[] parameters =
                {
                    new SqlParameter("@Id", id),
                    new SqlParameter("@Description", slideshow.Description),
                    new SqlParameter("@StartDate", startDate),
                    new SqlParameter("@EndDate", endDate),
                    new SqlParameter("@IsActive", slideshow.IsActive),
                    new SqlParameter("@TimeNext", slideshow.TimeNext),
                };
                SqlParameter[] parameters2 =
                {
                    new SqlParameter("@IpAddress", ipv4?.ToString()),
                    new SqlParameter("@Ipv6", ipv6?.ToString()),
                    new SqlParameter("@UserId", (object)slideshow.UserId ?? DBNull.Value),
                };

                _exQuery.ExecuteRawQuery(query, parameters);
                _exQuery.ExecuteRawQuery(query2, parameters2);

                return new JsonResult("Slideshow updated successfully");
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

        [HttpDelete]
        [Route("DeleteSlideshow")]
        public JsonResult DeleteSlideshow([FromBody] List<int> slideshowIds)
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


                if (slideshowIds == null || slideshowIds.Count == 0)
                {
                    return new JsonResult("No slideshow IDs provided for deletion");
                }

                StringBuilder deleteQuery = new StringBuilder("DELETE FROM TslideHeader WHERE id IN (");

                List<SqlParameter> parameters = new List<SqlParameter>();

                for (int i = 0; i < slideshowIds.Count; i++)
                {
                    string parameterName = "@SlideshowId" + i;
                    deleteQuery.Append(parameterName);

                    if (i < slideshowIds.Count - 1)
                    {
                        deleteQuery.Append(", ");
                    }

                    parameters.Add(new SqlParameter(parameterName, slideshowIds[i]));
                }

                deleteQuery.Append(");");

                _exQuery.ExecuteRawQuery(deleteQuery.ToString(), parameters.ToArray());

                return new JsonResult("Slideshow deleted successfully");
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
        [Route("SearchSlideshow")]
        public JsonResult SearchSlideshow(string searchQuery)
        {
            ResponseDto response = new ResponseDto();
            try
            {
                string query = "SELECT sh.id, sh.description, sh.startDate, sh.endDate, sh.IsActive, sh.timeNext, sh.dateModified, sh.dateCreated " +
                           "FROM TSlideHeader sh " +
                           "WHERE sh.id LIKE @searchQuery OR " +
                           "sh.description LIKE @searchQuery ";

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
