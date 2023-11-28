using Microsoft.AspNetCore.Mvc;
using KMS.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using System.Data;

namespace KMS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SlideshowController : ControllerBase
    {
        private readonly KioskManagementSystemContext _dbcontext;
        private IConfiguration _configuration;


        public SlideshowController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        [Route("ShowSlideshow")]
        public JsonResult GetSlideshow()
        {

            string query = "select ss.id, ss.packageName, ss.imagevideo, ss.fileType, ss.startDate, ss.endDate" +
                "\r\nfrom TSlideshow ss";
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

        


    }
}