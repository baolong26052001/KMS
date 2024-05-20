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
    public class InsurancePackageDetailController : ControllerBase
    {
        private readonly KioskManagementSystemContext _dbcontext;
        private IConfiguration _configuration;
        private readonly ExecuteQuery _exQuery;

        public InsurancePackageDetailController(IConfiguration configuration, KioskManagementSystemContext _context, ExecuteQuery exQuery)
        {
            _dbcontext = _context;
            _configuration = configuration;
            _exQuery = exQuery;
        }

        [HttpGet]
        [Route("ShowInsurancePackageDetail")] // coi giá tiền của gói bảo hiểm
        public JsonResult GetInsurancePackageDetail()
        {
            ResponseDto response = new ResponseDto();
            try
            {
                string query = "SELECT a.id, b.packageName, a.ageRangeId, N'Từ ' + CAST(d.startAge AS NVARCHAR) + N' đến ' + CAST(d.endAge AS NVARCHAR) + N' tuổi' AS ageRange, a.fee, c.provider, c.email, e.content, a.dateModified, a.dateCreated " +
                "FROM InsurancePackageDetail a " +
                "JOIN InsurancePackageHeader b ON a.packageHeaderId = b.id " +
                "LEFT JOIN InsuranceProvider c ON c.id = b.insuranceProviderId " +
                "LEFT JOIN AgeRange d ON d.id = a.ageRangeId " +
                "LEFT JOIN Term e ON e.id = b.termId";

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
        [Route("ShowInsurancePackageDetailByAgeRangeId/{id}")] // coi giá tiền dựa theo độ tuổi
        public JsonResult GetInsurancePackageDetailByAgeRangeId(int id)
        {
            ResponseDto response = new ResponseDto();
            try
            {
                string query = "SELECT\r\n    a.id,\r\n\tb.id as headerId,\r\n\te.id as termId,\r\n    b.packageName,\r\n    a.ageRangeId,\r\n    N'Từ ' + CAST(d.startAge AS NVARCHAR) + N' đến ' + CAST(d.endAge AS NVARCHAR) + N' tuổi' AS ageRange,\r\n    a.fee,\r\n    c.provider,\r\n    c.email,\r\n    e.content,\r\n    a.dateModified,\r\n    a.dateCreated\r\nFROM\r\n    InsurancePackageDetail a\r\nJOIN\r\n    AgeRange h ON h.id = a.ageRangeId\r\nJOIN\r\n    InsurancePackageHeader b ON a.packageHeaderId = b.id\r\nLEFT JOIN\r\n    InsuranceProvider c ON c.id = b.insuranceProviderId\r\nLEFT JOIN\r\n    AgeRange d ON d.id = a.ageRangeId\r\nLEFT JOIN\r\n    Term e ON e.id = b.termId\r\nwhere d.id = @Id";

                SqlParameter parameter = new SqlParameter("@Id", id);
                DataTable table = _exQuery.ExecuteRawQuery(query, new[] { parameter });

                if (table.Rows.Count > 0)
                {
                    return new JsonResult(table);
                }
                else
                {
                    return new JsonResult("Age Range ID not found");
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
        [Route("ShowInsurancePackageDetailByAgeRangeIdAndTermId/{ageRangeId}/{termId}")] // coi giá tiền dựa theo độ tuổi và điều khoản
        public JsonResult GetInsurancePackageDetailByAgeRangeIdAndTermId(int ageRangeId, int termId)
        {
            ResponseDto response = new ResponseDto();
            try
            {
                string query = "SELECT\r\n    a.id,\r\n    b.id AS headerId,\r\n    e.id AS termId,\r\n    b.packageName,\r\n    a.ageRangeId,\r\n    N'Từ ' + CAST(d.startAge AS NVARCHAR) + N' đến ' + CAST(d.endAge AS NVARCHAR) + N' tuổi' AS ageRange,\r\n    a.fee,\r\n    c.provider,\r\n    c.email,\r\n    e.content,\r\n\tb.priority,\r\n    a.dateModified,\r\n    a.dateCreated\r\nFROM\r\n    InsurancePackageDetail a\r\nLEFT JOIN\r\n    AgeRange h ON h.id = a.ageRangeId\r\nLEFT JOIN\r\n    InsurancePackageHeader b ON a.packageHeaderId = b.id\r\nLEFT JOIN\r\n    InsuranceProvider c ON c.id = b.insuranceProviderId\r\nLEFT JOIN\r\n    AgeRange d ON d.id = a.ageRangeId\r\nLEFT JOIN\r\n    Term e ON e.id = b.termId\r\nWHERE\r\n    d.id = @AgeRangeId\r\n    AND e.id = @TermId\r\n\tORDER BY b.priority asc";

                SqlParameter[] parameters =
                {
                    new SqlParameter("@AgeRangeId", ageRangeId),
                    new SqlParameter("@TermId", termId)
                };

                DataTable table = _exQuery.ExecuteRawQuery(query, parameters);

                if (table.Rows.Count > 0)
                {
                    return new JsonResult(table);
                }
                else
                {
                    return new JsonResult("Age Range ID or term ID not found");
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
        [Route("ShowInsurancePackageDetailByTermId/{id}")] // coi giá tiền dựa theo điều khoản
        public JsonResult GetInsurancePackageDetailByTermId(int id)
        {
            ResponseDto response = new ResponseDto();
            try
            {
                string query = "SELECT\r\n    a.id,\r\n\tb.id as headerId,\r\n\te.id as termId,\r\n    b.packageName,\r\n    a.ageRangeId,\r\n    N'Từ ' + CAST(d.startAge AS NVARCHAR) + N' đến ' + CAST(d.endAge AS NVARCHAR) + N' tuổi' AS ageRange,\r\n    a.fee,\r\n    c.provider,\r\n    c.email,\r\n    e.content,\r\n    a.dateModified,\r\n    a.dateCreated\r\nFROM\r\n    InsurancePackageDetail a\r\nJOIN\r\n    AgeRange h ON h.id = a.ageRangeId\r\nJOIN\r\n    InsurancePackageHeader b ON a.packageHeaderId = b.id\r\nLEFT JOIN\r\n    InsuranceProvider c ON c.id = b.insuranceProviderId\r\nLEFT JOIN\r\n    AgeRange d ON d.id = a.ageRangeId\r\nLEFT JOIN\r\n    Term e ON e.id = b.termId\r\nwhere e.id = @Id";

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

        [HttpGet]
        [Route("ShowInsurancePackageDetailByHeaderId/{id}")] // coi giá tiền dựa theo id của package header
        public JsonResult GetInsurancePackageDetailByHeaderId(int id)
        {
            ResponseDto response = new ResponseDto();
            try
            {
                string query = "SELECT a.id, b.packageName, a.ageRangeId, N'Từ ' + CAST(d.startAge AS NVARCHAR) + N' đến ' + CAST(d.endAge AS NVARCHAR) + N' tuổi' AS ageRange, a.fee, c.provider, c.email, e.content, a.dateModified, a.dateCreated " +
                "FROM InsurancePackageDetail a " +
                "JOIN InsurancePackageHeader b ON a.packageHeaderId = b.id " +
                "LEFT JOIN InsuranceProvider c ON c.id = b.insuranceProviderId " +
                "LEFT JOIN AgeRange d ON d.id = a.ageRangeId " +
                "LEFT JOIN Term e ON e.id = b.termId where b.id=@Id";

                SqlParameter parameter = new SqlParameter("@Id", id);
                DataTable table = _exQuery.ExecuteRawQuery(query, new[] { parameter });

                if (table.Rows.Count > 0)
                {
                    return new JsonResult(table);
                }
                else
                {
                    return new JsonResult("Insurance Package Detail ID not found");
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
        [Route("ShowInsurancePackageDetail/{id}")] // (KMS) dùng để hiện dữ liệu khi edit insurance detail
        public JsonResult GetInsurancePackageDetailById(int id)
        {
            ResponseDto response = new ResponseDto();
            try
            {
                string query = "SELECT a.id, b.packageName, a.ageRangeId, N'Từ ' + CAST(d.startAge AS NVARCHAR) + N' đến ' + CAST(d.endAge AS NVARCHAR) + N' tuổi' AS ageRange, a.fee, c.provider, c.email, e.content, a.dateModified, a.dateCreated " +
                "FROM InsurancePackageDetail a " +
                "JOIN InsurancePackageHeader b ON a.packageHeaderId = b.id " +
                "LEFT JOIN InsuranceProvider c ON c.id = b.insuranceProviderId " +
                "LEFT JOIN AgeRange d ON d.id = a.ageRangeId " +
                "LEFT JOIN Term e ON e.id = b.termId where a.id=@Id";

                SqlParameter parameter = new SqlParameter("@Id", id);
                DataTable table = _exQuery.ExecuteRawQuery(query, new[] { parameter });

                if (table.Rows.Count > 0)
                {
                    return new JsonResult(table);
                }
                else
                {
                    return new JsonResult("Insurance Package Detail ID not found");
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
        [Route("AddInsurancePackageDetail")]
        public JsonResult AddInsurancePackageDetail([FromBody] InsurancePackageDetailModel insurancePackageDetail)
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

                string query = "INSERT INTO InsurancePackageDetail (packageHeaderId, ageRangeId, fee, dateCreated, dateModified) " +
                           "VALUES (@PackageHeaderId, @AgeRangeId, @Fee, GETDATE(), GETDATE())";
                string query2 = "INSERT INTO TAudit (userId, ipAddress, macAddress, action, tableName, dateModified, dateCreated, isActive) " +
                           "VALUES (@UserId, @IpAddress, @Ipv6, 'Add', 'InsurancePackageDetail', GETDATE(), GETDATE(), 1)";

                SqlParameter[] parameters =
                {
                    new SqlParameter("@PackageHeaderId", insurancePackageDetail.PackageHeaderId),
                    new SqlParameter("@AgeRangeId", (object)insurancePackageDetail.AgeRangeId ?? DBNull.Value),
                    new SqlParameter("@Fee", insurancePackageDetail.Fee),
                };
                SqlParameter[] parameters2 =
                {
                    new SqlParameter("@IpAddress", (object)ipv4?.ToString() ?? DBNull.Value),
                    new SqlParameter("@Ipv6", (object)ipv6?.ToString() ?? DBNull.Value),
                    new SqlParameter("@UserId", (object)insurancePackageDetail.UserId ?? DBNull.Value),
                };

                _exQuery.ExecuteRawQuery(query, parameters);
                _exQuery.ExecuteRawQuery(query2, parameters2);

                return new JsonResult("Insurance Package Detail added successfully");
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
        [Route("EditInsurancePackageDetail/{id}")]
        public JsonResult EditInsurancePackageDetail(int id, [FromBody] InsurancePackageDetailModel insurancePackageDetail)
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

                string query = "UPDATE InsurancePackageDetail " +
                           "SET packageHeaderId = @PackageHeaderId, ageRangeId = @AgeRangeId, fee = @Fee, dateModified = GETDATE() " +
                           "WHERE id = @Id";
                string query2 = "INSERT INTO TAudit (userId, ipAddress, macAddress, action, tableName, dateModified, dateCreated, isActive) " +
                           "VALUES (@UserId, @IpAddress, @Ipv6, 'Update', 'InsurancePackageDetail', GETDATE(), GETDATE(), 1)";

                SqlParameter[] parameters =
                {
                    new SqlParameter("@Id", id),
                    new SqlParameter("@PackageHeaderId", insurancePackageDetail.PackageHeaderId),
                    new SqlParameter("@AgeRangeId", (object)insurancePackageDetail.AgeRangeId ?? DBNull.Value),
                    new SqlParameter("@Fee", insurancePackageDetail.Fee),
                };
                SqlParameter[] parameters2 =
                {
                    new SqlParameter("@IpAddress", (object)ipv4?.ToString() ?? DBNull.Value),
                    new SqlParameter("@Ipv6", (object)ipv6?.ToString() ?? DBNull.Value),
                    new SqlParameter("@UserId", (object)insurancePackageDetail.UserId ?? DBNull.Value),
                };


                _exQuery.ExecuteRawQuery(query, parameters);
                _exQuery.ExecuteRawQuery(query2, parameters2);

                return new JsonResult("Insurance Package Detail updated successfully");
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
        [Route("DeleteInsurancePackageDetail")]
        public JsonResult DeleteInsurancePackageDetail([FromBody] List<int> insurancePackageDetailIds)
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


                if (insurancePackageDetailIds == null || insurancePackageDetailIds.Count == 0)
                {
                    return new JsonResult("No insurance package detail IDs provided for deletion");
                }

                StringBuilder deleteQuery = new StringBuilder("DELETE FROM InsurancePackageDetail WHERE id IN (");


                List<SqlParameter> parameters = new List<SqlParameter>();

                for (int i = 0; i < insurancePackageDetailIds.Count; i++)
                {
                    string parameterName = "@InsurancePackageDetailId" + i;
                    deleteQuery.Append(parameterName);

                    if (i < insurancePackageDetailIds.Count - 1)
                    {
                        deleteQuery.Append(", ");
                    }

                    parameters.Add(new SqlParameter(parameterName, insurancePackageDetailIds[i]));
                }

                deleteQuery.Append(");");

                _exQuery.ExecuteRawQuery(deleteQuery.ToString(), parameters.ToArray());


                return new JsonResult("Insurance Package Detail deleted successfully");
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
        [Route("SearchInsurancePackageDetail")]
        public JsonResult SearchInsurancePackageDetail(string searchQuery)
        {
            ResponseDto response = new ResponseDto();
            try
            {
                string query = "SELECT a.id, b.packageName, N'Từ ' + CAST(d.startAge AS NVARCHAR) + N' đến ' + CAST(d.endAge AS NVARCHAR) + N' tuổi' AS ageRange, a.fee, c.provider, c.email, e.content, a.dateModified, a.dateCreated " +
                           "FROM InsurancePackageDetail a " +
                           "LEFT JOIN InsurancePackageHeader b ON a.packageHeaderId = b.id " +
                           "LEFT JOIN InsuranceProvider c ON c.id = b.insuranceProviderId " +
                           "LEFT JOIN AgeRange d ON d.id = a.ageRangeId " +
                           "LEFT JOIN Term e ON e.id = b.termId " +
                           "WHERE a.id LIKE @searchQuery OR " +
                           "b.packageName LIKE @searchQuery OR " +
                           "c.provider LIKE @searchQuery OR " +
                           "CONVERT(VARCHAR(20), a.fee) LIKE @searchQuery OR " +
                           "c.email LIKE @searchQuery OR " +
                           "e.content LIKE @searchQuery";

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

        [HttpGet]
        [Route("FilterInsurancePackageDetail")]
        public JsonResult FilterInsurancePackageDetail([FromQuery] DateTime? startDate = null, [FromQuery] DateTime? endDate = null)
        {
            ResponseDto response = new ResponseDto();
            try
            {
                string query = "SELECT a.id, b.packageName, N'Từ ' + CAST(d.startAge AS NVARCHAR) + N' đến ' + CAST(d.endAge AS NVARCHAR) + N' tuổi' AS ageRange, a.fee, c.provider, c.email, e.content, a.dateModified, a.dateCreated " +
                           "FROM InsurancePackageDetail a " +
                           "LEFT JOIN InsurancePackageHeader b ON a.packageHeaderId = b.id " +
                           "LEFT JOIN InsuranceProvider c ON c.id = b.insuranceProviderId " +
                           "LEFT JOIN AgeRange d ON d.id = a.ageRangeId " +
                           "LEFT JOIN Term e ON e.id = b.termId ";

                List<SqlParameter> parameters = new List<SqlParameter>();

                if (startDate.HasValue && endDate.HasValue)
                {

                    startDate = startDate.Value.Date.AddHours(0).AddMinutes(0).AddSeconds(0);
                    endDate = endDate.Value.Date.AddHours(23).AddMinutes(59).AddSeconds(59);

                    query += (parameters.Count == 0 ? " WHERE " : " AND ") + "a.dateCreated >= @startDate AND a.dateCreated <= @endDate";
                    parameters.Add(new SqlParameter("@startDate", startDate.Value));
                    parameters.Add(new SqlParameter("@endDate", endDate.Value));
                }

                DataTable table = _exQuery.ExecuteRawQuery(query, parameters.ToArray());

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
