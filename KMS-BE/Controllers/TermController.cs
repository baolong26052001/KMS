using Microsoft.AspNetCore.Mvc;
using KMS.Models;
using KMS.Tools;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Text;
using System.Net.Sockets;
using System.Net;

namespace KMS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TermController : ControllerBase
    {
        private readonly KioskManagementSystemContext _dbcontext;
        private IConfiguration _configuration;
        private readonly ExecuteQuery _exQuery;

        public TermController(IConfiguration configuration, KioskManagementSystemContext _context, ExecuteQuery exQuery)
        {
            _dbcontext = _context;
            _configuration = configuration;
            _exQuery = exQuery;
        }

        [HttpGet]
        [Route("ShowTerm")]
        public JsonResult GetTerm()
        {
            ResponseDto response = new ResponseDto();
            try
            {
                string query = "SELECT * FROM Term";

                DataTable table = _exQuery.ExecuteRawQuery(query);
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
        [Route("ShowTerm/{id}")]
        public JsonResult GetTermById(int id)
        {
            ResponseDto response = new ResponseDto();
            try
            {
                string query = "SELECT * FROM Term where id=@Id";

                SqlParameter parameter = new SqlParameter("@Id", id);
                DataTable table = _exQuery.ExecuteRawQuery(query, new[] { parameter });

                if (table.Rows.Count > 0)
                {
                    return new JsonResult(table);
                }
                else
                {
                    return new JsonResult("Term ID not found");
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
        [Route("AddTerm")]
        public JsonResult AddTerm([FromBody] TermModel term)
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

                string query = "INSERT INTO Term (content, dateCreated, dateModified) " +
                           "VALUES (@Content, GETDATE(), GETDATE())";
                string query2 = "INSERT INTO TAudit (userId, ipAddress, macAddress, action, tableName, dateModified, dateCreated, isActive) " +
                           "VALUES (@UserId, @IpAddress, @Ipv6, 'Add', 'Term', GETDATE(), GETDATE(), 1)";

                SqlParameter[] parameters =
                {
                    new SqlParameter("@Content", term.Content),
                };
                SqlParameter[] parameters2 =
                {
                    new SqlParameter("@IpAddress", ipv4?.ToString()),
                    new SqlParameter("@Ipv6", ipv6?.ToString()),
                    new SqlParameter("@UserId", (object)term.UserId ?? DBNull.Value),
                };

                _exQuery.ExecuteRawQuery(query, parameters);
                _exQuery.ExecuteRawQuery(query2, parameters2);

                return new JsonResult("Term added successfully");
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

        [HttpPut]
        [Route("EditTerm/{id}")]
        public JsonResult EditTerm(int id, [FromBody] TermModel term)
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

                string query = "UPDATE Term " +
                           "SET content = @Content, dateModified = GETDATE() " +
                           "WHERE id = @Id";
                string query2 = "INSERT INTO TAudit (userId, ipAddress, macAddress, action, tableName, dateModified, dateCreated, isActive) " +
                           "VALUES (@UserId, @IpAddress, @Ipv6, 'Update', 'Term', GETDATE(), GETDATE(), 1)";

                SqlParameter[] parameters =
                {
                    new SqlParameter("@Id", id),
                    new SqlParameter("@Content", term.Content),

                };
                SqlParameter[] parameters2 =
                {
                    new SqlParameter("@IpAddress", ipv4?.ToString()),
                    new SqlParameter("@Ipv6", ipv6?.ToString()),
                    new SqlParameter("@UserId", (object)term.UserId ?? DBNull.Value),
                };

                _exQuery.ExecuteRawQuery(query, parameters);
                _exQuery.ExecuteRawQuery(query2, parameters2);

                return new JsonResult("Term updated successfully");
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
        [Route("DeleteTerm")]
        public JsonResult DeleteTerm([FromBody] List<int> termIds)
        {
            ResponseDto response = new ResponseDto();
            try
            {
                if (termIds == null || termIds.Count == 0)
                {
                    return new JsonResult("No term IDs provided for deletion");
                }

                StringBuilder deleteQuery = new StringBuilder("DELETE FROM Term WHERE id IN (");
                string query2 = "INSERT INTO TAudit (action, tableName, dateModified, dateCreated, isActive) VALUES ('Delete', 'Term', GETDATE(), GETDATE(), 1)";

                List<SqlParameter> parameters = new List<SqlParameter>();
                SqlParameter[] parameters2 = { };
                for (int i = 0; i < termIds.Count; i++)
                {
                    string parameterName = "@TermId" + i;
                    deleteQuery.Append(parameterName);

                    if (i < termIds.Count - 1)
                    {
                        deleteQuery.Append(", ");
                    }

                    parameters.Add(new SqlParameter(parameterName, termIds[i]));
                }

                deleteQuery.Append(");");

                _exQuery.ExecuteRawQuery(deleteQuery.ToString(), parameters.ToArray());
                _exQuery.ExecuteRawQuery(query2, parameters2);

                return new JsonResult("Term deleted successfully");
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
