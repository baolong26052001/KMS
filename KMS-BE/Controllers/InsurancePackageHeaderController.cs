using Microsoft.AspNetCore.Mvc;
using KMS.Models;
using KMS.Tools;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Text;

namespace KMS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InsurancePackageHeaderController : ControllerBase
    {
        private readonly KioskManagementSystemContext _dbcontext;
        private IConfiguration _configuration;
        private readonly ExecuteQuery _exQuery;

        public InsurancePackageHeaderController(IConfiguration configuration, KioskManagementSystemContext _context, ExecuteQuery exQuery)
        {
            _dbcontext = _context;
            _configuration = configuration;
            _exQuery = exQuery;
        }

        [HttpGet]
        [Route("ShowInsurancePackageHeader")]
        public JsonResult GetInsurancePackageHeader()
        {
            ResponseDto response = new ResponseDto();
            try
            {
                string query = "SELECT b.id, b.packageName, c.provider, c.email, e.content, f.typeName, b.priority, b.isActive, b.dateModified, b.dateCreated " +
                "FROM InsurancePackageHeader b " +
                "LEFT JOIN InsuranceProvider c ON c.id = b.insuranceProviderId " +
                "LEFT JOIN Term e ON e.id = b.termId " +
                "LEFT JOIN InsuranceType f ON b.insuranceTypeId = f.id";


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
        [Route("ShowInsurancePackageHeader/{id}")]
        public JsonResult GetInsurancePackageHeaderById(int id)
        {
            ResponseDto response = new ResponseDto();
            try
            {
                string query = "SELECT b.id, f.id AS insuranceTypeId, e.id AS termId, c.id AS insuranceProviderId, b.packageName, c.provider, c.email, e.content, f.typeName, b.priority, b.isActive, b.dateModified, b.dateCreated " +
                "FROM InsurancePackageHeader b " +
                "LEFT JOIN InsuranceProvider c ON c.id = b.insuranceProviderId " +
                "LEFT JOIN Term e ON e.id = b.termId " +
                "LEFT JOIN InsuranceType f ON b.insuranceTypeId = f.id " +
                "WHERE b.id = @Id";



                SqlParameter parameter = new SqlParameter("@Id", id);
                DataTable table = _exQuery.ExecuteRawQuery(query, new[] { parameter });

                if (table.Rows.Count > 0)
                {
                    return new JsonResult(table);
                }
                else
                {
                    return new JsonResult("Insurance Package Header ID not found");
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
        [Route("ShowInsurancePackageHeaderByInsuranceType/{insuranceType}")]
        public JsonResult GetInsurancePackageHeaderByInsuranceType(int insuranceType)
        {
            ResponseDto response = new ResponseDto();
            try
            {
                string query = "SELECT b.id, b.packageName, c.provider, c.email, e.content, f.typeName, b.priority, b.isActive, b.dateModified, b.dateCreated " +
                "FROM InsurancePackageHeader b " +
                "LEFT JOIN InsuranceProvider c ON c.id = b.insuranceProviderId " +
                "LEFT JOIN Term e ON e.id = b.termId " +
                "LEFT JOIN InsuranceType f ON b.insuranceTypeId = f.id " +

                "WHERE f.id = @InsuranceTypeId ORDER BY b.priority asc";


                SqlParameter parameter = new SqlParameter("@InsuranceTypeId", insuranceType);
                DataTable table = _exQuery.ExecuteRawQuery(query, new[] { parameter });

                if (table.Rows.Count > 0)
                {
                    return new JsonResult(table);
                }
                else
                {
                    return new JsonResult("Insurance Type not found");
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
        [Route("ShowInsurancePackageHeaderByInsuranceProvider/{insuranceProvider}")]
        public JsonResult GetInsurancePackageHeaderByInsuranceProvider(int insuranceProvider)
        {
            ResponseDto response = new ResponseDto();
            try
            {
                string query = "SELECT b.id, b.packageName, c.provider, c.email, e.content, f.typeName, b.priority, b.isActive, b.dateModified, b.dateCreated " +
                "FROM InsurancePackageHeader b " +
                "LEFT JOIN InsuranceProvider c ON c.id = b.insuranceProviderId " +
                "LEFT JOIN Term e ON e.id = b.termId " +
                "LEFT JOIN InsuranceType f ON b.insuranceTypeId = f.id " +

                "WHERE c.id = @InsuranceProviderId ORDER BY b.priority asc";


                SqlParameter parameter = new SqlParameter("@InsuranceProviderId", insuranceProvider);
                DataTable table = _exQuery.ExecuteRawQuery(query, new[] { parameter });

                if (table.Rows.Count > 0)
                {
                    return new JsonResult(table);
                }
                else
                {
                    return new JsonResult("Insurance Package Header ID not found");
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
        [Route("ShowInsurancePackageHeaderByTypeId/{typeId}")]
        public JsonResult GetInsurancePackageHeaderByTypeId(int typeId)
        {
            ResponseDto response = new ResponseDto();
            try
            {
                string query = "SELECT b.id, b.packageName, c.provider, c.email, e.content, f.typeName, b.priority, b.isActive, b.dateModified, b.dateCreated " +
                "FROM InsurancePackageHeader b " +
                "LEFT JOIN InsuranceProvider c ON c.id = b.insuranceProviderId " +
                "LEFT JOIN Term e ON e.id = b.termId " +
                "LEFT JOIN InsuranceType f ON b.insuranceTypeId = f.id " +

                "WHERE f.id = @TypeId ORDER BY b.priority asc";


                SqlParameter parameter = new SqlParameter("@TypeId", typeId);
                DataTable table = _exQuery.ExecuteRawQuery(query, new[] { parameter });

                if (table.Rows.Count > 0)
                {
                    return new JsonResult(table);
                }
                else
                {
                    return new JsonResult("Insurance Type not found");
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
        [Route("AddInsurancePackageHeader")]
        public JsonResult AddInsurancePackageHeader([FromBody] InsurancePackageHeader insurancePackageHeader)
        {
            ResponseDto response = new ResponseDto();
            try
            {
                string query = "INSERT INTO InsurancePackageHeader (priority, packageName, insuranceTypeId, termId, insuranceProviderId, dateCreated, dateModified, isActive) " +
                           "VALUES (@Priority, @PackageName, @InsuranceTypeId, @TermId, @InsuranceProviderId, GETDATE(), GETDATE(), @IsActive)";

                SqlParameter[] parameters =
                {
                    new SqlParameter("@Priority", insurancePackageHeader.Priority),
                    new SqlParameter("@PackageName", insurancePackageHeader.PackageName),
                    new SqlParameter("@InsuranceTypeId", insurancePackageHeader.InsuranceTypeId),
                    new SqlParameter("@TermId", (object)insurancePackageHeader.TermId ?? DBNull.Value),
                    new SqlParameter("@InsuranceProviderId", insurancePackageHeader.InsuranceProviderId),
                    new SqlParameter("@IsActive", insurancePackageHeader.IsActive),
                };

                _exQuery.ExecuteRawQuery(query, parameters);

                return new JsonResult("Insurance Package Header added successfully");
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
        [Route("EditInsurancePackageHeader/{id}")]
        public JsonResult EditInsurancePackageHeader(int id, [FromBody] InsurancePackageHeader insurancePackageHeader)
        {
            ResponseDto response = new ResponseDto();
            try
            {
                string query = "UPDATE InsurancePackageHeader " +
                           "SET priority = @Priority, packageName = @PackageName, insuranceTypeId = @InsuranceTypeId, termId = @TermId, insuranceProviderId = @InsuranceProviderId, isActive = @IsActive, dateModified = GETDATE() " +
                           "WHERE id = @Id";

                SqlParameter[] parameters =
                {
                    new SqlParameter("@Id", id),
                    new SqlParameter("@Priority", insurancePackageHeader.Priority),
                    new SqlParameter("@PackageName", insurancePackageHeader.PackageName),
                    new SqlParameter("@InsuranceTypeId", insurancePackageHeader.InsuranceTypeId),
                    new SqlParameter("@TermId", (object)insurancePackageHeader.TermId ?? DBNull.Value),
                    new SqlParameter("@InsuranceProviderId", insurancePackageHeader.InsuranceProviderId),
                    new SqlParameter("@IsActive", insurancePackageHeader.IsActive),
                };


                _exQuery.ExecuteRawQuery(query, parameters);


                return new JsonResult("Insurance Package Header updated successfully");
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
        [Route("DeleteInsurancePackageHeader")]
        public JsonResult DeleteInsurancePackageHeader([FromBody] List<int> insurancePackageHeaderIds)
        {
            ResponseDto response = new ResponseDto();
            try
            {
                if (insurancePackageHeaderIds == null || insurancePackageHeaderIds.Count == 0)
                {
                    return new JsonResult("No insurance package header IDs provided for deletion");
                }

                StringBuilder deleteQuery = new StringBuilder("DELETE FROM InsurancePackageHeader WHERE id IN (");


                List<SqlParameter> parameters = new List<SqlParameter>();

                for (int i = 0; i < insurancePackageHeaderIds.Count; i++)
                {
                    string parameterName = "@InsurancePackageHeaderId" + i;
                    deleteQuery.Append(parameterName);

                    if (i < insurancePackageHeaderIds.Count - 1)
                    {
                        deleteQuery.Append(", ");
                    }

                    parameters.Add(new SqlParameter(parameterName, insurancePackageHeaderIds[i]));
                }

                deleteQuery.Append(");");

                _exQuery.ExecuteRawQuery(deleteQuery.ToString(), parameters.ToArray());


                return new JsonResult("Insurance Package Header deleted successfully");
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
        [Route("SearchInsurancePackageHeader")]
        public JsonResult SearchInsurancePackageHeader(string searchQuery)
        {
            ResponseDto response = new ResponseDto();
            try
            {
                string query = "SELECT b.id, b.packageName, b.priority, c.provider, c.email, e.content, f.typeName, b.dateModified, b.dateCreated " +
                           "FROM InsurancePackageHeader b " +
                           "LEFT JOIN InsuranceProvider c ON c.id = b.insuranceProviderId " +
                           "LEFT JOIN Term e ON e.id = b.termId " +
                           "LEFT JOIN InsuranceType f ON b.insuranceTypeId = f.id " +
                           "WHERE b.id LIKE @searchQuery OR " +
                           "b.packageName LIKE @searchQuery OR " +
                           "c.provider LIKE @searchQuery OR " +
                           "c.email LIKE @searchQuery OR " +
                           "f.typeName LIKE @searchQuery OR " +
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
        [Route("FilterInsurancePackageHeader")]
        public JsonResult FilterInsurancePackageHeader([FromQuery] DateTime? startDate = null, [FromQuery] DateTime? endDate = null)
        {
            ResponseDto response = new ResponseDto();
            try
            {
                string query = "SELECT b.id, b.packageName, b.priority, c.provider, c.email, e.content, f.typeName, b.dateModified, b.dateCreated " +
                           "FROM InsurancePackageHeader b " +
                           "INNER JOIN InsuranceProvider c ON c.id = b.insuranceProviderId " +
                           "INNER JOIN Term e ON e.id = b.termId " +
                           "INNER JOIN InsuranceType f ON b.insuranceTypeId = f.id ";

                List<SqlParameter> parameters = new List<SqlParameter>();

                if (startDate.HasValue && endDate.HasValue)
                {

                    startDate = startDate.Value.Date.AddHours(0).AddMinutes(0).AddSeconds(0);
                    endDate = endDate.Value.Date.AddHours(23).AddMinutes(59).AddSeconds(59);

                    query += (parameters.Count == 0 ? " WHERE " : " AND ") + "b.dateCreated >= @startDate AND b.dateCreated <= @endDate";
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
