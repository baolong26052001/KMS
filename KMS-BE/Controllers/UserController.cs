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
using System.Net;
using IPinfo;
using System.Net.Sockets;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;


namespace KMS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly KioskManagementSystemContext _dbcontext;
        private IConfiguration _configuration;
        private readonly ExecuteQuery _exQuery;

        public UserController(IConfiguration configuration, KioskManagementSystemContext _context, ExecuteQuery exQuery)
        {
            _dbcontext = _context;
            _configuration = configuration;
            _exQuery = exQuery;
        }

        [HttpGet]
        [Route("ShowUsers")]
        public JsonResult GetUsers()
        {
            string query = "SELECT u.id, u.username, u.fullname, u.email, ug.groupName, u.lastLogin, u.isActive, DATEDIFF(DAY, u.lastLogin, GETDATE()) AS TotalDaysDormant " +
                " FROM TUser u \r\nLEFT JOIN TUserGroup ug ON u.userGroupId = ug.id " +
                " WHERE ug.id IS NOT NULL or ug.id IS NULL";
            DataTable table = _exQuery.ExecuteRawQuery(query);
            return new JsonResult(table);
        }

        [HttpGet, Authorize(Roles = "Admin")]
        [Route("ShowUsers/{id}")]
        public JsonResult GetUserById(int id)
        {
            string query = "SELECT u.id, u.username, u.fullname, u.email, ug.groupName, u.lastLogin, u.isActive, DATEDIFF(DAY, u.lastLogin, GETDATE()) AS TotalDaysDormant " +
                " FROM TUser u \r\nLEFT JOIN TUserGroup ug ON u.userGroupId = ug.id " +
                " WHERE u.id = @UserId and (ug.id IS NOT NULL or ug.id IS NULL)";

            SqlParameter parameter = new SqlParameter("@UserId", id);
            DataTable table = _exQuery.ExecuteRawQuery(query, new[] { parameter });

            if (table.Rows.Count > 0)
            {
                return new JsonResult(table);
            }
            else
            {
                return new JsonResult("User not found");
            }
        }

        [HttpGet]
        [Route("ShowUsersInEditPage/{id}")]
        public JsonResult GetUserByIdInEditPage(int id)
        {
            string query = "SELECT u.id, u.username, u.fullname, u.password, u.email, u.userGroupId, u.isActive" +
                "\r\nFROM TUser u, TUserGroup ug" +
                "\r\nWHERE u.userGroupId = ug.id AND u.id = @UserId";

            SqlParameter parameter = new SqlParameter("@UserId", id);
            DataTable table = _exQuery.ExecuteRawQuery(query, new[] { parameter });

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
                int idOfUser = 0;
                var ipAddress = HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault() ?? HttpContext.Connection.RemoteIpAddress.ToString();
                string host = Dns.GetHostName();
                IPHostEntry ip = Dns.GetHostEntry(host);

                string password = Password.hashPassword(user.Password);

                var dbUser = _dbcontext.Tusers
                    .Where(u => u.Username == user.Username && u.Password == password)
                    .FirstOrDefault();

                int groupId = (int)dbUser.UserGroupId;

                if (dbUser == null)
                {
                    return BadRequest("Username or password is incorrect");
                }
                else
                {
                    idOfUser = dbUser.Id;
                }

                
                dbUser.LastLogin = DateTime.Now;
                await _dbcontext.SaveChangesAsync();


                string query = "INSERT INTO TAudit (userId, action, ipAddress, macAddress, dateCreated) VALUES (@UserId, 'Login', @IpAddress, @Ipv6, GETDATE())";

                IPAddress ipv4 = null;
                IPAddress ipv6 = null;

                foreach (var address in ip.AddressList)
                {
                    if (address.AddressFamily == AddressFamily.InterNetwork) // IPv4
                    {
                        ipv4 = address;
                    }
                    else if (address.AddressFamily == AddressFamily.InterNetworkV6) // IPv6
                    {
                        ipv6 = address;
                    }
                }

                SqlParameter[] parameters =
                {
                    new SqlParameter("@IpAddress", ipv4?.ToString()),
                    new SqlParameter("@Ipv6", ipv6?.ToString()),
                    new SqlParameter("@UserId", idOfUser),
                };

                _exQuery.ExecuteRawQuery(query, parameters);

                string token = CreateToken(user, groupId);
                return Ok(new { message = "Login successful", Token = token, GroupId = groupId });
                
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
                                "VALUES (@Username, @Fullname, @Email, @Password, @UserGroupId, GETDATE(), @IsActive, GETDATE(), GETDATE());";

            string query2 = "INSERT INTO TAudit (action, tableName, dateModified, dateCreated, isActive) VALUES ('Add', 'TUser', GETDATE(), GETDATE(), 1)";

            SqlParameter[] parameters =
            {
                new SqlParameter("@Username", newUser.Username),
                new SqlParameter("@Fullname", newUser.Fullname),
                new SqlParameter("@Email", newUser.Email),
                new SqlParameter("@Password", Password.hashPassword(newUser.Password)),
                new SqlParameter("@UserGroupId", newUser.UserGroupId),
                new SqlParameter("@IsActive", newUser.IsActive),
            };

            SqlParameter[] parameters2 =
            {

            };

            _exQuery.ExecuteRawQuery(insertQuery, parameters);
            _exQuery.ExecuteRawQuery(query2, parameters2);

            

            return new JsonResult(new { Message = "User added successfully" });
        }

        private JsonResult GetGroupName()
        {
            string query = "select TUserGroup.groupName from tuser, tusergroup where tuser.userGroupId = TUserGroup.id group by TUserGroup.groupName";
            DataTable table = _exQuery.ExecuteRawQuery(query);
            return new JsonResult(table);
        }

        private string CreateToken(Tuser user, int groupId)
        {
            string roleName = "";

            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
            };

            if (groupId == 1)
            {
                claims.Add(new Claim(ClaimTypes.Role, "Admin"));
            }

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }


        [HttpPut]
        [Route("UpdateUser/{id}")]
        public JsonResult UpdateUser(int id, [FromBody] Tuser updatedUser)
        {
            string updateQuery = "UPDATE TUser SET username = @Username, fullname = @Fullname, email = @Email, " +
                                 "password = @Password, userGroupId = @UserGroupId, isActive = @IsActive, " +
                                 "dateModified = GETDATE() WHERE id = @Id;";

            string query2 = "INSERT INTO TAudit (action, tableName, dateModified, dateCreated, isActive) VALUES ('Update', 'TUser', GETDATE(), GETDATE(), 1)";

            SqlParameter[] parameters =
            {
                new SqlParameter("@Id", id),
                new SqlParameter("@Username", updatedUser.Username),
                new SqlParameter("@Fullname", updatedUser.Fullname),
                new SqlParameter("@Email", updatedUser.Email),
                new SqlParameter("@Password", Password.hashPassword(updatedUser.Password)),
                new SqlParameter("@UserGroupId", updatedUser.UserGroupId),
                new SqlParameter("@IsActive", updatedUser.IsActive),
            };

            SqlParameter[] parameters2 =
            {

            };

            _exQuery.ExecuteRawQuery(updateQuery, parameters);
            _exQuery.ExecuteRawQuery(query2, parameters2);

            return new JsonResult("User updated successfully");
        }

        [HttpDelete]
        [Route("DeleteUsers")]
        public JsonResult DeleteUsers([FromBody] List<int> userIds)
        {
            if (userIds == null || userIds.Count == 0)
            {
                return new JsonResult("No user IDs provided for deletion");
            }

            StringBuilder deleteQuery = new StringBuilder("DELETE FROM TUser WHERE id IN (");
            string query2 = "INSERT INTO TAudit (action, tableName, dateModified, dateCreated, isActive) VALUES ('Delete', 'TUser', GETDATE(), GETDATE(), 1)";

            List<SqlParameter> parameters = new List<SqlParameter>();
            SqlParameter[] parameters2 =
            {

            };

            for (int i = 0; i < userIds.Count; i++)
            {
                string parameterName = "@UserId" + i;
                deleteQuery.Append(parameterName);

                if (i < userIds.Count - 1)
                {
                    deleteQuery.Append(", ");
                }

                parameters.Add(new SqlParameter(parameterName, userIds[i]));
            }

            deleteQuery.Append(");");

            _exQuery.ExecuteRawQuery(deleteQuery.ToString(), parameters.ToArray());
            _exQuery.ExecuteRawQuery(query2, parameters2);

            return new JsonResult("Users deleted successfully");
        }

        [HttpGet]
        [Route("SearchUsers")]
        public JsonResult SearchUsers(string searchQuery)
        {
            string query = "SELECT u.id, u.username, u.fullname, u.email, ug.groupName, u.lastLogin, u.isActive, DATEDIFF(DAY, u.lastLogin, GETDATE()) AS TotalDaysDormant " +
                           "FROM TUser u " +
                           "LEFT JOIN TUserGroup ug ON u.userGroupId = ug.id " +
                           "WHERE (u.id LIKE @searchQuery OR " +
                           "u.username LIKE @searchQuery OR " +
                           "u.fullname LIKE @searchQuery OR " +
                           "u.email LIKE @searchQuery OR " +
                           "ug.groupName LIKE @searchQuery) AND " +
                           "(ug.id IS NOT NULL or ug.id IS NULL)";

            SqlParameter parameter = new SqlParameter("@searchQuery", "%" + searchQuery + "%");
            DataTable table = _exQuery.ExecuteRawQuery(query, new[] { parameter });

            return new JsonResult(table);
        }



    }
}
