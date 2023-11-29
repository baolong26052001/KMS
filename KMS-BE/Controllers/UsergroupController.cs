using Microsoft.AspNetCore.Mvc;
using KMS.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace KMS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsergroupController : ControllerBase
    {
        private readonly KioskManagementSystemContext _dbcontext;
        private IConfiguration _configuration;

        public UsergroupController(IConfiguration configuration, KioskManagementSystemContext _context)
        {
            _dbcontext = _context;
            _configuration = configuration;
        }

        [HttpGet]
        [Route("ShowUsergroup")]
        public JsonResult GetUsergroup()
        {
            string query = "select ug.id, ug.groupName, ug.dateModified, ug.dateCreated, ug.isActive" +
                "\r\nfrom TUserGroup ug";
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
        [Route("ShowUsergroup/{id}")]
        public JsonResult GetUsergroupById(int id)
        {
            string query = "SELECT id, groupName, dateModified, dateCreated, isActive " +
                           "FROM TUserGroup " +
                           "WHERE id = @Id";

            DataTable table = new DataTable();
            string sqlDatasource = _configuration.GetConnectionString("DefaultConnection");
            SqlDataReader myReader;

            using (SqlConnection myCon = new SqlConnection(sqlDatasource))
            {
                myCon.Open();

                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@Id", id);

                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                }

                myCon.Close();
            }

            return new JsonResult(table);
        }


        [HttpPost]
        [Route("AddUsergroup")]
        public JsonResult AddUsergroup([FromBody] TuserGroup usergroup)
        {
            string query = "INSERT INTO TUserGroup (groupName, accessRuleId, dateModified, dateCreated, isActive) " +
                           "VALUES (@GroupName, @AccessRuleId, GETDATE(), GETDATE(), @IsActive)";

            using (SqlConnection myCon = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@GroupName", usergroup.GroupName);
                    myCommand.Parameters.AddWithValue("@AccessRuleId", usergroup.AccessRuleId);
                    myCommand.Parameters.AddWithValue("@IsActive", usergroup.IsActive);

                    myCommand.ExecuteNonQuery();
                }
            }
            return new JsonResult("Usergroup added successfully");
        }


        [HttpPut]
        [Route("UpdateUsergroup/{id}")]
        public JsonResult UpdateUsergroup(int id, [FromBody] TuserGroup usergroup)
        {
            string query = "UPDATE TUserGroup " +
                           "SET groupName = @GroupName, accessRuleId = @AccessRuleId,dateModified = GETDATE(), isActive = @IsActive " +
                           "WHERE id = @Id";

            using (SqlConnection myCon = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@Id", id);
                    myCommand.Parameters.AddWithValue("@GroupName", usergroup.GroupName);
                    myCommand.Parameters.AddWithValue("@AccessRuleId", usergroup.AccessRuleId);
                    myCommand.Parameters.AddWithValue("@IsActive", usergroup.IsActive);

                    myCommand.ExecuteNonQuery();
                }
            }
            return new JsonResult("Usergroup updated successfully");
        }


        [HttpDelete]
        [Route("DeleteUsergroup/{id}")]
        public JsonResult DeleteUsergroup(int id)
        {
            string query = "DELETE FROM TUserGroup WHERE id = @Id";

            using (SqlConnection myCon = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@Id", id);

                    myCommand.ExecuteNonQuery();
                }
            }
            return new JsonResult("Usergroup deleted successfully");
        }

    }
}