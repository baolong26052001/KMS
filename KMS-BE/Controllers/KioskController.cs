﻿using Microsoft.AspNetCore.Mvc;
using KMS.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Text;
using KMS.Tools;
using System.Net.Sockets;
using System.Net;

namespace KMS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class KioskController : ControllerBase
    {
        private readonly KioskManagementSystemContext _dbcontext;
        private IConfiguration _configuration;
        private readonly ExecuteQuery _exQuery;

        public KioskController(IConfiguration configuration, KioskManagementSystemContext _context, ExecuteQuery exQuery)
        {
            _dbcontext = _context;
            _configuration = configuration;
            _exQuery = exQuery;
        }

        [HttpGet]
        [Route("ShowKiosk")] // show all kiosk setup
        public JsonResult GetKiosk()
        {
            ResponseDto response = new ResponseDto();
            try
            {
                string query = "SELECT k.id, k.kioskName, k.location, st.stationName, ss.description as packageName, k.kioskStatus, k.cameraStatus, k.cashDepositStatus, k.scannerStatus, k.printerStatus " +
                           "FROM TKiosk k " +
                           "LEFT JOIN TStation st ON k.stationCode = st.id " +
                           "LEFT JOIN TSlideHeader ss ON ss.id = k.slidePackage";


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
        [Route("ShowKioskSetup/{id}")]
        public JsonResult GetKioskById(int id)
        {
            ResponseDto response = new ResponseDto();
            try
            {
                string query = "SELECT k.id, k.kioskName, k.location, st.id as stationCode, st.stationName, ss.id as slidePackage, ss.description as packageName, k.webServices, k.kioskStatus, k.cameraStatus, k.cashDepositStatus, k.scannerStatus, k.printerStatus " +
                           "FROM TKiosk k " +
                           "LEFT JOIN TStation st ON k.stationCode = st.id " +
                           "LEFT JOIN TSlideHeader ss ON ss.id = k.slidePackage " +
                           "WHERE k.id = @id";

                SqlParameter parameter = new SqlParameter("@id", id);
                DataTable table = _exQuery.ExecuteRawQuery(query, new[] { parameter });

                if (table.Rows.Count > 0)
                {
                    return new JsonResult(table);
                }
                else
                {
                    return new JsonResult("Kiosk not found");
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
        [Route("FilterKioskSetup")] // filter by package name, station name, country
        public JsonResult FilterKioskSetup([FromQuery] string? packageName = null, [FromQuery] string? location = null, [FromQuery] string? stationName = null)
        {
            ResponseDto response = new ResponseDto();
            try
            {
                string query = "SELECT k.id, k.kioskName, k.location, st.stationName, ss.description as packageName, k.kioskStatus, k.cameraStatus, k.cashDepositStatus, k.scannerStatus, k.printerStatus " +
                           "FROM TKiosk k " +
                           "LEFT JOIN TStation st ON k.stationCode = st.id " +
                           "LEFT JOIN TSlideHeader ss ON ss.id = k.slidePackage ";

                List<SqlParameter> parameters = new List<SqlParameter>();

                if (!string.IsNullOrEmpty(stationName))
                {
                    query += (parameters.Count == 0 ? " WHERE " : " AND ") + "st.stationName = @stationName";
                    parameters.Add(new SqlParameter("@stationName", stationName));
                }

                if (!string.IsNullOrEmpty(location))
                {
                    query += (parameters.Count == 0 ? " WHERE " : " AND ") + "k.location = @location";
                    parameters.Add(new SqlParameter("@location", location));
                }

                if (!string.IsNullOrEmpty(packageName))
                {
                    query += (parameters.Count == 0 ? " WHERE " : " AND ") + "ss.description = @packageName";
                    parameters.Add(new SqlParameter("@packageName", packageName));
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




        [HttpGet]
        [Route("ShowKioskHardware")]
        public JsonResult GetKioskHardware()
        {
            ResponseDto response = new ResponseDto();
            try
            {
                string query = "select k.id, k.availableMemory, k.ipAddress, k.OSName, k.OSPlatform, k.OSVersion" +
                "\r\nfrom TKiosk k";
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
        [Route("ShowKioskHardware/{id}")]
        public JsonResult GetKioskHardwareById(int id)
        {
            ResponseDto response = new ResponseDto();
            try
            {
                string query = "select k.id, k.availableMemory, k.ipAddress, k.OSName, k.OSPlatform, k.OSVersion, k.processor, k.totalMemory, k.diskSizeC, k.freeSpaceC, k.diskSizeD, k.freeSpaceD " +
                           "FROM TKiosk k " +
                           "WHERE k.id = @id";

                SqlParameter parameter = new SqlParameter("@id", id);
                DataTable table = _exQuery.ExecuteRawQuery(query, new[] { parameter });

                if (table.Rows.Count > 0)
                {
                    return new JsonResult(table);
                }
                else
                {
                    return new JsonResult("Kiosk hardware not found");
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
        [Route("AddKiosk")]
        public JsonResult AddKiosk([FromBody] TkioskModel kiosk)
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

                string query = "INSERT INTO TKiosk (kioskName, location, stationCode, slidePackage, webServices) " +
                           "VALUES (@kioskName, @location, @stationCode, @slidePackage, @webServices)";
                string query2 = "INSERT INTO TAudit (userId, ipAddress, macAddress, action, tableName, dateModified, dateCreated, isActive) " +
                           "VALUES (@UserId, @IpAddress, @Ipv6, 'Add', 'TKiosk', GETDATE(), GETDATE(), 1)";

                SqlParameter[] parameters =
                {
                    new SqlParameter("@kioskName", kiosk.KioskName),
                    new SqlParameter("@location", kiosk.Location),
                    new SqlParameter("@stationCode", kiosk.StationCode),
                    new SqlParameter("@slidePackage", kiosk.SlidePackage),
                    new SqlParameter("@webServices", kiosk.WebServices),
                };


                SqlParameter[] parameters2 =
                {
                    new SqlParameter("@IpAddress", ipv4?.ToString()),
                    new SqlParameter("@Ipv6", ipv6?.ToString()),
                    new SqlParameter("@UserId", (object)kiosk.UserId ?? DBNull.Value),
                };

                _exQuery.ExecuteRawQuery(query, parameters);
                _exQuery.ExecuteRawQuery(query2, parameters2);

                return new JsonResult("Kiosk added successfully");
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
        [Route("UpdateKiosk/{id}")]
        public JsonResult UpdateKiosk(int id, [FromBody] TkioskModel kiosk)
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

                string query = "UPDATE TKiosk SET kioskName = @kioskName, location = @location, stationCode = @stationCode, " +
                           "slidePackage = @slidePackage, webServices = @webServices " +
                           "WHERE id = @id";
                string query2 = "INSERT INTO TAudit (userId, ipAddress, macAddress, action, tableName, dateModified, dateCreated, isActive) " +
                           "VALUES (@UserId, @IpAddress, @Ipv6, 'Update', 'TKiosk', GETDATE(), GETDATE(), 1)";

                SqlParameter[] parameters =
                {
                    new SqlParameter("@id", id),
                    new SqlParameter("@kioskName", kiosk.KioskName),
                    new SqlParameter("@location", kiosk.Location),
                    new SqlParameter("@stationCode", kiosk.StationCode),
                    new SqlParameter("@slidePackage", kiosk.SlidePackage),
                    new SqlParameter("@webServices", kiosk.WebServices),
                };
                SqlParameter[] parameters2 =
                {
                    new SqlParameter("@IpAddress", ipv4?.ToString()),
                    new SqlParameter("@Ipv6", ipv6?.ToString()),
                    new SqlParameter("@UserId", (object)kiosk.UserId ?? DBNull.Value),
                };

                _exQuery.ExecuteRawQuery(query, parameters);
                _exQuery.ExecuteRawQuery(query2, parameters2);

                return new JsonResult("Kiosk updated successfully");
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
        [Route("DeleteKiosk")]
        public JsonResult DeleteKiosk([FromBody] List<int> kioskIds)
        {
            if (kioskIds == null || kioskIds.Count == 0)
            {
                return new JsonResult("No kiosk IDs provided for deletion");
            }

            StringBuilder deleteQuery = new StringBuilder("DELETE FROM TKiosk WHERE id IN (");
            string query2 = "INSERT INTO TAudit (action, tableName, dateModified, dateCreated, isActive) VALUES ('Delete', 'TKiosk', GETDATE(), GETDATE(), 1)";

            List<SqlParameter> parameters = new List<SqlParameter>();
            SqlParameter[] parameters2 = { };
            for (int i = 0; i < kioskIds.Count; i++)
            {
                string parameterName = "@KioskId" + i;
                deleteQuery.Append(parameterName);

                if (i < kioskIds.Count - 1)
                {
                    deleteQuery.Append(", ");
                }

                parameters.Add(new SqlParameter(parameterName, kioskIds[i]));
            }

            deleteQuery.Append(");");

            _exQuery.ExecuteRawQuery(deleteQuery.ToString(), parameters.ToArray());
            _exQuery.ExecuteRawQuery(query2, parameters2);

            return new JsonResult("Kiosk deleted successfully");
        }

        [HttpGet]
        [Route("SearchKioskSetup")]
        public JsonResult SearchKioskSetup(string searchQuery)
        {
            ResponseDto response = new ResponseDto();
            try
            {
                string query = "SELECT k.id, k.kioskName, k.location, st.stationName, ss.description as packageName, k.kioskStatus, k.cameraStatus, k.cashDepositStatus, k.scannerStatus, k.printerStatus " +
                           "FROM TKiosk k " +
                           "LEFT JOIN TStation st ON k.stationCode = st.id " +
                           "LEFT JOIN TSlideHeader ss ON ss.id = k.slidePackage " +
                           "WHERE k.id LIKE @searchQuery OR " +
                           "k.kioskName LIKE @searchQuery OR " +
                           "k.location LIKE @searchQuery OR " +
                           "st.stationName LIKE @searchQuery OR " +
                           "ss.description LIKE @searchQuery";

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

        [HttpGet]
        [Route("SearchKioskHardware")]
        public JsonResult SearchKioskHardware(string searchQuery)
        {
            ResponseDto response = new ResponseDto();
            try
            {
                string query = "select k.id, k.availableMemory, k.ipAddress, k.OSName, k.OSPlatform, k.OSVersion " +
                           "FROM TKiosk k " +
                           "WHERE k.id LIKE @searchQuery OR " +
                           "k.availableMemory LIKE @searchQuery OR " +
                           "k.ipAddress LIKE @searchQuery OR " +
                           "k.OSName LIKE @searchQuery OR " +
                           "k.OSPlatform LIKE @searchQuery OR " +
                           "k.OSVersion LIKE @searchQuery";

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

        [HttpGet]
        [Route("ShowKioskHealth")]
        public JsonResult GetKioskHealth()
        {
            ResponseDto response = new ResponseDto();
            try
            {
                string query = "select k.dateCreated,  k.stationCode,  k.id,  DATEDIFF(MINUTE, k.onlineTime, GETDATE()) AS durationUptime,  DATEDIFF(MINUTE, k.offlineTime, GETDATE()) AS durationDowntime, k.dateModified as lastUpdated " +
                "from TKiosk k";
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
        [Route("FilterKioskHealth")]
        public JsonResult FilterKioskHealth([FromQuery] int? stationCode = null, [FromQuery] DateTime? startDate = null, [FromQuery] DateTime? endDate = null)
        {
            ResponseDto response = new ResponseDto();
            try
            {
                string query = "select k.dateCreated,  k.stationCode,  k.id,  DATEDIFF(MINUTE, k.onlineTime, GETDATE()) AS durationUptime,  DATEDIFF(MINUTE, k.offlineTime, GETDATE()) AS durationDowntime, k.dateModified as lastUpdated " +
                "from TKiosk k";

                List<SqlParameter> parameters = new List<SqlParameter>();



                if (stationCode != null)
                {
                    query += (parameters.Count == 0 ? " WHERE " : " AND ") + "k.stationCode LIKE @stationCode";
                    parameters.Add(new SqlParameter("@stationCode", "%" + stationCode + "%"));
                }

                if (startDate.HasValue && endDate.HasValue)
                {

                    startDate = startDate.Value.Date.AddHours(0).AddMinutes(0).AddSeconds(0);
                    endDate = endDate.Value.Date.AddHours(23).AddMinutes(59).AddSeconds(59);

                    query += (parameters.Count == 0 ? " WHERE " : " AND ") + "k.dateCreated >= @startDate AND k.dateCreated <= @endDate";
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

        [HttpGet]
        [Route("SearchKioskHealth")]
        public JsonResult SearchKioskHealth(string searchQuery)
        {
            ResponseDto response = new ResponseDto();
            try
            {
                string query = "select k.dateCreated,  k.stationCode,  k.id,  DATEDIFF(MINUTE, k.onlineTime, GETDATE()) AS durationUptime,  DATEDIFF(MINUTE, k.offlineTime, GETDATE()) AS durationDowntime, k.dateModified as lastUpdated " +
                           "from TKiosk k " +
                           "WHERE k.id LIKE @searchQuery OR " +
                           "k.stationCode LIKE @searchQuery";

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

        [HttpGet]
        [Route("ShowKioskHealth/{id}")]
        public JsonResult GetKioskHealthById(int id)
        {
            ResponseDto response = new ResponseDto();
            try
            {
                string query = "select k.dateCreated,  k.stationCode,  k.id,  DATEDIFF(MINUTE, k.onlineTime, GETDATE()) AS durationUptime,  DATEDIFF(MINUTE, k.offlineTime, GETDATE()) AS durationDowntime, k.dateModified as lastUpdated " +
                           "FROM TKiosk k " +
                           "WHERE k.id = @id";

                SqlParameter parameter = new SqlParameter("@id", id);
                DataTable table = _exQuery.ExecuteRawQuery(query, new[] { parameter });

                if (table.Rows.Count > 0)
                {
                    return new JsonResult(table);
                }
                else
                {
                    return new JsonResult("Kiosk hardware not found");
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
        [Route("ShowKioskHealthDetail")]
        public JsonResult GetKioskHealthDetail()
        {
            ResponseDto response = new ResponseDto();
            try
            {
                string query = "select k.dateCreated,  k.stationCode,  k.id,  k.component, k.onlineTime, k.offlineTime,   \r\nDATEDIFF(MINUTE, k.onlineTime, GETDATE()) AS durationUptime, \r\nDATEDIFF(MINUTE, k.offlineTime, GETDATE()) AS durationDowntime, k.dateModified as lastUpdated  \r\nfrom TKiosk k";
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

    }
}
