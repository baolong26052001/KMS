using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using KMS.Models;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Text;
using KMS.Tools;

namespace KMS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StationController : ControllerBase
    {
        private readonly KioskManagementSystemContext _dbcontext;
        private IConfiguration _configuration;
        private readonly ExecuteQuery _exQuery;

        public StationController(IConfiguration configuration, KioskManagementSystemContext _context, ExecuteQuery exQuery)
        {
            _dbcontext = _context;
            _configuration = configuration;
            _exQuery = exQuery;
        }

        

        [HttpGet]
        [Route("ShowStation")]
        public JsonResult GetStation()
        {
            string query = "select st.id, st.stationName, st.companyName, st.city, st.address, st.isActive" +
                           "\r\nfrom TStation st";
            DataTable table = _exQuery.ExecuteRawQuery(query);
            return new JsonResult(table);
        }

        [HttpGet]
        [Route("ShowStation/{id}")]
        public JsonResult GetStationById(int id)
        {
            string query = "SELECT * " +
                           "FROM TStation " +
                           "WHERE id = @Id";
            SqlParameter parameter = new SqlParameter("@Id", id);
            DataTable table = _exQuery.ExecuteRawQuery(query, new[] { parameter });

            if (table.Rows.Count > 0)
            {
                return new JsonResult(table);
            }
            else
            {
                return new JsonResult("Station not found");
            }
        }

        [HttpGet]
        [Route("FilterStation")]
        public JsonResult FilterStation([FromQuery] string? stationName = null, [FromQuery] string? companyName = null, [FromQuery] string? city = null)
        {
            string query = "SELECT id, stationName, companyName, city, address, isActive " +
                           "FROM TStation ";

            List<SqlParameter> parameters = new List<SqlParameter>();

            

            if (!string.IsNullOrEmpty(stationName))
            {
                query += (parameters.Count == 0 ? " WHERE " : " AND ") + "stationName LIKE @stationName";
                parameters.Add(new SqlParameter("@stationName", "%" + stationName + "%"));
            }

            if (!string.IsNullOrEmpty(companyName))
            {
                query += (parameters.Count == 0 ? " WHERE " : " AND ") + "companyName LIKE @companyName";
                parameters.Add(new SqlParameter("@companyName", "%" + companyName + "%"));
            }

            if (!string.IsNullOrEmpty(city))
            {
                query += (parameters.Count == 0 ? " WHERE " : " AND ") + "city LIKE @city";
                parameters.Add(new SqlParameter("@city", "%" + city + "%"));
            }

            DataTable table = _exQuery.ExecuteRawQuery(query, parameters.ToArray());

            return new JsonResult(table);
        }


        [HttpPost]
        [Route("AddStation")]
        public JsonResult AddStation([FromBody] Tstation station)
        {
            string query = "INSERT INTO TStation (stationName, companyName, city, address, isActive, dateCreated) " +
                           "VALUES (@StationName, @CompanyName, @City, @Address, @IsActive, GETDATE())";
            string query2 = "INSERT INTO TAudit (action, tableName, dateModified, dateCreated, isActive) VALUES ('Add', 'TStation', GETDATE(), GETDATE(), 1)";
            
            SqlParameter[] parameters =
            {
                new SqlParameter("@StationName", station.StationName),
                new SqlParameter("@CompanyName", station.CompanyName),
                new SqlParameter("@City", station.City),
                new SqlParameter("@Address", station.Address),
                new SqlParameter("@IsActive", station.IsActive)
            };
            SqlParameter[] parameters2 = { };
            _exQuery.ExecuteRawQuery(query, parameters);
            _exQuery.ExecuteRawQuery(query2, parameters2);

            return new JsonResult("Station added successfully");
        }

        [HttpPut]
        [Route("UpdateStation/{id}")]
        public JsonResult UpdateStation(int id, [FromBody] Tstation station)
        {
            string query = "UPDATE TStation " +
                           "SET stationName = @StationName, companyName = @CompanyName, " +
                           "city = @City, address = @Address, dateModified = GETDATE(), isActive = @IsActive " +
                           "WHERE id = @Id";
            string query2 = "INSERT INTO TAudit (action, tableName, dateModified, dateCreated, isActive) VALUES ('Update', 'TStation', GETDATE(), GETDATE(), 1)";
            
            SqlParameter[] parameters =
            {
                new SqlParameter("@Id", id),
                new SqlParameter("@StationName", station.StationName),
                new SqlParameter("@CompanyName", station.CompanyName),
                new SqlParameter("@City", station.City),
                new SqlParameter("@Address", station.Address),
                new SqlParameter("@IsActive", station.IsActive)
            };
            SqlParameter[] parameters2 = { };
            _exQuery.ExecuteRawQuery(query, parameters);
            _exQuery.ExecuteRawQuery(query2, parameters2);
            return new JsonResult("Station updated successfully");
        }

        [HttpDelete]
        [Route("DeleteStation")]
        public JsonResult DeleteStation([FromBody] List<int> stationIds)
        {
            if (stationIds == null || stationIds.Count == 0)
            {
                return new JsonResult("No station IDs provided for deletion");
            }

            StringBuilder deleteQuery = new StringBuilder("DELETE FROM TStation WHERE id IN (");
            string query2 = "INSERT INTO TAudit (action, tableName, dateModified, dateCreated, isActive) VALUES ('Delete', 'TStation', GETDATE(), GETDATE(), 1)";

            List<SqlParameter> parameters = new List<SqlParameter>();
            SqlParameter[] parameters2 = { };
            for (int i = 0; i < stationIds.Count; i++)
            {
                string parameterName = "@StationId" + i;
                deleteQuery.Append(parameterName);

                if (i < stationIds.Count - 1)
                {
                    deleteQuery.Append(", ");
                }

                parameters.Add(new SqlParameter(parameterName, stationIds[i]));
            }

            deleteQuery.Append(");");

            _exQuery.ExecuteRawQuery(deleteQuery.ToString(), parameters.ToArray());
            _exQuery.ExecuteRawQuery(query2, parameters2);

            return new JsonResult("Station deleted successfully");
        }

        [HttpGet]
        [Route("SearchStation")]
        public JsonResult SearchStation(string searchQuery)
        {
            string query = "SELECT id, stationName, companyName, city, address, isActive " +
                           "FROM TStation " +
                           "WHERE id LIKE @searchQuery OR " +
                           "stationName LIKE @searchQuery OR " +
                           "companyName LIKE @searchQuery OR " +
                           "city LIKE @searchQuery OR " +
                           "address LIKE @searchQuery";

            SqlParameter parameter = new SqlParameter("@searchQuery", "%" + searchQuery + "%");
            DataTable table = _exQuery.ExecuteRawQuery(query, new[] { parameter });

            return new JsonResult(table);
        }

    }
}
