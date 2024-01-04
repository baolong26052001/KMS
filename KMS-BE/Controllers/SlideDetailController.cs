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
    public class SlideDetailController : ControllerBase
    {
        private readonly KioskManagementSystemContext _dbcontext;
        private IConfiguration _configuration;
        private readonly ExecuteQuery _exQuery;

        public SlideDetailController(IConfiguration configuration, KioskManagementSystemContext _context, ExecuteQuery exQuery)
        {
            _dbcontext = _context;
            _configuration = configuration;
            _exQuery = exQuery;
        }

        [HttpGet]
        [Route("ShowSlideDetail/{id}")]
        public JsonResult GetSlideDetailById(int id)
        {
            string query = "select sd.id, sd.description, sd.typeContent, sd.contentUrl, sd.slideHeaderId, sd.isActive, sd.dateModified, sd.dateCreated " +
                "\r\nfrom TSlideDetail sd " +
                "WHERE slideHeaderId = @Id";
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

        [HttpGet]
        [Route("FilterSlideDetail/{id}")]
        public JsonResult FilterSlideDetail(int id, [FromQuery] DateTime? startDate = null, [FromQuery] DateTime? endDate = null)
        {
            string query = "select sd.id, sd.description, sd.typeContent, sd.contentUrl, sd.slideHeaderId, sd.isActive, sd.dateModified, sd.dateCreated " +
                           "FROM TSlideDetail sd " +
                           "WHERE @Id = slideHeaderId";

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

        [HttpDelete]
        [Route("DeleteSlideshow")]
        public JsonResult DeleteSlideshow([FromBody] List<int> slideshowIds)
        {
            if (slideshowIds == null || slideshowIds.Count == 0)
            {
                return new JsonResult("No slideshow IDs provided for deletion");
            }

            StringBuilder deleteQuery = new StringBuilder("DELETE FROM TSlideDetail WHERE id IN (");

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
        [Route("SearchSlideDetail")]
        public JsonResult SearchSlideDetail(string searchQuery)
        {
            string query = "SELECT  sd.id, sd.description, sd.typeContent, sd.contentUrl, sd.slideHeaderId, sd.isActive, sd.dateModified, sd.dateCreated " +
                           "FROM TSlideDetail sd " +
                           "WHERE sd.id LIKE @searchQuery OR " +
                           "sd.description LIKE @searchQuery OR " +
                           "sd.typeContent LIKE @searchQuery OR " +
                           "sd.slideHeaderId LIKE @searchQuery";

            SqlParameter parameter = new SqlParameter("@searchQuery", "%" + searchQuery + "%");
            DataTable table = _exQuery.ExecuteRawQuery(query, new[] { parameter });

            return new JsonResult(table);
        }

    }
}
