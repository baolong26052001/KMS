using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using KMS.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace KMS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StationController : ControllerBase
    {
        private readonly KioskManagementSystemContext _dbcontext;
        private readonly IConfiguration _configuration;

        public StationController(IConfiguration configuration, KioskManagementSystemContext context)
        {
            _dbcontext = context;
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
        [Route("ShowStation")]
        public JsonResult GetStation()
        {
            string query = "select st.id, st.stationName, st.companyName, st.city, st.address, st.isActive" +
                           "\r\nfrom TStation st";
            DataTable table = ExecuteRawQuery(query);
            return new JsonResult(table);
        }

        [HttpGet]
        [Route("ShowStation/{id}")]
        public JsonResult GetStationById(int id)
        {
            string query = "SELECT id, stationName, companyName, city, address, isActive " +
                           "FROM TStation " +
                           "WHERE id = @Id";
            SqlParameter parameter = new SqlParameter("@Id", id);
            DataTable table = ExecuteRawQuery(query, new[] { parameter });

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

            DataTable table = ExecuteRawQuery(query, parameters.ToArray());

            return new JsonResult(table);
        }


        [HttpPost]
        [Route("AddStation")]
        public JsonResult AddStation([FromBody] Tstation station)
        {
            string query = "INSERT INTO TStation (stationName, companyName, city, address, isActive, dateCreated) " +
                           "VALUES (@StationName, @CompanyName, @City, @Address, @IsActive, GETDATE())";
            SqlParameter[] parameters =
            {
                new SqlParameter("@StationName", station.StationName),
                new SqlParameter("@CompanyName", station.CompanyName),
                new SqlParameter("@City", station.City),
                new SqlParameter("@Address", station.Address),
                new SqlParameter("@IsActive", station.IsActive)
            };
            ExecuteRawQuery(query, parameters);

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
            SqlParameter[] parameters =
            {
                new SqlParameter("@Id", id),
                new SqlParameter("@StationName", station.StationName),
                new SqlParameter("@CompanyName", station.CompanyName),
                new SqlParameter("@City", station.City),
                new SqlParameter("@Address", station.Address),
                new SqlParameter("@IsActive", station.IsActive)
            };
            ExecuteRawQuery(query, parameters);

            return new JsonResult("Station updated successfully");
        }

        [HttpDelete]
        [Route("DeleteStation/{id}")]
        public JsonResult DeleteStation(int id)
        {
            string query = "DELETE FROM TStation WHERE id = @Id";
            SqlParameter parameter = new SqlParameter("@Id", id);
            ExecuteRawQuery(query, new[] { parameter });

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
                           "address LIKE @searchQuery OR " +
                           "CAST(isActive AS VARCHAR) LIKE @searchQuery";

            SqlParameter parameter = new SqlParameter("@searchQuery", "%" + searchQuery + "%");
            DataTable table = ExecuteRawQuery(query, new[] { parameter });

            return new JsonResult(table);
        }

    }
}
