using KMS.Models;
using KMS.Tools;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;
using System;

namespace KMS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SlideDetailController : ControllerBase
    {
        private readonly KioskManagementSystemContext _dbcontext;
        private IConfiguration _configuration;
        private readonly ExecuteQuery _exQuery;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public SlideDetailController(IWebHostEnvironment hostingEnvironment, IConfiguration configuration, KioskManagementSystemContext _context, ExecuteQuery exQuery)
        {
            _hostingEnvironment = hostingEnvironment;
            _dbcontext = _context;
            _configuration = configuration;
            _exQuery = exQuery;
        }

        [HttpGet]
        [Route("ShowSlideDetail/{id}")]
        public JsonResult GetSlideDetailById(int id) // show by header id
        {
            string query = "select sd.id, sd.description, sd.typeContent, sd.contentUrl, sd.slideHeaderId, sd.isActive, sd.dateModified, sd.dateCreated " +
                "\r\nfrom TSlideDetail sd " +
                "WHERE sd.slideHeaderId = @Id";
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
        [Route("ShowSlideDetailInEditPage/{id}")]
        public JsonResult GetSlideDetailInEditPage(int id) // show data in edit page
        {
            string query = "select sd.id, sd.description, sd.typeContent, sd.contentUrl, sd.slideHeaderId, sd.isActive, sd.dateModified, sd.dateCreated " +
                "\r\nfrom TSlideDetail sd " +
                "WHERE sd.id = @Id";
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
        public JsonResult AddSlideDetail([FromForm] TslideDetail slideDetail)
        {
            try
            {
                
                if (slideDetail.File != null && slideDetail.File.Length > 0)
                {
                    
                    var localFolderPath = "../KioskApp/Insurance/bin/Debug/net6.0-windows/images/";


                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + slideDetail.File.FileName;
                    var filePath = Path.Combine(localFolderPath, uniqueFileName);

                    
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        slideDetail.File.CopyTo(stream);
                    }

                    
                    slideDetail.ContentUrl = uniqueFileName;
                }

                else
                {
                    return new JsonResult("Cannot detect image file");
                }

                
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
            catch (Exception ex)
            {
                
                return new JsonResult($"Error adding slide detail: {ex.Message}");
            }
        }

        [HttpPut]
        [Route("UpdateSlideDetail/{id}")]
        public JsonResult UpdateSlideDetail(int id, [FromForm] TslideDetail slideDetail)
        {
            try
            {
                string oldContentUrl = GetOldContentUrl(id);

                if (slideDetail.File != null && slideDetail.File.Length > 0)
                {
                    
                    var localFolderPath = "../KioskApp/Insurance/bin/Debug/net6.0-windows/images/";


                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + slideDetail.File.FileName;
                    var filePath = Path.Combine(localFolderPath, uniqueFileName);

                    
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        slideDetail.File.CopyTo(stream);
                    }

                    
                    slideDetail.ContentUrl = uniqueFileName;

                    var existingFilePath = Path.Combine(localFolderPath, oldContentUrl);
                    if (System.IO.File.Exists(existingFilePath))
                    {
                        System.IO.File.Delete(existingFilePath);
                    }


                }

                else
                {
                    return new JsonResult("Cannot detect image file");
                }




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
            catch (Exception ex)
            {
                
                return new JsonResult($"Error adding slide detail: {ex.Message}");
            }
            
        }

        private string GetOldContentUrl(int id)
        {
            string query = "SELECT ContentUrl FROM TSlideDetail WHERE id = @Id";
            SqlParameter parameter = new SqlParameter("@Id", id);
            DataTable table = _exQuery.ExecuteRawQuery(query, new[] { parameter });

            if (table.Rows.Count > 0)
            {
                return table.Rows[0]["ContentUrl"].ToString();
            }
            else
            {
                
                return null;
            }
        }

        [HttpGet]
        [Route("FilterSlideDetail/{id}")]
        public JsonResult FilterSlideDetail(int id, [FromQuery] DateTime? startDate = null, [FromQuery] DateTime? endDate = null)
        {
            string query = "select sd.id, sd.description, sd.typeContent, sd.contentUrl, sd.slideHeaderId, sd.isActive, sd.dateModified, sd.dateCreated " +
                           "FROM TSlideDetail sd " +
                           "WHERE slideHeaderId = @Id";

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

            
            List<string> oldContentUrls = new List<string>();
            foreach (int id in slideDetailIds)
            {
                string oldContentUrl = GetOldContentUrl(id);
                if (!string.IsNullOrEmpty(oldContentUrl))
                {
                    oldContentUrls.Add(oldContentUrl);
                }
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

            
            foreach (string contentUrl in oldContentUrls)
            {
                System.IO.File.Delete("../KioskApp/Insurance/bin/Debug/net6.0-windows/images/" + contentUrl);
            }

            return new JsonResult("Slide detail and associated image files deleted successfully");
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
