using Microsoft.AspNetCore.Mvc;
using KMS.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace KMS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class KioskController : ControllerBase
    {
        private readonly KioskManagementSystemContext _dbcontext;
        private IConfiguration _configuration;

        public KioskController(IConfiguration configuration, KioskManagementSystemContext _context)
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
        [Route("ShowKiosk")]
        public JsonResult GetKiosk()
        {
            string query = "select k.id,  k.kioskName, k.location, st.stationName, ss.packageName, k.kioskStatus, k.cameraStatus, k.cashDepositStatus, k.scannerStatus, k.printerStatus\r\nfrom TKiosk k, TStation st, TSlideshow ss\r\nwhere k.stationCode = st.id and ss.id = k.slidePackage";
            DataTable table = ExecuteRawQuery(query);
            return new JsonResult(table);
        }



        [HttpGet]
        [Route("ShowKioskSetup/{id}")]
        public JsonResult GetKioskById(int id)
        {
            string query = "SELECT k.id, k.kioskName, k.location, st.stationName, ss.packageName, k.kioskStatus, k.cameraStatus, k.cashDepositStatus, k.scannerStatus, k.printerStatus " +
                           "FROM TKiosk k " +
                           "JOIN TStation st ON k.stationCode = st.id " +
                           "JOIN TSlideshow ss ON ss.id = k.slidePackage " +
                           "WHERE k.id = @id";

            SqlParameter parameter = new SqlParameter("@id", id);
            DataTable table = ExecuteRawQuery(query, new[] { parameter });

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
        [Route("ShowKioskSetup")] // show by package
        public JsonResult GetKioskByPackage(string packageName)
        {
            string query = "SELECT k.id, k.kioskName, k.location, st.stationName, ss.packageName, k.kioskStatus, k.cameraStatus, k.cashDepositStatus, k.scannerStatus, k.printerStatus " +
                           "FROM TKiosk k " +
                           "JOIN TStation st ON k.stationCode = st.id " +
                           "JOIN TSlideshow ss ON ss.id = k.slidePackage " +
                           "WHERE ss.packageName = @packageName";

            SqlParameter parameter = new SqlParameter("@packageName", packageName);
            DataTable table = ExecuteRawQuery(query, new[] { parameter });

            return new JsonResult(table);
        }

        [HttpGet]
        [Route("ShowKioskHardware")]
        public JsonResult GetKioskHardware()
        {
            string query = "select k.id, k.availableMemory, k.ipAddress, k.OSName, k.OSPlatform, k.OSVersion" +
                "\r\nfrom TKiosk k";
            DataTable table = ExecuteRawQuery(query);
            return new JsonResult(table);
        }

        [HttpPost]
        [Route("AddKiosk")]
        public JsonResult AddKiosk([FromBody] Tkiosk kiosk)
        {
            string query = "INSERT INTO TKiosk (kioskName, location, stationCode, slidePackage, kioskStatus, cameraStatus, cashDepositStatus, scannerStatus, printerStatus) " +
                           "VALUES (@kioskName, @location, @stationCode, @slidePackage, @kioskStatus, @cameraStatus, @cashDepositStatus, @scannerStatus, @printerStatus)";
            SqlParameter[] parameters =
            {
                new SqlParameter("@kioskName", kiosk.KioskName),
                new SqlParameter("@location", kiosk.Location),
                new SqlParameter("@stationCode", kiosk.StationCode),
                new SqlParameter("@slidePackage", kiosk.SlidePackage),
                new SqlParameter("@kioskStatus", kiosk.KioskStatus),
                new SqlParameter("@cameraStatus", kiosk.CameraStatus),
                new SqlParameter("@cashDepositStatus", kiosk.CashDepositStatus),
                new SqlParameter("@scannerStatus", kiosk.ScannerStatus),
                new SqlParameter("@printerStatus", kiosk.PrinterStatus)
            };
            ExecuteRawQuery(query, parameters);

            return new JsonResult("Kiosk added successfully");
        }

        [HttpPut]
        [Route("UpdateKiosk/{id}")]
        public JsonResult UpdateKiosk(int id, [FromBody] Tkiosk kiosk)
        {
            string query = "UPDATE TKiosk SET kioskName = @kioskName, location = @location, stationCode = @stationCode, " +
                           "slidePackage = @slidePackage, kioskStatus = @kioskStatus, cameraStatus = @cameraStatus, " +
                           "cashDepositStatus = @cashDepositStatus, scannerStatus = @scannerStatus, printerStatus = @printerStatus " +
                           "WHERE id = @id";
            SqlParameter[] parameters =
            {
                new SqlParameter("@id", id),
                new SqlParameter("@kioskName", kiosk.KioskName),
                new SqlParameter("@location", kiosk.Location),
                new SqlParameter("@stationCode", kiosk.StationCode),
                new SqlParameter("@slidePackage", kiosk.SlidePackage),
                new SqlParameter("@kioskStatus", kiosk.KioskStatus),
                new SqlParameter("@cameraStatus", kiosk.CameraStatus),
                new SqlParameter("@cashDepositStatus", kiosk.CashDepositStatus),
                new SqlParameter("@scannerStatus", kiosk.ScannerStatus),
                new SqlParameter("@printerStatus", kiosk.PrinterStatus)
            };
            ExecuteRawQuery(query, parameters);

            return new JsonResult("Kiosk updated successfully");
        }

        [HttpDelete]
        [Route("DeleteKiosk/{id}")]
        public JsonResult DeleteKiosk(int id)
        {
            string query = "DELETE FROM TKiosk WHERE id = @id";
            SqlParameter parameter = new SqlParameter("@id", id);
            ExecuteRawQuery(query, new[] { parameter });

            return new JsonResult("Kiosk deleted successfully");
        }

        [HttpGet]
        [Route("SearchKiosk")]
        public JsonResult SearchKiosk(string searchQuery)
        {
            string query = "SELECT k.id, k.kioskName, k.location, st.stationName, ss.packageName, k.kioskStatus, k.cameraStatus, k.cashDepositStatus, k.scannerStatus, k.printerStatus " +
                           "FROM TKiosk k " +
                           "JOIN TStation st ON k.stationCode = st.id " +
                           "JOIN TSlideshow ss ON ss.id = k.slidePackage " +
                           "WHERE k.kioskName LIKE @searchQuery OR " +
                           "k.location LIKE @searchQuery OR " +
                           "st.stationName LIKE @searchQuery OR " +
                           "ss.packageName LIKE @searchQuery OR " +
                           "k.kioskStatus LIKE @searchQuery OR " +
                           "k.cameraStatus LIKE @searchQuery OR " +
                           "k.cashDepositStatus LIKE @searchQuery OR " +
                           "k.scannerStatus LIKE @searchQuery OR " +
                           "k.printerStatus LIKE @searchQuery";

            SqlParameter parameter = new SqlParameter("@searchQuery", "%" + searchQuery + "%");
            DataTable table = ExecuteRawQuery(query, new[] { parameter });

            return new JsonResult(table);
        }

    }
}
