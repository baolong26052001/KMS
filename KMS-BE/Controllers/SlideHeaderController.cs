using KMS.Models;
using KMS.Tools;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Text;
using Microsoft.Extensions.Configuration;


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
            string query = "select sh.id, sh.description, sh.startDate, sh.endDate, sh.IsActive, sh.timeNext, sh.dateModified, sh.dateCreated" +
                "\r\nfrom TSlideHeader sh";
            DataTable table = _exQuery.ExecuteRawQuery(query);
            return new JsonResult(table);
        }

        [HttpGet]
        [Route("ShowSlideHeader/{id}")]
        public JsonResult GetSlideshowById(int id)
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

        [HttpGet]
        [Route("FilterSlideshow")]
        public JsonResult FilterSlideshow([FromQuery] DateTime? startDate = null, [FromQuery] DateTime? endDate = null)
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


        [HttpPost]
        [Route("AddSlideshow")]
        public JsonResult AddSlideshow([FromBody] TslideHeader slideshow)
        {
            string query = "INSERT INTO TSlideHeader (description, startDate, endDate, IsActive, timeNext, dateModified, dateCreated) " +
                           "VALUES (@Description, @StartDate, @EndDate, @IsActive, @TimeNext, GETDATE(), GETDATE())";
            SqlParameter[] parameters =
            {
                new SqlParameter("@Description", slideshow.Description),
                new SqlParameter("@StartDate", slideshow.StartDate),
                new SqlParameter("@EndDate", slideshow.EndDate),
                new SqlParameter("@IsActive", slideshow.IsActive),
                new SqlParameter("@TimeNext", slideshow.TimeNext),
            };
            _exQuery.ExecuteRawQuery(query, parameters);
            return new JsonResult("Slideshow added successfully");
        }

        [HttpPut]
        [Route("UpdateSlideshow/{id}")]
        public JsonResult UpdateSlideshow(int id, [FromBody] TslideHeader slideshow)
        {
            string query = "UPDATE TslideHeader SET description = @Description, dateModified = GETDATE(), IsActive = @IsActive, timeNext = @TimeNext, " +
                           "startDate = @StartDate, endDate = @EndDate " +
                           "WHERE id = @Id";
            SqlParameter[] parameters =
            {
                new SqlParameter("@Id", id),
                new SqlParameter("@Description", slideshow.Description),
                new SqlParameter("@StartDate", slideshow.StartDate),
                new SqlParameter("@EndDate", slideshow.EndDate),
                new SqlParameter("@IsActive", slideshow.IsActive),
                new SqlParameter("@TimeNext", slideshow.TimeNext),
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

        [HttpGet]
        [Route("SearchSlideshow")]
        public JsonResult SearchSlideshow(string searchQuery)
        {
            string query = "SELECT sh.id, sh.description, sh.startDate, sh.endDate, sh.IsActive, sh.timeNext, sh.dateModified, sh.dateCreated " +
                           "FROM TSlideHeader sh " +
                           "WHERE sh.id LIKE @searchQuery OR " +
                           "sh.description LIKE @searchQuery ";

            SqlParameter parameter = new SqlParameter("@searchQuery", "%" + searchQuery + "%");
            DataTable table = _exQuery.ExecuteRawQuery(query, new[] { parameter });

            return new JsonResult(table);
        }



    }
}
