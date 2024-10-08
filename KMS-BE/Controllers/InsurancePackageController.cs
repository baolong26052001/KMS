﻿using Microsoft.AspNetCore.Mvc;
using KMS.Models;
using KMS.Tools;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace KMS.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class InsurancePackageController : ControllerBase
    {
        private readonly KioskManagementSystemContext _dbcontext;
        private IConfiguration _configuration;
        private readonly ExecuteQuery _exQuery;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public InsurancePackageController(IConfiguration configuration, KioskManagementSystemContext _context, ExecuteQuery exQuery, IHttpContextAccessor httpContextAccessor)
        {
            _dbcontext = _context;
            _configuration = configuration;
            _exQuery = exQuery;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        [Route("ShowBenefitById/{id}")] // (KMS) hiện thông tin khi ở màn hình edit benefit
        public JsonResult GetBenefitById(int id)
        {
            ResponseDto response = new ResponseDto();
            try
            {
                string query = "select * " +
                "from Benefit " +
                "where id=@id";

                SqlParameter parameter = new SqlParameter("@id", id);
                DataTable table = _exQuery.ExecuteRawQuery(query, new[] { parameter });

                if (table.Rows.Count > 0)
                {
                    return new JsonResult(table);
                }
                else
                {
                    return new JsonResult(new
                    {
                        Code = 404,
                        Message = "Benefit not found"
                    });
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
        [Route("ShowBenefitDetailById/{id}")] // (KMS) hiện thông tin khi ở màn hình edit benefit detail
        public JsonResult GetBenefitDetailById(int id)
        {
            ResponseDto response = new ResponseDto();
            try
            {
                string query = "select * " +
                "from BenefitDetail " +
                "where id=@id";

                SqlParameter parameter = new SqlParameter("@id", id);
                DataTable table = _exQuery.ExecuteRawQuery(query, new[] { parameter });

                if (table.Rows.Count > 0)
                {
                    return new JsonResult(table);
                }
                else
                {
                    return new JsonResult(new
                    {
                        Code = 404,
                        Message = "Benefit not found"
                    });
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
        [Route("ShowInsurancePackageDetail/{id}")] // (KMS) khi ấn vào view package header A, thì sẽ show ra "benefit" của package header A
                                                   // (Kiosk App) hiện ra benefit của package header A
        public JsonResult GetInsurancePackageDetail(int id)
        {
            ResponseDto response = new ResponseDto();
            try
            {
                string query = "select b.id, b.content, b.coverage, b.description, ipack.packageName, itype.typeName, " +
                "b.dateModified, b.dateCreated " +
                "from Benefit b " +
                "LEFT JOIN InsurancePackageHeader ipack ON b.packageId = ipack.id " +
                "LEFT JOIN InsuranceType itype ON itype.id = ipack.insuranceTypeId " +
                "where ipack.id=@id";

                SqlParameter parameter = new SqlParameter("@id", id);
                DataTable table = _exQuery.ExecuteRawQuery(query, new[] { parameter });

                if (table.Rows.Count > 0)
                {
                    return new JsonResult(table);
                }
                else
                {
                    return new JsonResult(new
                    {
                        Code = 404,
                        Message = "Not found"
                    });
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
        [Route("ShowBenefit/{id}")] // ấn vào view benefit, sẽ ra benefit detail
        public JsonResult GetBenefitDetail(int id)
        {
            ResponseDto response = new ResponseDto();
            try
            {
                string query = "select id,content,coverage,benefitId,dateCreated,dateModified\r\nfrom BenefitDetail " +
                "where benefitId=@id";

                SqlParameter parameter = new SqlParameter("@id", id);
                DataTable table = _exQuery.ExecuteRawQuery(query, new[] { parameter });

                if (table.Rows.Count > 0)
                {
                    return new JsonResult(table);
                }
                else
                {
                    return new JsonResult(new
                    {
                        Code = 404,
                        Message = "Benefit not found"
                    });
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
        [Route("ShowBenefitForKioskApp/{packageId}")]
        public JsonResult ShowBenefitForKioskApp(int packageId)
        {
            ResponseDto response = new ResponseDto();
            try
            {
                string query = @"
            select 
                a.id as idHeader,
                a.content as contentHeader,
                a.coverage as coverageHeader,
                a.description as descriptionHeader,
                a.packageId,
                b.id as idDetail,
                b.content as contentDetail,
                b.coverage as coverageDetail,
                b.benefitId
            from Benefit a
            left join BenefitDetail b on b.benefitId = a.id
            where a.packageId = @packageId";

                SqlParameter parameter = new SqlParameter("@packageId", packageId);
                DataTable table = _exQuery.ExecuteRawQuery(query, new[] { parameter });

                if (table.Rows.Count > 0)
                {
                    var result = new List<object>();
                    var headers = new Dictionary<int, dynamic>();

                    foreach (DataRow row in table.Rows)
                    {
                        int idHeader = Convert.ToInt32(row["idHeader"]);
                        if (!headers.ContainsKey(idHeader))
                        {
                            headers[idHeader] = new
                            {
                                idHeader = idHeader,
                                contentHeader = row["contentHeader"].ToString(),
                                coverageHeader = row["coverageHeader"].ToString(),
                                descriptionHeader = row["descriptionHeader"].ToString(),
                                packageId = Convert.ToInt32(row["packageId"]),
                                details = new List<object>()
                            };
                        }

                        if (row["idDetail"] != DBNull.Value)
                        {
                            headers[idHeader].details.Add(new
                            {
                                idDetail = Convert.ToInt32(row["idDetail"]),
                                contentDetail = row["contentDetail"].ToString(),
                                coverageDetail = row["coverageDetail"].ToString(),
                                benefitId = Convert.ToInt32(row["benefitId"])
                            });
                        }
                    }

                    result.AddRange(headers.Values);
                    return new JsonResult(result);
                }
                else
                {
                    return new JsonResult(new
                    {
                        Code = 404,
                        Message = "Benefit not found"
                    });
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
        [Route("AddBenefit")]
        public JsonResult AddBenefit([FromBody] BenefitModel benefit)
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

                string query = "INSERT INTO Benefit (packageId, content, coverage, description, dateModified, dateCreated) " +
                           "VALUES (@PackageId, @Content, @Coverage, @Description, GETDATE(), GETDATE())";
                string query2 = "INSERT INTO TAudit (userId, ipAddress, macAddress, action, tableName, dateModified, dateCreated, isActive) " +
                           "VALUES (@UserId, @IpAddress, @Ipv6, 'Add', 'Benefit', GETDATE(), GETDATE(), 1)";

                SqlParameter[] parameters =
                {
                    new SqlParameter("@PackageId", benefit.PackageId),
                    new SqlParameter("@Content", benefit.Content),
                    new SqlParameter("@Coverage", benefit.Coverage),
                    new SqlParameter("@Description", benefit.Description),


                };
                SqlParameter[] parameters2 = 
                {
                    new SqlParameter("@IpAddress", (object)ipv4?.ToString() ?? DBNull.Value),
                    new SqlParameter("@Ipv6", (object)ipv6?.ToString() ?? DBNull.Value),
                    new SqlParameter("@UserId", (object)benefit.UserId ?? DBNull.Value),
                };

                _exQuery.ExecuteRawQuery(query, parameters);
                _exQuery.ExecuteRawQuery(query2, parameters2);

                return new JsonResult("Benefit added successfully");
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
        [Route("AddBenefitDetail")]
        public JsonResult AddBenefitDetail([FromBody] BenefitDetailModel benefitDetail)
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

                string query = "INSERT INTO BenefitDetail (benefitId, content, coverage, dateModified, dateCreated) " +
                           "VALUES (@BenefitId, @Content, @Coverage, GETDATE(), GETDATE())";
                string query2 = "INSERT INTO TAudit (userId, ipAddress, macAddress, action, tableName, dateModified, dateCreated, isActive) " +
                           "VALUES (@UserId, @IpAddress, @Ipv6, 'Add', 'BenefitDetail', GETDATE(), GETDATE(), 1)";
                SqlParameter[] parameters =
                {

                    new SqlParameter("@BenefitId", benefitDetail.BenefitId),
                    new SqlParameter("@Content", benefitDetail.Content),
                    new SqlParameter("@Coverage", benefitDetail.Coverage),

                };
                SqlParameter[] parameters2 =
                {
                    new SqlParameter("@IpAddress", (object)ipv4?.ToString() ?? DBNull.Value),
                    new SqlParameter("@Ipv6", (object)ipv6?.ToString() ?? DBNull.Value),
                    new SqlParameter("@UserId", (object)benefitDetail.UserId ?? DBNull.Value),
                };

                _exQuery.ExecuteRawQuery(query, parameters);
                _exQuery.ExecuteRawQuery(query2, parameters2);

                return new JsonResult("Benefit detail added successfully");
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


        
        private int ShowValueCookie()
        {
            if (HttpContext.Request.Cookies.ContainsKey("userId"))
            {
                var userIdCookieValue = HttpContext.Request.Cookies["userId"];
                int number = Int32.Parse(userIdCookieValue);

                if (!string.IsNullOrEmpty(userIdCookieValue))
                {
                    return number;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return -1;
            }
        }

        [HttpPut]
        [Route("EditBenefit/{id}")]
        public JsonResult EditBenefit(int id, [FromBody] BenefitModel benefit)
        {
            ResponseDto response = new ResponseDto();
            try
            {

                //int userId = ShowValueCookie();
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
                


                string query = "UPDATE Benefit " +
                           "SET content = @Content, coverage = @Coverage, description = @Description, " +
                           "dateModified = GETDATE() " +
                           "WHERE id = @id";
                string query2 = "INSERT INTO TAudit (ipAddress, macAddress, userId, action, tableName, dateModified, dateCreated, isActive) " +
                           "VALUES (@IpAddress, @Ipv6, @UserId, 'Update', 'Benefit', GETDATE(), GETDATE(), 1)";

                SqlParameter[] parameters =
                {
                    new SqlParameter("@id", id),
                    new SqlParameter("@Content", benefit.Content),
                    new SqlParameter("@Coverage", benefit.Coverage),
                    new SqlParameter("@Description", benefit.Description),


                };
                SqlParameter[] parameters2 =
                {
                    new SqlParameter("@IpAddress", (object)ipv4?.ToString() ?? DBNull.Value),
                    new SqlParameter("@Ipv6", (object)ipv6?.ToString() ?? DBNull.Value),
                    new SqlParameter("@UserId", (object)benefit.UserId ?? DBNull.Value),
                };

                _exQuery.ExecuteRawQuery(query, parameters);
                _exQuery.ExecuteRawQuery(query2, parameters2);

                return new JsonResult("Benefit updated successfully");
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
        [Route("EditBenefitDetail/{id}")]
        public JsonResult EditBenefitDetail(int id, [FromBody] BenefitDetailModel benefitDetail)
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

                string query = "UPDATE BenefitDetail " +
                           "SET content = @Content, coverage = @Coverage, " +
                           "dateModified = GETDATE() " +
                           "WHERE id = @id";
                string query2 = "INSERT INTO TAudit (ipAddress, macAddress, userId, action, tableName, dateModified, dateCreated, isActive) " +
                    "VALUES (@IpAddress, @Ipv6, @UserId, 'Update', 'BenefitDetail', GETDATE(), GETDATE(), 1)";

                SqlParameter[] parameters =
                {
                    new SqlParameter("@id", id),
                    new SqlParameter("@Content", benefitDetail.Content),
                    new SqlParameter("@Coverage", benefitDetail.Coverage),
                };
                SqlParameter[] parameters2 =
                {
                    new SqlParameter("@IpAddress", (object)ipv4?.ToString() ?? DBNull.Value),
                    new SqlParameter("@Ipv6", (object)ipv6?.ToString() ?? DBNull.Value),
                    new SqlParameter("@UserId", (object)benefitDetail.UserId ?? DBNull.Value),
                };

                _exQuery.ExecuteRawQuery(query, parameters);
                _exQuery.ExecuteRawQuery(query2, parameters2);

                return new JsonResult("Benefit detail updated successfully");
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
        [Route("DeleteBenefit")]
        public JsonResult DeleteBenefit([FromBody] List<int> benefitIds)
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


                if (benefitIds == null || benefitIds.Count == 0)
                {
                    return new JsonResult("No benefit IDs provided for deletion");
                }

                StringBuilder deleteQuery = new StringBuilder("DELETE FROM Benefit WHERE id IN (");
                string query2 = "INSERT INTO TAudit (action, tableName, dateModified, dateCreated, isActive) VALUES ('Delete', 'Benefit', GETDATE(), GETDATE(), 1)";

                List<SqlParameter> parameters = new List<SqlParameter>();
                SqlParameter[] parameters2 = { };
                for (int i = 0; i < benefitIds.Count; i++)
                {
                    string parameterName = "@BenefitId" + i;
                    deleteQuery.Append(parameterName);

                    if (i < benefitIds.Count - 1)
                    {
                        deleteQuery.Append(", ");
                    }

                    parameters.Add(new SqlParameter(parameterName, benefitIds[i]));
                }

                deleteQuery.Append(");");

                _exQuery.ExecuteRawQuery(deleteQuery.ToString(), parameters.ToArray());

                return new JsonResult("Benefit deleted successfully");
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
        [Route("DeleteBenefitDetail")]
        public JsonResult DeleteBenefitDetail([FromBody] List<int> benefitDetailIds)
        {
            ResponseDto response = new ResponseDto();
            try
            {
                if (benefitDetailIds == null || benefitDetailIds.Count == 0)
                {
                    return new JsonResult("No benefit detail IDs provided for deletion");
                }

                StringBuilder deleteQuery = new StringBuilder("DELETE FROM BenefitDetail WHERE id IN (");
                string query2 = "INSERT INTO TAudit (action, tableName, dateModified, dateCreated, isActive) VALUES ('Delete', 'BenefitDetail', GETDATE(), GETDATE(), 1)";


                List<SqlParameter> parameters = new List<SqlParameter>();
                SqlParameter[] parameters2 = { };
                for (int i = 0; i < benefitDetailIds.Count; i++)
                {
                    string parameterName = "@BenefitDetailId" + i;
                    deleteQuery.Append(parameterName);

                    if (i < benefitDetailIds.Count - 1)
                    {
                        deleteQuery.Append(", ");
                    }

                    parameters.Add(new SqlParameter(parameterName, benefitDetailIds[i]));
                }

                deleteQuery.Append(");");

                _exQuery.ExecuteRawQuery(deleteQuery.ToString(), parameters.ToArray());
                _exQuery.ExecuteRawQuery(query2, parameters2);
                return new JsonResult("Benefit detail deleted successfully");
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
