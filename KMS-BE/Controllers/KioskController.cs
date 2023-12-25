﻿using Microsoft.AspNetCore.Mvc;
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
            string query = "SELECT k.id, k.kioskName, k.location, st.stationName, ss.packageName, k.kioskStatus, k.cameraStatus, k.cashDepositStatus, k.scannerStatus, k.printerStatus " +
                           "FROM TKiosk k " +
                           "LEFT JOIN TStation st ON k.stationCode = st.id " +
                           "LEFT JOIN TSlideshow ss ON ss.id = k.slidePackage";

            DataTable table = _exQuery.ExecuteRawQuery(query);
            return new JsonResult(table);
        }




        [HttpGet]
        [Route("ShowKioskSetup/{id}")]
        public JsonResult GetKioskById(int id)
        {
            string query = "SELECT k.id, k.kioskName, k.location, st.stationName, ss.packageName, k.kioskStatus, k.cameraStatus, k.cashDepositStatus, k.scannerStatus, k.printerStatus " +
                           "FROM TKiosk k " +
                           "LEFT JOIN TStation st ON k.stationCode = st.id " +
                           "LEFT JOIN TSlideshow ss ON ss.id = k.slidePackage " +
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

        [HttpGet]
        [Route("FilterKioskSetup")] // filter by package name, station name, country
        public JsonResult FilterKioskSetup([FromQuery] string? packageName = null, [FromQuery] string? location = null, [FromQuery] string? stationName = null)
        {
            string query = "SELECT k.id, k.kioskName, k.location, st.stationName, ss.packageName, k.kioskStatus, k.cameraStatus, k.cashDepositStatus, k.scannerStatus, k.printerStatus " +
                           "FROM TKiosk k " +
                           "LEFT JOIN TStation st ON k.stationCode = st.id " +
                           "LEFT JOIN TSlideshow ss ON ss.id = k.slidePackage ";

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
                query += (parameters.Count == 0 ? " WHERE " : " AND ") + "ss.packageName = @packageName";
                parameters.Add(new SqlParameter("@packageName", packageName));
            }

            DataTable table = _exQuery.ExecuteRawQuery(query, parameters.ToArray());

            return new JsonResult(table);
        }




        [HttpGet]
        [Route("ShowKioskHardware")]
        public JsonResult GetKioskHardware()
        {
            string query = "select k.id, k.availableMemory, k.ipAddress, k.OSName, k.OSPlatform, k.OSVersion" +
                "\r\nfrom TKiosk k";
            DataTable table = _exQuery.ExecuteRawQuery(query);
            return new JsonResult(table);
        }

        [HttpGet]
        [Route("ShowKioskHardware/{id}")]
        public JsonResult GetKioskHardwareById(int id)
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

        [HttpPost]
        [Route("AddKiosk")]
        public JsonResult AddKiosk([FromBody] Tkiosk kiosk)
        {
            string query = "INSERT INTO TKiosk (kioskName, location, stationCode, slidePackage, webServices) " +
                           "VALUES (@kioskName, @location, @stationCode, @slidePackage, @webServices)";
            SqlParameter[] parameters =
            {
                new SqlParameter("@kioskName", kiosk.KioskName),
                new SqlParameter("@location", kiosk.Location),
                new SqlParameter("@stationCode", kiosk.StationCode),
                new SqlParameter("@slidePackage", kiosk.SlidePackage),
                new SqlParameter("@webServices", kiosk.WebServices),
            };
            _exQuery.ExecuteRawQuery(query, parameters);

            return new JsonResult("Kiosk added successfully");
        }

        [HttpPut]
        [Route("UpdateKiosk/{id}")]
        public JsonResult UpdateKiosk(int id, [FromBody] Tkiosk kiosk)
        {
            string query = "UPDATE TKiosk SET kioskName = @kioskName, location = @location, stationCode = @stationCode, " +
                           "slidePackage = @slidePackage, webServices = @webServices " +
                           "WHERE id = @id";
            SqlParameter[] parameters =
            {
                new SqlParameter("@id", id),
                new SqlParameter("@kioskName", kiosk.KioskName),
                new SqlParameter("@location", kiosk.Location),
                new SqlParameter("@stationCode", kiosk.StationCode),
                new SqlParameter("@slidePackage", kiosk.SlidePackage),
                new SqlParameter("@webServices", kiosk.WebServices),
            };
            _exQuery.ExecuteRawQuery(query, parameters);

            return new JsonResult("Kiosk updated successfully");
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


            List<SqlParameter> parameters = new List<SqlParameter>();
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

            return new JsonResult("Kiosk deleted successfully");
        }

        [HttpGet]
        [Route("SearchKioskSetup")]
        public JsonResult SearchKioskSetup(string searchQuery)
        {
            string query = "SELECT k.id, k.kioskName, k.location, st.stationName, ss.packageName, k.kioskStatus, k.cameraStatus, k.cashDepositStatus, k.scannerStatus, k.printerStatus " +
                           "FROM TKiosk k " +
                           "LEFT JOIN TStation st ON k.stationCode = st.id " +
                           "LEFT JOIN TSlideshow ss ON ss.id = k.slidePackage " +
                           "WHERE k.id LIKE @searchQuery OR " +
                           "k.kioskName LIKE @searchQuery OR " +
                           "k.location LIKE @searchQuery OR " +
                           "st.stationName LIKE @searchQuery OR " +
                           "ss.packageName LIKE @searchQuery";

            SqlParameter parameter = new SqlParameter("@searchQuery", "%" + searchQuery + "%");
            DataTable table = _exQuery.ExecuteRawQuery(query, new[] { parameter });

            return new JsonResult(table);
        }

        [HttpGet]
        [Route("SearchKioskHardware")]
        public JsonResult SearchKioskHardware(string searchQuery)
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

        [HttpGet]
        [Route("ShowKioskHealth")]
        public JsonResult GetKioskHealth()
        {
            string query = "select k.dateCreated,  k.stationCode,  k.id,  DATEDIFF(MINUTE, k.onlineTime, GETDATE()) AS durationUptime,  DATEDIFF(MINUTE, k.offlineTime, GETDATE()) AS durationDowntime, k.dateModified as lastUpdated " +
                "from TKiosk k";
            DataTable table = _exQuery.ExecuteRawQuery(query);
            return new JsonResult(table);
        }

        [HttpGet]
        [Route("ShowKioskHealth/{id}")]
        public JsonResult GetKioskHealthById(int id)
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

        [HttpGet]
        [Route("ShowKioskHealthDetail")]
        public JsonResult GetKioskHealthDetail()
        {
            string query = "select k.dateCreated,  k.stationCode,  k.id,  k.component, k.onlineTime, k.offlineTime,   \r\nDATEDIFF(MINUTE, k.onlineTime, GETDATE()) AS durationUptime, \r\nDATEDIFF(MINUTE, k.offlineTime, GETDATE()) AS durationDowntime, k.dateModified as lastUpdated  \r\nfrom TKiosk k";
            DataTable table = _exQuery.ExecuteRawQuery(query);
            return new JsonResult(table);
        }

    }
}
