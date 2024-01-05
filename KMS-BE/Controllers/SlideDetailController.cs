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

        [HttpPost]
        [Route("AddSlideDetail")]
        public JsonResult AddSlideDetail([FromBody] TslideDetail slideDetail)
        {
            string query = "INSERT INTO TSlideDetail (description, typeContent, contentUrl, slideHeaderId, dateModified, dateCreated, isActive) " +
                           "VALUES (@Description, @TypeContent, @ContentUrl, @SlideHeaderId, GETDATE(), GETDATE(), 1)";
            SqlParameter[] parameters =
            {
                new SqlParameter("@Description", slideDetail.Description),
                new SqlParameter("@TypeContent", slideDetail.TypeContent),
                new SqlParameter("@ContentUrl", slideDetail.ContentUrl),
                new SqlParameter("@SlideHeaderId", slideDetail.SlideHeaderId),
                new SqlParameter("@IsActive", slideDetail.IsActive),
            };
            _exQuery.ExecuteRawQuery(query, parameters);
            return new JsonResult("Slide detail added successfully");
        }

        [HttpPut]
        [Route("UpdateSlideDetail/{id}")]
        public JsonResult UpdateSlideDetail(int id, [FromBody] TslideDetail slideDetail)
        {
            string query = "UPDATE TSlideDetail SET description = @Description, typeContent = @TypeContent, contentUrl = @ContentUrl, slideHeaderId = @SlideHeaderId, dateModified = GETDATE(), isActive = @IsActive  " +
                           "WHERE id = @Id";
            SqlParameter[] parameters =
            {
                new SqlParameter("@Id", id),
                new SqlParameter("@Description", slideDetail.Description),
                new SqlParameter("@TypeContent", slideDetail.TypeContent),
                new SqlParameter("@ContentUrl", slideDetail.ContentUrl),
                new SqlParameter("@SlideHeaderId", slideDetail.SlideHeaderId),
                new SqlParameter("@IsActive", slideDetail.IsActive),
            };

            _exQuery.ExecuteRawQuery(query, parameters);
            return new JsonResult("Slide detail updated successfully");
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
        [Route("DeleteSlideDetail")]
        public JsonResult DeleteSlideDetail([FromBody] List<int> slideDetailIds)
        {
            if (slideDetailIds == null || slideDetailIds.Count == 0)
            {
                return new JsonResult("No slideshow IDs provided for deletion");
            }

            StringBuilder deleteQuery = new StringBuilder("DELETE FROM TSlideDetail WHERE id IN (");

            List<SqlParameter> parameters = new List<SqlParameter>();

            for (int i = 0; i < slideDetailIds.Count; i++)
            {
                string parameterName = "@SlideDetailId" + i;
                deleteQuery.Append(parameterName);

                if (i < slideDetailIds.Count - 1)
                {
                    deleteQuery.Append(", ");
                }

                parameters.Add(new SqlParameter(parameterName, slideDetailIds[i]));
            }

            deleteQuery.Append(");");

            _exQuery.ExecuteRawQuery(deleteQuery.ToString(), parameters.ToArray());

            return new JsonResult("Slide detail deleted successfully");
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
