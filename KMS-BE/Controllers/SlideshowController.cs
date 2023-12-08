using Microsoft.AspNetCore.Mvc;
using KMS.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace KMS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SlideshowController : ControllerBase
    {
        private readonly KioskManagementSystemContext _dbcontext;
        private IConfiguration _configuration;

        public SlideshowController(IConfiguration configuration, KioskManagementSystemContext _context)
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
        [Route("ShowSlideshow")]
        public JsonResult GetSlideshow()
        {
            string query = "select ss.id, ss.packageName, ss.imagevideo, ss.fileType, ss.startDate, ss.endDate" +
                "\r\nfrom TSlideshow ss";
            DataTable table = ExecuteRawQuery(query);
            return new JsonResult(table);
        }

        [HttpGet]
        [Route("ShowSlideshow/{id}")]
        public JsonResult GetSlideshowById(int id)
        {
            string query = "SELECT id, packageName, imagevideo, fileType, startDate, endDate " +
                           "FROM TSlideshow " +
                           "WHERE id = @Id";
            SqlParameter parameter = new SqlParameter("@Id", id);
            DataTable table = ExecuteRawQuery(query, new[] { parameter });

            if (table.Rows.Count > 0)
            {
                return new JsonResult(table);
            }
            else
            {
                return new JsonResult("Slideshow not found");
            }
        }

        [HttpGet]
        [Route("FilterSlideshow")]
        public JsonResult FilterSlideshow([FromQuery] string? packageName = null)
        {
            string query = "SELECT id, packageName, imagevideo, fileType, startDate, endDate " +
                           "FROM TSlideshow ";

            List<SqlParameter> parameters = new List<SqlParameter>();

            

            if (!string.IsNullOrEmpty(packageName))
            {
                query += (parameters.Count == 0 ? " WHERE " : " AND ") + "packageName LIKE @packageName";
                parameters.Add(new SqlParameter("@packageName", "%" + packageName + "%"));
            }

            DataTable table = ExecuteRawQuery(query, parameters.ToArray());

            return new JsonResult(table);
        }


        [HttpPost]
        [Route("AddSlideshow")]
        public JsonResult AddSlideshow([FromBody] Tslideshow slideshow)
        {
            string query = "INSERT INTO TSlideshow (packageName, imagevideo, fileType, startDate, endDate) " +
                           "VALUES (@PackageName, @ImageVideo, @FileType, @StartDate, @EndDate)";
            SqlParameter[] parameters =
            {
                new SqlParameter("@PackageName", slideshow.PackageName),
                new SqlParameter("@ImageVideo", slideshow.Imagevideo),
                new SqlParameter("@FileType", slideshow.FileType),
                new SqlParameter("@StartDate", slideshow.StartDate),
                new SqlParameter("@EndDate", slideshow.EndDate)
            };
            ExecuteRawQuery(query, parameters);

            return new JsonResult("Slideshow added successfully");
        }

        [HttpPut]
        [Route("UpdateSlideshow/{id}")]
        public JsonResult UpdateSlideshow(int id, [FromBody] Tslideshow slideshow)
        {
            string query = "UPDATE TSlideshow SET packageName = @PackageName, imagevideo = @ImageVideo, " +
                           "fileType = @FileType, startDate = @StartDate, endDate = @EndDate " +
                           "WHERE id = @Id";
            SqlParameter[] parameters =
            {
                new SqlParameter("@Id", id),
                new SqlParameter("@PackageName", slideshow.PackageName),
                new SqlParameter("@ImageVideo", slideshow.Imagevideo),
                new SqlParameter("@FileType", slideshow.FileType),
                new SqlParameter("@StartDate", slideshow.StartDate),
                new SqlParameter("@EndDate", slideshow.EndDate)
            };
            ExecuteRawQuery(query, parameters);

            return new JsonResult("Slideshow updated successfully");
        }

        [HttpDelete]
        [Route("DeleteSlideshow/{id}")]
        public JsonResult DeleteSlideshow(int id)
        {
            string query = "DELETE FROM TSlideshow WHERE id = @Id";
            SqlParameter parameter = new SqlParameter("@Id", id);
            ExecuteRawQuery(query, new[] { parameter });

            return new JsonResult("Slideshow deleted successfully");
        }

        [HttpGet]
        [Route("SearchSlideshow")]
        public JsonResult SearchSlideshow(string searchQuery)
        {
            string query = "SELECT id, packageName, imagevideo, fileType, startDate, endDate " +
                           "FROM TSlideshow " +
                           "WHERE id LIKE @searchQuery OR " +
                           "packageName LIKE @searchQuery OR " +
                           "imagevideo LIKE @searchQuery OR " +
                           "fileType LIKE @searchQuery";

            SqlParameter parameter = new SqlParameter("@searchQuery", "%" + searchQuery + "%");
            DataTable table = ExecuteRawQuery(query, new[] { parameter });

            return new JsonResult(table);
        }

    }
}
