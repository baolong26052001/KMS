using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using KMS.Models;
using KMS.Tools;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Text;
using System;
using System.Security.Cryptography;



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
        public JsonResult AddUser([FromBody] Tuser newUser)
        {
            // Assuming UserDTO is a Data Transfer Object representing the user details
            // You should replace UserDTO with your actual user model or class

            string insertQuery = "INSERT INTO TUser (username, fullname, email, password, userGroupId, lastLogin, isActive, dateCreated, dateModified) " +
                         "VALUES (@Username, @Fullname, @Email, @Password, @UserGroupId, @LastLogin, @IsActive, @DateCreated, @DateModified);";


            string sqlDatasource = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection myCon = new SqlConnection(sqlDatasource))
            {
                myCon.Open();

                using (SqlCommand myCommand = new SqlCommand(insertQuery, myCon))
                {
                    // Hash the password using SHA-256 and then encode with Base64 (for educational purposes only)
                    string hashedPassword = HashPassword(newUser.Password);

                    myCommand.Parameters.AddWithValue("@Username", newUser.Username);
                    myCommand.Parameters.AddWithValue("@Fullname", newUser.Fullname);
                    myCommand.Parameters.AddWithValue("@Email", newUser.Email);
                    myCommand.Parameters.AddWithValue("@Password", hashedPassword);
                    myCommand.Parameters.AddWithValue("@UserGroupId", newUser.UserGroupId); // Assuming there's a property for UserGroupId in your DTO
                    myCommand.Parameters.AddWithValue("@LastLogin", DateTime.Now); // Set the current date/time as the last login for a new user
                    myCommand.Parameters.AddWithValue("@IsActive", newUser.IsActive); // Assuming there's a property for IsActive in your DTO
                    myCommand.Parameters.AddWithValue("@DateCreated", DateTime.Now); // Set the current date/time as the creation date for a new user
                    myCommand.Parameters.AddWithValue("@DateModified", DateTime.Now); // Set the current date/time as the modification date for a new user

                    myCommand.ExecuteNonQuery();
                }
            }

            return new JsonResult("User added successfully");
        }

        // Hash the password using SHA-256 and then encode with Base64 (for educational purposes only)
        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
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
                if (updatedUser.Fullname != null)
                {
                    existingUser.Fullname = updatedUser.Fullname;
                }
                
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