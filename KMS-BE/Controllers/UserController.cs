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
            ResponseDto response = new ResponseDto();
            try
            {
                string query = "SELECT u.id, u.username, u.fullname, u.email, ug.groupName, u.lastLogin, u.isActive, DATEDIFF(DAY, u.lastLogin, GETDATE()) AS TotalDaysDormant " +
                " FROM TUser u \r\nLEFT JOIN TUserGroup ug ON u.userGroupId = ug.id " +
                " WHERE ug.id IS NOT NULL or ug.id IS NULL";
                DataTable table = _exQuery.ExecuteRawQuery(query);
                response.Data = table;
                return new JsonResult(table);
            }
            catch (Exception ex)
            {
                response.Code = -1;
                response.Message = ex.Message;
                response.Exception = ex.ToString();
                response.Data = null;
            }
            return new JsonResult(response);
        }

        [HttpGet]
        [Route("ShowUsers/{id}")]
        public JsonResult GetUserById(int id)
        {
            ResponseDto response = new ResponseDto();
            try
            {
                string query = "SELECT u.id, u.username, u.fullname, u.email, ug.groupName, u.lastLogin, u.isActive, DATEDIFF(DAY, u.lastLogin, GETDATE()) AS TotalDaysDormant " +
                " FROM TUser u \r\nLEFT JOIN TUserGroup ug ON u.userGroupId = ug.id " +
                " WHERE u.id = @UserId and (ug.id IS NOT NULL or ug.id IS NULL)";

                SqlParameter parameter = new SqlParameter("@UserId", id);
                DataTable table = _exQuery.ExecuteRawQuery(query, new[] { parameter });
                response.Data = table;

                if (table.Rows.Count > 0)
                {
                    return new JsonResult(table);
                }
                else
                {
                    return new JsonResult("User not found");
                }
            }
            catch (Exception ex)
            {
                response.Code = -1;
                response.Message = ex.Message;
                response.Exception = ex.ToString();
                response.Data = null;
            }
            return new JsonResult(response);
        }

        [HttpGet]
        [Route("ShowUsersInEditPage/{id}")]
        public JsonResult GetUserByIdInEditPage(int id)
        {
            ResponseDto response = new ResponseDto();
            try
            {
                string query = "SELECT u.id, u.username, u.fullname, u.password, u.email, u.userGroupId, u.isActive" +
                "\r\nFROM TUser u" +
                "\r\nLEFT JOIN TUserGroup ug ON u.userGroupId = ug.id" +
                "\r\nWHERE u.id = @UserId";


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
            catch (Exception ex)
            {
                response.Code = -1;
                response.Message = ex.Message;
                response.Exception = ex.ToString();
                response.Data = null;
            }
            return new JsonResult(response);
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
                    .Where(u => u.Username == user.Username && u.Password == password && u.IsActive == true)
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


                string query = "INSERT INTO TAudit (userId, action, ipAddress, macAddress, dateModified, dateCreated, isActive) VALUES (@UserId, 'Login', @IpAddress, @Ipv6, GETDATE(), GETDATE(), 1)";

                IPAddress ipv4 = null;
                IPAddress ipv6 = null;

                foreach (var address in ip.AddressList)
                {
                    if (address.AddressFamily == AddressFamily.InterNetwork)
                    {
                        ipv4 = address;
                    }
                    else if (address.AddressFamily == AddressFamily.InterNetworkV6)
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
                
                return Ok(new { message = "Login successful", UserId = idOfUser, Username = user.Username, Token = token, GroupId = groupId, Role = GetGroupNameList()[GetGroupIdList().IndexOf(groupId)] });

            }
            catch (Exception e)
            {
                return BadRequest(new { Code = -1, message = "Login failed" });
            }
        }

        

        [HttpPost]
        [Route("AddUser")]
        public JsonResult AddUser([FromBody] TuserModel newUser)
        {
            ResponseDto response = new ResponseDto();
            try
            {
                var ipAddress = HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault() ?? HttpContext.Connection.RemoteIpAddress.ToString();
                string host = Dns.GetHostName();
                IPHostEntry ip = Dns.GetHostEntry(host);
                IPAddress ipv4 = null;
                IPAddress ipv6 = null;

                foreach (var address in ip.AddressList)
                {
                    if (address.AddressFamily == AddressFamily.InterNetwork)
                    {
                        ipv4 = address;
                    }
                    else if (address.AddressFamily == AddressFamily.InterNetworkV6)
                    {
                        ipv6 = address;
                    }
                }

                string insertQuery = "INSERT INTO TUser (username, fullname, email, password, userGroupId, lastLogin, isActive, dateCreated, dateModified) " +
                                "VALUES (@Username, @Fullname, @Email, @Password, @UserGroupId, GETDATE(), @IsActive, GETDATE(), GETDATE());";
                string query2 = "INSERT INTO TAudit (userId, ipAddress, macAddress, action, tableName, dateModified, dateCreated, isActive) " +
                           "VALUES (@UserId, @IpAddress, @Ipv6, 'Add', 'TUser', GETDATE(), GETDATE(), 1)";

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
                    new SqlParameter("@IpAddress", ipv4?.ToString()),
                    new SqlParameter("@Ipv6", ipv6?.ToString()),
                    new SqlParameter("@UserId", (object)newUser.UserId ?? DBNull.Value),
                };


                _exQuery.ExecuteRawQuery(insertQuery, parameters);
                _exQuery.ExecuteRawQuery(query2, parameters2);



                return new JsonResult(new { Message = "User added successfully" });
            }
            catch (Exception ex)
            {
                response.Code = -1;
                response.Message = ex.Message;
                response.Exception = ex.ToString();
                response.Data = null;
            }
            return new JsonResult(response);
        }

        private List<int> GetGroupIdList()
        {
            string query = "select id from TUserGroup";
            DataTable table = _exQuery.ExecuteRawQuery(query);

            List<int> groupIdList = new List<int>();

            foreach (DataRow row in table.Rows)
            {
                if (row["id"] != null && row["id"] != DBNull.Value)
                {
                    int groupId = Convert.ToInt32(row["id"]);
                    groupIdList.Add(groupId);
                }
            }

            return groupIdList;
        }

        private List<string> GetGroupNameList()
        {
            string query = "select groupName from TUserGroup";
            DataTable table = _exQuery.ExecuteRawQuery(query);

            List<string> groupNameList = new List<string>();

            foreach (DataRow row in table.Rows)
            {
                if (row["groupName"] != null && row["groupName"] != DBNull.Value)
                {
                    string groupName = row["groupName"].ToString();
                    groupNameList.Add(groupName);
                }
            }

            return groupNameList;
        }


        private string CreateToken(Tuser user, int groupId)
        {
            

            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
            };

            for (int i = 0; i < GetGroupIdList().Count; i++)
            {
                if (groupId == GetGroupIdList()[i])
                {
                    claims.Add(new Claim(ClaimTypes.Role, GetGroupNameList()[i]));
                }
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
        public JsonResult UpdateUser(int id, [FromBody] TuserModel updatedUser)
        {
            ResponseDto response = new ResponseDto();
            try
            {
                var ipAddress = HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault() ?? HttpContext.Connection.RemoteIpAddress.ToString();
                string host = Dns.GetHostName();
                IPHostEntry ip = Dns.GetHostEntry(host);
                IPAddress ipv4 = null;
                IPAddress ipv6 = null;

                foreach (var address in ip.AddressList)
                {
                    if (address.AddressFamily == AddressFamily.InterNetwork)
                    {
                        ipv4 = address;
                    }
                    else if (address.AddressFamily == AddressFamily.InterNetworkV6)
                    {
                        ipv6 = address;
                    }
                }

                string getPasswordQuery = "SELECT password FROM TUser WHERE id = @Id;";
                SqlParameter[] getPasswordParams = { new SqlParameter("@Id", id) };
                string currentPassword = _exQuery.ExecuteScalar<string>(getPasswordQuery, getPasswordParams);

                if (currentPassword != updatedUser.Password)
                {
                    string updateQuery = "UPDATE TUser SET username = @Username, fullname = @Fullname, email = @Email, " +
                                         "password = @Password, userGroupId = @UserGroupId, isActive = @IsActive, " +
                                         "dateModified = GETDATE() WHERE id = @Id;";
                    string query2 = "INSERT INTO TAudit (userId, ipAddress, macAddress, action, tableName, dateModified, dateCreated, isActive) " +
                           "VALUES (@UserId, @IpAddress, @Ipv6, 'Update', 'TUser', GETDATE(), GETDATE(), 1)";

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
                        new SqlParameter("@IpAddress", ipv4?.ToString()),
                        new SqlParameter("@Ipv6", ipv6?.ToString()),
                        new SqlParameter("@UserId", (object)updatedUser.UserId ?? DBNull.Value),
                    };

                    _exQuery.ExecuteRawQuery(updateQuery, parameters);
                    _exQuery.ExecuteRawQuery(query2, parameters2);

                    

                    return new JsonResult("User updated successfully");
                }
                else
                {
                    string updateQuery = "UPDATE TUser SET username = @Username, fullname = @Fullname, email = @Email, " +
                                         "userGroupId = @UserGroupId, isActive = @IsActive, " +
                                         "dateModified = GETDATE() WHERE id = @Id;";
                    string query2 = "INSERT INTO TAudit (userId, ipAddress, macAddress, action, tableName, dateModified, dateCreated, isActive) " +
                           "VALUES (@UserId, @IpAddress, @Ipv6, 'Update', 'TUser', GETDATE(), GETDATE(), 1)";

                    SqlParameter[] parameters =
                    {
                        new SqlParameter("@Id", id),
                        new SqlParameter("@Username", updatedUser.Username),
                        new SqlParameter("@Fullname", updatedUser.Fullname),
                        new SqlParameter("@Email", updatedUser.Email),
                        
                        new SqlParameter("@UserGroupId", updatedUser.UserGroupId),
                        new SqlParameter("@IsActive", updatedUser.IsActive),
                    };
                    SqlParameter[] parameters2 =
                    {
                        new SqlParameter("@IpAddress", ipv4?.ToString()),
                        new SqlParameter("@Ipv6", ipv6?.ToString()),
                        new SqlParameter("@UserId", (object)updatedUser.UserId ?? DBNull.Value),
                    };

                    _exQuery.ExecuteRawQuery(updateQuery, parameters);
                    _exQuery.ExecuteRawQuery(query2, parameters2);

                    

                    return new JsonResult("User updated successfully");
                }
            }
            catch (Exception ex)
            {
                response.Code = -1;
                response.Message = ex.Message;
                response.Exception = ex.ToString();
                response.Data = null;
                
            }
            return new JsonResult(response);
        }


        [HttpDelete]
        [Route("DeleteUsers")]
        public JsonResult DeleteUsers([FromBody] List<int> userIds)
        {
            ResponseDto response = new ResponseDto();
            try
            {

                var ipAddress = HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault() ?? HttpContext.Connection.RemoteIpAddress.ToString();
                string host = Dns.GetHostName();
                IPHostEntry ip = Dns.GetHostEntry(host);
                IPAddress ipv4 = null;
                IPAddress ipv6 = null;

                foreach (var address in ip.AddressList)
                {
                    if (address.AddressFamily == AddressFamily.InterNetwork)
                    {
                        ipv4 = address;
                    }
                    else if (address.AddressFamily == AddressFamily.InterNetworkV6)
                    {
                        ipv6 = address;
                    }
                }


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

                return new JsonResult("Users deleted successfully");
            }
            catch (Exception ex)
            {
                response.Code = -1;
                response.Message = ex.Message;
                response.Exception = ex.ToString();
                response.Data = null;
            }
            return new JsonResult(response);
            
        }

        [HttpGet]
        [Route("SearchUsers")]
        public JsonResult SearchUsers(string searchQuery)
        {
            ResponseDto response = new ResponseDto();
            try
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
            catch (Exception ex)
            {
                response.Code = -1;
                response.Message = ex.Message;
                response.Exception = ex.ToString();
                response.Data = null;
            }
            return new JsonResult(response);
            
        }



    }
}
