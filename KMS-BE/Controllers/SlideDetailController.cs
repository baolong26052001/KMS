using KMS.Models;
using KMS.Tools;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;
using System;
using System.Net.Sockets;
using System.Net;

namespace KMS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SlideDetailController : ControllerBase
    {
        private readonly KioskManagementSystemContext _dbcontext;
        private IConfiguration _configuration;
        private readonly ExecuteQuery _exQuery;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly string localFolderPath;

        public SlideDetailController(IWebHostEnvironment hostingEnvironment, IConfiguration configuration, KioskManagementSystemContext _context, ExecuteQuery exQuery)
        {
            _hostingEnvironment = hostingEnvironment;
            _dbcontext = _context;
            _configuration = configuration;
            _exQuery = exQuery;

            localFolderPath = "../KMS_BE/bin/Debug/net6.0/images/";
        }

        [HttpGet]
        [Route("ShowSlideDetail/{id}")]
        public JsonResult GetSlideDetailById(int id) // show by header id
        {
            ResponseDto response = new ResponseDto();
            try
            {
                string query = "select sd.id, sd.description, sd.typeContent, sd.imageBase64, sd.sequence, sd.contentUrl, sd.slideHeaderId, sd.isActive, sd.dateModified, sd.dateCreated " +
                "from TSlideDetail sd " +
                "WHERE sd.slideHeaderId = @Id ORDER BY sd.sequence ASC";

                SqlParameter parameter = new SqlParameter("@Id", id);
                DataTable table = _exQuery.ExecuteRawQuery(query, new[] { parameter });

                if (table.Rows.Count > 0)
                {
                    return new JsonResult(table);
                }
                else
                {
                    return new JsonResult("SlideDetail not found");
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
        [Route("ShowSlideDetailInEditPage/{id}")]
        public JsonResult GetSlideDetailInEditPage(int id) // show data in edit page
        {
            ResponseDto response = new ResponseDto();
            try
            {
                string query = "select sd.id, sd.description, sd.typeContent, sd.imageBase64, sd.sequence, sd.contentUrl, sd.slideHeaderId, sd.isActive, sd.dateModified, sd.dateCreated " +
                "\r\nfrom TSlideDetail sd " +
                "WHERE sd.id = @Id";
                SqlParameter parameter = new SqlParameter("@Id", id);
                DataTable table = _exQuery.ExecuteRawQuery(query, new[] { parameter });

                if (table.Rows.Count > 0)
                {
                    return new JsonResult(table);
                }
                else
                {
                    return new JsonResult("SlideDetail not found");
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

        [HttpPost]
        [Route("AddSlideDetail")]
        public JsonResult AddSlideDetail([FromForm] TslideDetailModel slideDetail)
        {
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

                byte[] imageByte;
                if (slideDetail.File != null && slideDetail.File.Length > 0)
                {
                    // Read the binary data of the image file
                    using (var memoryStream = new MemoryStream())
                    {
                        slideDetail.File.CopyTo(memoryStream);
                        imageByte = memoryStream.ToArray(); // Assign the binary data to the ImageData property
                    }

                    string base64String = Convert.ToBase64String(imageByte);
                    slideDetail.ImageBase64 = base64String;

                    //var uniqueFileName = Guid.NewGuid().ToString() + "_" + slideDetail.File.FileName;
                    var uniqueFileName = slideDetail.File.FileName;
                    var filePath = Path.Combine(localFolderPath, uniqueFileName);

                    if (!Directory.Exists(localFolderPath))
                    {
                        Directory.CreateDirectory(localFolderPath);
                    }

                    // Save the uploaded file to the local folder
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        slideDetail.File.CopyTo(stream);
                    }

                    slideDetail.ContentUrl = uniqueFileName;
                }


                else
                {
                    return new JsonResult("Cannot detect image file");
                }

                
                string query = "INSERT INTO TSlideDetail (sequence, description, typeContent, contentUrl, imageBase64, slideHeaderId, dateModified, dateCreated, isActive) " +
                               "VALUES (@Sequence, @Description, @TypeContent, @ContentUrl, @ImageBase64, @SlideHeaderId, GETDATE(), GETDATE(), 1)";
                string query2 = "INSERT INTO TAudit (userId, ipAddress, macAddress, action, tableName, dateModified, dateCreated, isActive) " +
                           "VALUES (@UserId, @IpAddress, @Ipv6, 'Add', 'TSlideDetail', GETDATE(), GETDATE(), 1)";

                SqlParameter[] parameters =
                {
                    new SqlParameter("@Sequence", slideDetail.Sequence),
                    new SqlParameter("@Description", slideDetail.Description),
                    new SqlParameter("@TypeContent", slideDetail.TypeContent),
                    new SqlParameter("@ContentUrl", slideDetail.ContentUrl),
                    
                    new SqlParameter("@ImageBase64", slideDetail.ImageBase64),
                    new SqlParameter("@SlideHeaderId", slideDetail.SlideHeaderId),
                    new SqlParameter("@IsActive", slideDetail.IsActive),
                };
                SqlParameter[] parameters2 =
                {
                    new SqlParameter("@IpAddress", ipv4?.ToString()),
                    new SqlParameter("@Ipv6", ipv6?.ToString()),
                    new SqlParameter("@UserId", (object)slideDetail.UserId ?? DBNull.Value),
                };

                _exQuery.ExecuteRawQuery(query, parameters);
                _exQuery.ExecuteRawQuery(query2, parameters2);

                return new JsonResult("Slide detail added successfully");
            }
            catch (Exception ex)
            {
                
                return new JsonResult($"Error adding slide detail: {ex.Message}");
            }
        }

        [HttpPut]
        [Route("UpdateSlideDetail/{id}")]
        public JsonResult UpdateSlideDetail(int id, [FromForm] TslideDetailModel slideDetail)
        {
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

                string oldContentUrl = GetOldContentUrl(id);

                byte[] imageByte;
                if (slideDetail.File != null && slideDetail.File.Length > 0)
                {

                    using (var memoryStream = new MemoryStream())
                    {
                        slideDetail.File.CopyTo(memoryStream);
                        imageByte = memoryStream.ToArray(); // Assign the binary data to the ImageData property
                    }

                    string base64String = Convert.ToBase64String(imageByte);
                    slideDetail.ImageBase64 = base64String;

                    //var uniqueFileName = Guid.NewGuid().ToString() + "_" + slideDetail.File.FileName;
                    var uniqueFileName = slideDetail.File.FileName;
                    var filePath = Path.Combine(localFolderPath, uniqueFileName);

                    
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        slideDetail.File.CopyTo(stream);
                    }

                    
                    slideDetail.ContentUrl = uniqueFileName;

                    var existingFilePath = Path.Combine(localFolderPath, oldContentUrl);

                    if (System.IO.File.Exists(existingFilePath))
                    {
                        System.IO.File.Delete(existingFilePath);
                    }

                    string query = "UPDATE TSlideDetail SET sequence = @Sequence, description = @Description, imageBase64 = @ImageBase64, typeContent = @TypeContent, contentUrl = @ContentUrl, slideHeaderId = @SlideHeaderId, dateModified = GETDATE(), isActive = @IsActive  " +
                            "WHERE id = @Id";
                    string query2 = "INSERT INTO TAudit (userId, ipAddress, macAddress, action, tableName, dateModified, dateCreated, isActive) " +
                           "VALUES (@UserId, @IpAddress, @Ipv6, 'Update', 'TSlideDetail', GETDATE(), GETDATE(), 1)";

                    SqlParameter[] parameters =
                    {
                        new SqlParameter("@Id", id),
                        new SqlParameter("@Sequence", slideDetail.Sequence),
                        new SqlParameter("@Description", slideDetail.Description),
                        new SqlParameter("@TypeContent", slideDetail.TypeContent),
                        
                        new SqlParameter("@ImageBase64", slideDetail.ImageBase64),
                        new SqlParameter("@ContentUrl", slideDetail.ContentUrl),
                        new SqlParameter("@SlideHeaderId", slideDetail.SlideHeaderId),
                        new SqlParameter("@IsActive", slideDetail.IsActive),
                    };
                    SqlParameter[] parameters2 =
                    {
                        new SqlParameter("@IpAddress", ipv4?.ToString()),
                        new SqlParameter("@Ipv6", ipv6?.ToString()),
                        new SqlParameter("@UserId", (object)slideDetail.UserId ?? DBNull.Value),
                    };

                    _exQuery.ExecuteRawQuery(query, parameters);
                    _exQuery.ExecuteRawQuery(query2, parameters2);

                    return new JsonResult("Slide detail updated successfully");

                }

                else
                {
                    string query = "UPDATE TSlideDetail SET sequence = @Sequence, description = @Description, typeContent = @TypeContent, contentUrl = @ContentUrl, slideHeaderId = @SlideHeaderId, dateModified = GETDATE(), isActive = @IsActive  " +
                            "WHERE id = @Id";
                    string query2 = "INSERT INTO TAudit (userId, ipAddress, macAddress, action, tableName, dateModified, dateCreated, isActive) " +
                           "VALUES (@UserId, @IpAddress, @Ipv6, 'Update', 'TSlideDetail', GETDATE(), GETDATE(), 1)";

                    SqlParameter[] parameters =
                    {
                        new SqlParameter("@Id", id),
                        new SqlParameter("@Sequence", slideDetail.Sequence),
                        new SqlParameter("@Description", slideDetail.Description),
                        new SqlParameter("@TypeContent", slideDetail.TypeContent),
                        new SqlParameter("@ContentUrl", slideDetail.ContentUrl),
                        new SqlParameter("@SlideHeaderId", slideDetail.SlideHeaderId),
                        new SqlParameter("@IsActive", slideDetail.IsActive),
                    };
                    SqlParameter[] parameters2 =
                    {
                        new SqlParameter("@IpAddress", ipv4?.ToString()),
                        new SqlParameter("@Ipv6", ipv6?.ToString()),
                        new SqlParameter("@UserId", (object)slideDetail.UserId ?? DBNull.Value),
                    };

                    _exQuery.ExecuteRawQuery(query, parameters);
                    _exQuery.ExecuteRawQuery(query2, parameters2);

                    return new JsonResult("Slide detail updated successfully");
                }

            }
            catch (Exception ex)
            {
                
                return new JsonResult($"Error adding slide detail: {ex.Message}");
            }
            
        }

        private string GetOldContentUrl(int id)
        {
            string query = "SELECT ContentUrl FROM TSlideDetail WHERE id = @Id";
            SqlParameter parameter = new SqlParameter("@Id", id);
            DataTable table = _exQuery.ExecuteRawQuery(query, new[] { parameter });

            if (table.Rows.Count > 0)
            {
                return table.Rows[0]["ContentUrl"].ToString();
            }
            else
            {
                
                return null;
            }
        }

        [HttpGet]
        [Route("FilterSlideDetail/{id}")]
        public JsonResult FilterSlideDetail(int id, [FromQuery] DateTime? startDate = null, [FromQuery] DateTime? endDate = null)
        {
            ResponseDto response = new ResponseDto();
            try
            {
                string query = "select sd.id, sd.description, sd.typeContent, sd.imageBase64, sd.sequence, sd.contentUrl, sd.slideHeaderId, sd.isActive, sd.dateModified, sd.dateCreated " +
                           "FROM TSlideDetail sd " +
                           "WHERE slideHeaderId = @Id";

                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@Id", id));


                if (startDate.HasValue && endDate.HasValue)
                {

                    startDate = startDate.Value.Date.AddHours(0).AddMinutes(0).AddSeconds(0);
                    endDate = endDate.Value.Date.AddHours(23).AddMinutes(59).AddSeconds(59);

                    query += (parameters.Count == 0 ? " WHERE " : " AND ") + "dateCreated >= @startDate AND dateCreated <= @endDate";
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

        [HttpDelete]
        [Route("DeleteSlideDetail")]
        public JsonResult DeleteSlideDetail([FromBody] List<int> slideDetailIds)
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


                if (slideDetailIds == null || slideDetailIds.Count == 0)
                {
                    return new JsonResult("No slideshow IDs provided for deletion");
                }


                List<string> oldContentUrls = new List<string>();
                foreach (int id in slideDetailIds)
                {
                    string oldContentUrl = GetOldContentUrl(id);
                    if (!string.IsNullOrEmpty(oldContentUrl))
                    {
                        oldContentUrls.Add(oldContentUrl);
                    }
                }


                StringBuilder deleteQuery = new StringBuilder("DELETE FROM TSlideDetail WHERE id IN (");
                List<SqlParameter> parameters = new List<SqlParameter>();

                for (int i = 0; i < slideDetailIds.Count; i++)
                {
                    string parameterName = "@SlideDetailId" + i;
                    deleteQuery.Append(parameterName);

                    if (i < slideDetailIds.Count - 1)
                    {
                        deleteQuery.Append(", ");
                    }

                    parameters.Add(new SqlParameter(parameterName, slideDetailIds[i]));
                }

                deleteQuery.Append(");");

                _exQuery.ExecuteRawQuery(deleteQuery.ToString(), parameters.ToArray());


                foreach (string contentUrl in oldContentUrls)
                {
                    System.IO.File.Delete(localFolderPath + contentUrl);
                }

                return new JsonResult("Slide detail and associated image files deleted successfully");
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
        [Route("SearchSlideDetail")]
        public JsonResult SearchSlideDetail(string searchQuery)
        {
            ResponseDto response = new ResponseDto();
            try
            {
                string query = "SELECT  sd.id, sd.description, sd.typeContent, sd.sequence, sd.contentUrl, sd.slideHeaderId, sd.isActive, sd.dateModified, sd.dateCreated " +
                           "FROM TSlideDetail sd " +
                           "WHERE sd.id LIKE @searchQuery OR " +
                           "sd.description LIKE @searchQuery OR " +
                           "sd.typeContent LIKE @searchQuery OR " +
                           "sd.slideHeaderId LIKE @searchQuery";

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
