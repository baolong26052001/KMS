﻿using Microsoft.AspNetCore.Mvc;
using KMS.Models;
using KMS.Tools;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Text;

namespace KMS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InsuranceTypeController : ControllerBase
    {
        private readonly KioskManagementSystemContext _dbcontext;
        private IConfiguration _configuration;
        private readonly ExecuteQuery _exQuery;

        public InsuranceTypeController(IConfiguration configuration, KioskManagementSystemContext _context, ExecuteQuery exQuery)
        {
            _dbcontext = _context;
            _configuration = configuration;
            _exQuery = exQuery;
        }

        [HttpGet]
        [Route("ShowInsuranceType")]
        public JsonResult GetInsuranceType()
        {
            ResponseDto response = new ResponseDto();
            try
            {
                string query = "SELECT * from InsuranceType";

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
        [Route("ShowInsuranceType/{id}")]
        public JsonResult GetInsuranceTypeById(int id)
        {
            ResponseDto response = new ResponseDto();
            try
            {
                string query = "SELECT * from InsuranceType where id=@Id";

                SqlParameter parameter = new SqlParameter("@Id", id);
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
        [Route("AddInsuranceType")]
        public JsonResult AddInsuranceType([FromBody] InsuranceType insuranceType)
        {
            ResponseDto response = new ResponseDto();
            try
            {
                string query = "INSERT INTO InsuranceType (typeName, dateModified, dateCreated) " +
                           "VALUES (@TypeName, GETDATE(), GETDATE())";
                string query2 = "INSERT INTO TAudit (action, tableName, dateModified, dateCreated, isActive) VALUES ('Add', 'InsuranceType', GETDATE(), GETDATE(), 1)";

                SqlParameter[] parameters =
                {
                    new SqlParameter("@TypeName", insuranceType.TypeName),

                };
                SqlParameter[] parameters2 = { };

                _exQuery.ExecuteRawQuery(query, parameters);
                _exQuery.ExecuteRawQuery(query2, parameters2);

                return new JsonResult("Insurance Type added successfully");
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
        [Route("EditInsuranceType/{id}")]
        public JsonResult EditInsuranceType(int id, [FromBody] InsuranceType insuranceType)
        {
            ResponseDto response = new ResponseDto();
            try
            {
                string query = "UPDATE InsuranceType " +
                           "SET typeName = @TypeName, dateModified = GETDATE() " +
                           "WHERE id = @id";
                string query2 = "INSERT INTO TAudit (action, tableName, dateModified, dateCreated, isActive) VALUES ('Edit', 'InsuranceType', GETDATE(), GETDATE(), 1)";

                SqlParameter[] parameters =
                {
                    new SqlParameter("@id", id),
                    new SqlParameter("@TypeName", insuranceType.TypeName),


                };
                SqlParameter[] parameters2 = { };

                _exQuery.ExecuteRawQuery(query, parameters);
                _exQuery.ExecuteRawQuery(query2, parameters2);

                return new JsonResult("Insurance type updated successfully");
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
        [Route("DeleteInsuranceType")]   
        public JsonResult DeleteInsuranceType([FromBody] List<int> insuranceTypeIds)
        {
            ResponseDto response = new ResponseDto();
            try
            {
                if (insuranceTypeIds == null || insuranceTypeIds.Count == 0)
                {
                    return new JsonResult("No insurance type IDs provided for deletion");
                }

                StringBuilder deleteQuery = new StringBuilder("DELETE FROM InsuranceType WHERE id IN (");
                string query2 = "INSERT INTO TAudit (action, tableName, dateModified, dateCreated, isActive) VALUES ('Delete', 'InsuranceType', GETDATE(), GETDATE(), 1)";

                List<SqlParameter> parameters = new List<SqlParameter>();
                SqlParameter[] parameters2 = { };
                for (int i = 0; i < insuranceTypeIds.Count; i++)
                {
                    string parameterName = "@InsuranceTypeId" + i;
                    deleteQuery.Append(parameterName);

                    if (i < insuranceTypeIds.Count - 1)
                    {
                        deleteQuery.Append(", ");
                    }

                    parameters.Add(new SqlParameter(parameterName, insuranceTypeIds[i]));
                }

                deleteQuery.Append(");");

                _exQuery.ExecuteRawQuery(deleteQuery.ToString(), parameters.ToArray());
                _exQuery.ExecuteRawQuery(query2, parameters2);

                return new JsonResult("Insurance type deleted successfully");
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