using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using System.IO;
using KMS.Models;
using KMS.Tools;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using KMS.Models;
using KMS.Tools;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace KMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompareImageName : ControllerBase
    {
        

        private readonly KioskManagementSystemContext _dbcontext;
        private IConfiguration _configuration;
        private readonly ExecuteQuery _exQuery;

        public CompareImageName(IConfiguration configuration, KioskManagementSystemContext _context, ExecuteQuery exQuery)
        {
            _dbcontext = _context;
            _configuration = configuration;
            _exQuery = exQuery;
            
        }

        [HttpGet]
        [Route("Compare/{imageName}")]
        public JsonResult CompareImage(string imageName)
        {
            try
            {
                string query = "SELECT * FROM LMember WHERE LOWER(imageIdCard) LIKE '%' + LOWER(@imageName) + '%'";
                
                SqlParameter parameter = new SqlParameter("@imageName", imageName);
                DataTable table = _exQuery.ExecuteRawQuery(query, new[] { parameter });

                if (table.Rows.Count > 0)
                {
                    return new JsonResult(table);
                }
                else
                {
                    return new JsonResult("Image not found");
                }
            }
            catch (Exception ex)
            {
                return new JsonResult("Internal server error") { StatusCode = 500 };
            }
        }


    }
}
