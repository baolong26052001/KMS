using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using KMS.Models;
using KMS.Tools;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Microsoft.Data.SqlClient;

namespace KMS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly KioskManagementSystemContext _dbcontext;
        private IConfiguration _configuration;

        public UserController(IConfiguration configuration, KioskManagementSystemContext _context)
        {
            _dbcontext = _context;
            _configuration = configuration;
        }

        [HttpGet]
        [Route("ShowUsers")]
        public JsonResult GetUsers()
        {
            string query = "select u.id, u.username, u.fullname, u.email, ug.groupName,u.lastLogin,u.isActive, DATEDIFF(DAY, u.lastLogin, GETDATE()) AS TotalDaysDormant" +
                "\r\nfrom TUser u, TUserGroup ug" +
                "\r\nwhere u.userGroupId = ug.id;";
            DataTable table = new DataTable();
            string sqlDatasource = _configuration.GetConnectionString("DefaultConnection");
            SqlDataReader myReader;
            using(SqlConnection myCon = new SqlConnection(sqlDatasource))
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
        [Route("ShowUsers/{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            try
            {
                var user = await _dbcontext.Tusers.FindAsync(id);

                if (user == null)
                {
                    return NotFound("User not found");
                }

                return Ok(user);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> userLogin([FromBody] Tuser user)
        {
            try
            {
                string password = Password.hashPassword(user.Password);
                var dbUser = _dbcontext.Tusers
                    .Where(u => u.Username == user.Username && u.Password == password)
                    .FirstOrDefault();

                if (dbUser == null)
                {
                    return BadRequest("Username or password is incorrect");
                }

                // Update LastLogin when the user logs in
                dbUser.LastLogin = DateTime.Now;
                await _dbcontext.SaveChangesAsync();

                return Ok("Login successful");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        [HttpPost]
        [Route("AddUser")]
        public async Task<IActionResult> userRegistration([FromBody] Tuser user)
        {
            try
            {
                var dbUser = _dbcontext.Tusers.Where(u => u.Username == user.Username).Select(u => new
                {
                    u.Id,
                    u.Username,
                    u.IsActive
                }).FirstOrDefault();

                if (dbUser != null)
                {
                    return BadRequest("Username already exist");
                }
                user.DateCreated = DateTime.Now;

                user.Password = Password.hashPassword(user.Password);
                user.IsActive = true;
                _dbcontext.Tusers.Add(user);
                await _dbcontext.SaveChangesAsync();
                return Ok("User is successfully registered");
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
            

            
        }

        [HttpPut]
        [Route("UpdateUser/{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] Tuser updatedUser)
        {
            try
            {
                var existingUser = await _dbcontext.Tusers.FindAsync(id);

                if (existingUser == null)
                {
                    return NotFound("User not found");
                }

                existingUser.DateModified = DateTime.Now;
                // Update properties of existingUser with values from updatedUser
                existingUser.Email = updatedUser.Email;
                

                await _dbcontext.SaveChangesAsync();

                return Ok("User updated successfully");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete]
        [Route("DeleteUser/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                var userToDelete = await _dbcontext.Tusers.FindAsync(id);

                if (userToDelete == null)
                {
                    return NotFound("User not found");
                }

                _dbcontext.Tusers.Remove(userToDelete);
                await _dbcontext.SaveChangesAsync();

                return Ok("User deleted successfully");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }

    
}