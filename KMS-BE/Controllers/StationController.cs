using Microsoft.AspNetCore.Mvc;
using KMS.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using System.Data;

namespace KMS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StationController : ControllerBase
    {
        private readonly KioskManagementSystemContext _dbcontext;
        private IConfiguration _configuration;

        public StationController(IConfiguration configuration, KioskManagementSystemContext _context)
        {
            _dbcontext = _context;
            _configuration = configuration;
        }

        [HttpGet]
        [Route("ShowStation")]
        public JsonResult GetStation()
        {

            string query = "select st.id, st.stationName, st.companyName, st.city, st.address, st.isActive" +
                "\r\nfrom TStation st";
            DataTable table = new DataTable();
            string sqlDatasource = _configuration.GetConnectionString("DefaultConnection");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDatasource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult(table);

        }

        [HttpGet]
        [Route("ShowStation/{id}")]
        public JsonResult GetStationById(int id)
        {
            string query = "SELECT id, stationName, companyName, city, address, isActive " +
                           "FROM TStation " +
                           "WHERE id = @Id";

            DataTable table = new DataTable();

            using (SqlConnection myCon = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                myCon.Open();

                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@Id", id);

                    SqlDataReader myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                }
            }

            if (table.Rows.Count > 0)
            {
                return new JsonResult(table);
            }
            else
            {
                return new JsonResult("Station not found");
            }
        }


        [HttpPost]
        [Route("AddStation")]
        public JsonResult AddStation([FromBody] Tstation station)
        {
            // Assuming StationModel is a class representing your station data
            // Validate the incoming data if necessary

            string query = "INSERT INTO TStation (stationName, companyName, city, address, isActive, dateCreated) " +
                           "VALUES (@StationName, @CompanyName, @City, @Address, @IsActive, GETDATE())";

            using (SqlConnection myCon = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                myCon.Open();

                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@StationName", station.StationName);
                    myCommand.Parameters.AddWithValue("@CompanyName", station.CompanyName);
                    myCommand.Parameters.AddWithValue("@City", station.City);
                    myCommand.Parameters.AddWithValue("@Address", station.Address);
                    myCommand.Parameters.AddWithValue("@IsActive", station.IsActive);

                    myCommand.ExecuteNonQuery();
                }
            }

            return new JsonResult("Station added successfully");
        }


        [HttpPut]
        [Route("UpdateStation/{id}")]
        public JsonResult UpdateStation(int id, [FromBody] Tstation station)
        {
            // Assuming StationModel is a class representing your station data
            // Validate the incoming data if necessary

            string query = "UPDATE TStation " +
                           "SET stationName = @StationName, companyName = @CompanyName, " +
                           "city = @City, address = @Address,dateModified = GETDATE(), isActive = @IsActive " +
                           "WHERE id = @Id";

            using (SqlConnection myCon = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                myCon.Open();

                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@Id", id);
                    myCommand.Parameters.AddWithValue("@StationName", station.StationName);
                    myCommand.Parameters.AddWithValue("@CompanyName", station.CompanyName);
                    myCommand.Parameters.AddWithValue("@City", station.City);
                    myCommand.Parameters.AddWithValue("@Address", station.Address);
                    myCommand.Parameters.AddWithValue("@IsActive", station.IsActive);

                    myCommand.ExecuteNonQuery();
                }
            }

            return new JsonResult("Station updated successfully");
        }




        [HttpDelete]
        [Route("DeleteStation/{id}")]
        public JsonResult DeleteStation(int id)
        {
            string query = "DELETE FROM TStation WHERE id = @Id";

            using (SqlConnection myCon = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                myCon.Open();

                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@Id", id);

                    myCommand.ExecuteNonQuery();
                }
            }

            return new JsonResult("Station deleted successfully");
        }



    }
}