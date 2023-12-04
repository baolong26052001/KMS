using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using KMS.Models;
using System;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Text;
using System.Security.Cryptography;
using System.Threading.Tasks;
using KMS.Tools;

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

        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }

        

        [HttpGet]
        [Route("ShowUsers")]
        public JsonResult GetUsers()
        {
            string query = "select u.id, u.username, u.fullname, u.email, ug.groupName,u.lastLogin,u.isActive, DATEDIFF(DAY, u.lastLogin, GETDATE()) AS TotalDaysDormant" +
                "\r\nfrom TUser u, TUserGroup ug" +
                "\r\nwhere u.userGroupId = ug.id;";
            DataTable table = ExecuteRawQuery(query);
            return new JsonResult(table);
        }

        [HttpGet]
        [Route("ShowUsers/{id}")]
        public JsonResult GetUserById(int id)
        {
            string query = "SELECT u.id, u.username, u.fullname, u.email, ug.groupName, u.lastLogin, u.isActive, DATEDIFF(DAY, u.lastLogin, GETDATE()) AS TotalDaysDormant" +
                "\r\nFROM TUser u, TUserGroup ug" +
                "\r\nWHERE u.userGroupId = ug.id AND u.id = @UserId";

            SqlParameter parameter = new SqlParameter("@UserId", id);
            DataTable table = ExecuteRawQuery(query, new[] { parameter });

            if (table.Rows.Count > 0)
            {
                return new JsonResult(table);
            }
            else
            {
                return new JsonResult("User not found");
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

                return Ok(new { message = "Login successful" });
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }

        [HttpPost]
        [Route("AddUser")]
        public JsonResult AddUser([FromBody] Tuser newUser)
        {
            string insertQuery = "INSERT INTO TUser (username, fullname, email, password, userGroupId, lastLogin, isActive, dateCreated, dateModified) " +
                                "VALUES (@Username, @Fullname, @Email, @Password, @UserGroupId, @LastLogin, @IsActive, @DateCreated, @DateModified);";

            SqlParameter[] parameters =
            {
                new SqlParameter("@Username", newUser.Username),
                new SqlParameter("@Fullname", newUser.Fullname),
                new SqlParameter("@Email", newUser.Email),
                new SqlParameter("@Password", Password.hashPassword(newUser.Password)),
                new SqlParameter("@UserGroupId", newUser.UserGroupId),
                new SqlParameter("@LastLogin", DateTime.Now),
                new SqlParameter("@IsActive", newUser.IsActive),
                new SqlParameter("@DateCreated", DateTime.Now),
                new SqlParameter("@DateModified", DateTime.Now),
            };

            ExecuteRawQuery(insertQuery, parameters);

            return new JsonResult("User added successfully");
        }

        [HttpPut]
        [Route("UpdateUser/{id}")]
        public JsonResult UpdateUser(int id, [FromBody] Tuser updatedUser)
        {
            string updateQuery = "UPDATE TUser SET username = @Username, fullname = @Fullname, email = @Email, " +
                                 "password = @Password, userGroupId = @UserGroupId, isActive = @IsActive, " +
                                 "dateModified = @DateModified WHERE id = @Id;";

            SqlParameter[] parameters =
            {
                new SqlParameter("@Id", id),
                new SqlParameter("@Username", updatedUser.Username),
                new SqlParameter("@Fullname", updatedUser.Fullname),
                new SqlParameter("@Email", updatedUser.Email),
                new SqlParameter("@Password", Password.hashPassword(updatedUser.Password)),
                new SqlParameter("@UserGroupId", updatedUser.UserGroupId),
                new SqlParameter("@IsActive", updatedUser.IsActive),
                new SqlParameter("@DateModified", DateTime.Now),
            };

            ExecuteRawQuery(updateQuery, parameters);

            return new JsonResult("User updated successfully");
        }

        [HttpDelete]
        [Route("DeleteUser/{userId}")]
        public JsonResult DeleteUser(int userId)
        {
            string deleteQuery = "DELETE FROM TUser WHERE id = @UserId;";

            SqlParameter parameter = new SqlParameter("@UserId", userId);
            ExecuteRawQuery(deleteQuery, new[] { parameter });

            return new JsonResult("User deleted successfully");
        }

        [HttpGet]
        [Route("SearchUsers")]
        public JsonResult SearchUsers(string searchQuery)
        {
            string query = "SELECT u.id, u.username, u.fullname, u.email, ug.groupName, u.lastLogin, u.isActive, DATEDIFF(DAY, u.lastLogin, GETDATE()) AS TotalDaysDormant " +
                           "FROM TUser u " +
                           "JOIN TUserGroup ug ON u.userGroupId = ug.id " +
                           "WHERE u.username LIKE @searchQuery OR " +
                           "u.fullname LIKE @searchQuery OR " +
                           "u.email LIKE @searchQuery OR " +
                           "ug.groupName LIKE @searchQuery OR " +
                           "CONVERT(VARCHAR(10), u.lastLogin, 120) LIKE @searchQuery OR " +
                           "CAST(u.isActive AS VARCHAR) LIKE @searchQuery OR " +
                           "CAST(DATEDIFF(DAY, u.lastLogin, GETDATE()) AS VARCHAR) LIKE @searchQuery";

            SqlParameter parameter = new SqlParameter("@searchQuery", "%" + searchQuery + "%");
            DataTable table = ExecuteRawQuery(query, new[] { parameter });

            return new JsonResult(table);
        }


    }
}
