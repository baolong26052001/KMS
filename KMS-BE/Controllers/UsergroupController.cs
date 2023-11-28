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

        [HttpPost]
        [Route("AddUsergroup")]
        public IActionResult AddUsergroup([FromBody] TuserGroup usergroup)
        {
            try
            {

                usergroup.DateCreated = DateTime.Now;
                usergroup.IsActive = true;

                _dbcontext.TuserGroups.Add(usergroup);
                _dbcontext.SaveChanges();
                return Ok("User group added successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("UpdateUsergroup/{id}")]
        public IActionResult UpdateUsergroup(int id, [FromBody] TuserGroup updatedUsergroup)
        {
            try
            {
                var existingUsergroup = _dbcontext.TuserGroups.Find(id);
                if (existingUsergroup != null)
                {
                    existingUsergroup.DateModified = DateTime.Now;
                    // Update properties of existingUsergroup with values from updatedUsergroup
                    existingUsergroup.GroupName = updatedUsergroup.GroupName;
                    

                    _dbcontext.SaveChanges();
                    return Ok("User group updated successfully");
                }
                return NotFound("User group not found");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("DeleteUsergroup/{id}")]
        public IActionResult DeleteUsergroup(int id)
        {
            try
            {
                var usergroupToDelete = _dbcontext.TuserGroups.Find(id);
                if (usergroupToDelete != null)
                {
                    _dbcontext.TuserGroups.Remove(usergroupToDelete);
                    _dbcontext.SaveChanges();
                    return Ok("User group deleted successfully");
                }
                return NotFound("User group not found");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}