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
        public JsonResult GetUserById(int id)
        {
            string query = "SELECT u.id, u.username, u.fullname, u.email, ug.groupName, u.lastLogin, u.isActive, DATEDIFF(DAY, u.lastLogin, GETDATE()) AS TotalDaysDormant" +
                "\r\nFROM TUser u, TUserGroup ug" +
                "\r\nWHERE u.userGroupId = ug.id AND u.id = @UserId";

            DataTable table = new DataTable();
            string sqlDatasource = _configuration.GetConnectionString("DefaultConnection");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDatasource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@UserId", id);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult(table);
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
        public JsonResult UpdateUser(int id, [FromBody] Tuser updatedUser)
        {
            string updateQuery = "UPDATE TUser SET username = @Username, fullname = @Fullname, email = @Email, " +
                                 "password = @Password, userGroupId = @UserGroupId, isActive = @IsActive, " +
                                 "dateModified = @DateModified WHERE id = @Id;";

            string sqlDatasource = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection myCon = new SqlConnection(sqlDatasource))
            {
                myCon.Open();

                using (SqlCommand myCommand = new SqlCommand(updateQuery, myCon))
                {
                    // Hash the password using SHA-256 and then encode with Base64 (for educational purposes only)
                    string hashedPassword = HashPassword(updatedUser.Password);

                    myCommand.Parameters.AddWithValue("@Id", id);
                    myCommand.Parameters.AddWithValue("@Username", updatedUser.Username);
                    myCommand.Parameters.AddWithValue("@Fullname", updatedUser.Fullname);
                    myCommand.Parameters.AddWithValue("@Email", updatedUser.Email);
                    myCommand.Parameters.AddWithValue("@Password", hashedPassword);
                    myCommand.Parameters.AddWithValue("@UserGroupId", updatedUser.UserGroupId);
                    myCommand.Parameters.AddWithValue("@IsActive", updatedUser.IsActive);
                    myCommand.Parameters.AddWithValue("@DateModified", DateTime.Now);

                    myCommand.ExecuteNonQuery();
                }
            }

            return new JsonResult("User updated successfully");
        }



        [HttpDelete]
        [Route("DeleteUser/{userId}")]
        public JsonResult DeleteUser(int userId)
        {
            string deleteQuery = "DELETE FROM TUser WHERE id = @UserId;";

            string sqlDatasource = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection myCon = new SqlConnection(sqlDatasource))
            {
                myCon.Open();

                using (SqlCommand myCommand = new SqlCommand(deleteQuery, myCon))
                {
                    myCommand.Parameters.AddWithValue("@UserId", userId);

                    myCommand.ExecuteNonQuery();
                }
            }

            return new JsonResult("User deleted successfully");
        }

    }


}