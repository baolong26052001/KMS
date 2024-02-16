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
    public class InsuranceProviderController : ControllerBase
    {
        private readonly KioskManagementSystemContext _dbcontext;
        private IConfiguration _configuration;
        private readonly ExecuteQuery _exQuery;

        public InsuranceProviderController(IConfiguration configuration, KioskManagementSystemContext _context, ExecuteQuery exQuery)
        {
            _dbcontext = _context;
            _configuration = configuration;
            _exQuery = exQuery;
        }

        [HttpGet]
        [Route("ShowInsuranceProvider")]
        public JsonResult GetInsuranceProvider()
        {
            ResponseDto response = new ResponseDto();
            try
            {
                string query = "SELECT * FROM InsuranceProvider";

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
        [Route("ShowInsuranceProvider/{id}")]
        public JsonResult GetInsuranceProviderById(int id)
        {
            ResponseDto response = new ResponseDto();
            try
            {
                string query = "SELECT * FROM InsuranceProvider where id=@Id";

                SqlParameter parameter = new SqlParameter("@Id", id);
                DataTable table = _exQuery.ExecuteRawQuery(query, new[] { parameter });

                if (table.Rows.Count > 0)
                {
                    return new JsonResult(table);
                }
                else
                {
                    return new JsonResult("Insurance Provider ID not found");
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
        [Route("AddInsuranceProvider")]
        public JsonResult AddInsuranceProvider([FromBody] InsuranceProvider insuranceProvider)
        {
            ResponseDto response = new ResponseDto();
            try
            {
                string query = "INSERT INTO InsuranceProvider (provider, email, dateCreated, dateModified) " +
                           "VALUES (@Provider, @Email, GETDATE(), GETDATE())";

                SqlParameter[] parameters =
                {
                    new SqlParameter("@Provider", insuranceProvider.Provider),
                    new SqlParameter("@Email", insuranceProvider.Email),
                };

                _exQuery.ExecuteRawQuery(query, parameters);

                return new JsonResult("Insurance Provider added successfully");
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
        [Route("EditInsuranceProvider/{id}")]
        public JsonResult EditInsuranceProvider(int id, [FromBody] InsuranceProvider insuranceProvider)
        {
            ResponseDto response = new ResponseDto();
            try
            {
                string query = "UPDATE InsuranceProvider " +
                           "SET provider = @Provider, email = @Email, dateModified = GETDATE() " +
                           "WHERE id = @Id";

                SqlParameter[] parameters =
                {
                    new SqlParameter("@Id", id),
                    new SqlParameter("@Provider", insuranceProvider.Provider),
                    new SqlParameter("@Email", insuranceProvider.Email),
                };


                _exQuery.ExecuteRawQuery(query, parameters);


                return new JsonResult("Insurance Provider updated successfully");
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
        [Route("DeleteInsuranceProvider")]
        public JsonResult DeleteInsuranceProvider([FromBody] List<int> insuranceProviderIds)
        {
            ResponseDto response = new ResponseDto();
            try
            {
                if (insuranceProviderIds == null || insuranceProviderIds.Count == 0)
                {
                    return new JsonResult("No insurance provider IDs provided for deletion");
                }

                StringBuilder deleteQuery = new StringBuilder("DELETE FROM InsuranceProvider WHERE id IN (");
                string query2 = "INSERT INTO TAudit (action, tableName, dateModified, dateCreated, isActive) VALUES ('Delete', 'InsuranceProvider', GETDATE(), GETDATE(), 1)";

                List<SqlParameter> parameters = new List<SqlParameter>();
                SqlParameter[] parameters2 = { };
                for (int i = 0; i < insuranceProviderIds.Count; i++)
                {
                    string parameterName = "@InsuranceProviderId" + i;
                    deleteQuery.Append(parameterName);

                    if (i < insuranceProviderIds.Count - 1)
                    {
                        deleteQuery.Append(", ");
                    }

                    parameters.Add(new SqlParameter(parameterName, insuranceProviderIds[i]));
                }

                deleteQuery.Append(");");

                _exQuery.ExecuteRawQuery(deleteQuery.ToString(), parameters.ToArray());
                _exQuery.ExecuteRawQuery(query2, parameters2);

                return new JsonResult("Insurance provider deleted successfully");
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
