using Microsoft.AspNetCore.Mvc;
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
    public class SlideshowController : ControllerBase
    {
        private readonly KioskManagementSystemContext _dbcontext;
        private IConfiguration _configuration;
        private readonly ExecuteQuery _exQuery;

        public SlideshowController(IConfiguration configuration, KioskManagementSystemContext _context, ExecuteQuery exQuery)
        {
            _dbcontext = _context;
            _configuration = configuration;
            _exQuery = exQuery;
        }

        [HttpGet]
        [Route("ShowSlideshow")]
        public JsonResult GetSlideshow()
        {
            string query = "select ss.id, ss.packageName, ss.imagevideo, ss.fileType, ss.startDate, ss.endDate" +
                "\r\nfrom TSlideshow ss";
            DataTable table = _exQuery.ExecuteRawQuery(query);
            return new JsonResult(table);
        }

        [HttpGet]
        [Route("ShowSlideshow/{id}")]
        public JsonResult GetSlideshowById(int id)
        {
            string query = "SELECT * " +
                           "FROM TSlideshow " +
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

        [HttpGet]
        [Route("FilterSlideshow")]
        public JsonResult FilterSlideshow([FromQuery] string? packageName = null, [FromQuery] DateTime? startDate = null, [FromQuery] DateTime? endDate = null)
        {
            string query = "SELECT id, packageName, imagevideo, fileType, startDate, endDate " +
                           "FROM TSlideshow ";

            List<SqlParameter> parameters = new List<SqlParameter>();



            if (!string.IsNullOrEmpty(packageName))
            {
                query += (parameters.Count == 0 ? " WHERE " : " AND ") + "packageName LIKE @packageName";
                parameters.Add(new SqlParameter("@packageName", "%" + packageName + "%"));
            }

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
            _exQuery.ExecuteRawQuery(query, parameters);

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
            _exQuery.ExecuteRawQuery(query, parameters);

            return new JsonResult("Slideshow updated successfully");
        }

        [HttpDelete]
        [Route("DeleteSlideshow")]
        public JsonResult DeleteSlideshow([FromBody] List<int> slideshowIds)
        {
            if (slideshowIds == null || slideshowIds.Count == 0)
            {
                return new JsonResult("No slideshow IDs provided for deletion");
            }

            StringBuilder deleteQuery = new StringBuilder("DELETE FROM TSlideshow WHERE id IN (");


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
            DataTable table = _exQuery.ExecuteRawQuery(query, new[] { parameter });

            return new JsonResult(table);
        }



    }
}
