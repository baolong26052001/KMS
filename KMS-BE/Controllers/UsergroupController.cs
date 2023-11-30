using Microsoft.AspNetCore.Mvc;
using KMS.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
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

                myCon.Close();
            }

            return table;
        }

        [HttpGet]
        [Route("ShowUsergroup")]
        public JsonResult GetUsergroup()
        {
            string query = "select ug.id, ug.groupName, ug.dateModified, ug.dateCreated, ug.isActive" +
                "\r\nfrom TUserGroup ug";
            DataTable table = ExecuteRawQuery(query);
            return new JsonResult(table);
        }

        [HttpGet]
        [Route("ShowUsergroup/{id}")]
        public JsonResult GetUsergroupById(int id)
        {
            string query = "SELECT id, groupName, dateModified, dateCreated, isActive " +
                           "FROM TUserGroup " +
                           "WHERE id = @Id";

            SqlParameter parameter = new SqlParameter("@Id", id);
            DataTable table = ExecuteRawQuery(query, new[] { parameter });

            if (table.Rows.Count > 0)
            {
                return new JsonResult(table);
            }
            else
            {
                return new JsonResult("Usergroup not found");
            }
        }

        [HttpPost]
        [Route("AddUsergroup")]
        public JsonResult AddUsergroup([FromBody] TuserGroup usergroup)
        {
            string query = "INSERT INTO TUserGroup (groupName, accessRuleId, dateModified, dateCreated, isActive) " +
                           "VALUES (@GroupName, @AccessRuleId, GETDATE(), GETDATE(), @IsActive)";

            SqlParameter[] parameters =
            {
                new SqlParameter("@GroupName", usergroup.GroupName),
                new SqlParameter("@AccessRuleId", usergroup.AccessRuleId),
                new SqlParameter("@IsActive", usergroup.IsActive)
            };

            ExecuteRawQuery(query, parameters);

            return new JsonResult("Usergroup added successfully");
        }

        [HttpPut]
        [Route("UpdateUsergroup/{id}")]
        public JsonResult UpdateUsergroup(int id, [FromBody] TuserGroup usergroup)
        {
            string query = "UPDATE TUserGroup " +
                           "SET groupName = @GroupName, accessRuleId = @AccessRuleId, dateModified = GETDATE(), isActive = @IsActive " +
                           "WHERE id = @Id";

            SqlParameter[] parameters =
            {
                new SqlParameter("@Id", id),
                new SqlParameter("@GroupName", usergroup.GroupName),
                new SqlParameter("@AccessRuleId", usergroup.AccessRuleId),
                new SqlParameter("@IsActive", usergroup.IsActive)
            };

            ExecuteRawQuery(query, parameters);

            return new JsonResult("Usergroup updated successfully");
        }

        [HttpDelete]
        [Route("DeleteUsergroup/{id}")]
        public JsonResult DeleteUsergroup(int id)
        {
            string query = "DELETE FROM TUserGroup WHERE id = @Id";

            SqlParameter parameter = new SqlParameter("@Id", id);
            ExecuteRawQuery(query, new[] { parameter });

            return new JsonResult("Usergroup deleted successfully");
        }
    }
}
