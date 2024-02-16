﻿using Microsoft.AspNetCore.Mvc;
using KMS.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Text;
using KMS.Tools;

namespace KMS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsergroupController : ControllerBase
    {
        private readonly KioskManagementSystemContext _dbcontext;
        private IConfiguration _configuration;
        private readonly ExecuteQuery _exQuery;

        public UsergroupController(IConfiguration configuration, KioskManagementSystemContext _context, ExecuteQuery exQuery)
        {
            _dbcontext = _context;
            _configuration = configuration;
            _exQuery = exQuery;
        }

        

        [HttpGet]
        [Route("ShowUsergroup")]
        public JsonResult GetUsergroup()
        {
            ResponseDto response = new ResponseDto();
            try
            {
                string query = "select ug.id, ug.groupName, ug.accessRuleId, ug.dateModified, ug.dateCreated, ug.isActive" +
                "\r\nfrom TUserGroup ug";
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
        [Route("ShowUsergroup/{id}")]
        public JsonResult GetUsergroupById(int id)
        {
            ResponseDto response = new ResponseDto();
            try
            {
                string query = "SELECT id, groupName, accessRuleId, dateModified, dateCreated, isActive " +
                           "FROM TUserGroup " +
                           "WHERE id = @Id";

                SqlParameter parameter = new SqlParameter("@Id", id);
                DataTable table = _exQuery.ExecuteRawQuery(query, new[] { parameter });

                if (table.Rows.Count > 0)
                {
                    return new JsonResult(table);
                }
                else
                {
                    return new JsonResult("Usergroup not found");
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
        [Route("FilterUsergroup")] // filter user group by group name and by active
        public JsonResult FilterUsergroup([FromQuery] string? groupName = null, [FromQuery] bool? isActive = null)
        {
            ResponseDto response = new ResponseDto();
            try
            {
                string query = "SELECT id, groupName, accessRuleId, dateModified, dateCreated, isActive " +
                           "FROM TUserGroup ";

                List<SqlParameter> parameters = new List<SqlParameter>();

                if (!string.IsNullOrEmpty(groupName))
                {
                    query += "WHERE groupName LIKE @groupName ";
                    parameters.Add(new SqlParameter("@groupName", "%" + groupName + "%"));
                }



                if (isActive.HasValue)
                {
                    query += (parameters.Count == 0 ? "WHERE" : "AND") + " isActive = @isActive ";
                    parameters.Add(new SqlParameter("@isActive", isActive.Value));
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



        [HttpPost]
        [Route("AddUsergroup")]
        public JsonResult AddUsergroup([FromBody] TuserGroup usergroup)
        {
            ResponseDto response = new ResponseDto();
            try
            {
                string query = "INSERT INTO TUserGroup (groupName, accessRuleId, dateModified, dateCreated, isActive) " +
                           "VALUES (@GroupName, @AccessRuleId, GETDATE(), GETDATE(), @IsActive)";
                string query2 = "INSERT INTO TAudit (action, tableName, dateModified, dateCreated, isActive) VALUES ('Add', 'TUsergroup', GETDATE(), GETDATE(), 1)";

                SqlParameter[] parameters =
                {
                    new SqlParameter("@GroupName", usergroup.GroupName),
                    new SqlParameter("@AccessRuleId", usergroup.AccessRuleId),
                    new SqlParameter("@IsActive", usergroup.IsActive)
                };
                SqlParameter[] parameters2 = { };

                _exQuery.ExecuteRawQuery(query, parameters);
                _exQuery.ExecuteRawQuery(query2, parameters2);

                return new JsonResult("Usergroup added successfully");
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
        [Route("UpdateUsergroup/{id}")]
        public JsonResult UpdateUsergroup(int id, [FromBody] Tusergroup usergroup)
        {
            ResponseDto response = new ResponseDto();
            try
            {
                string query = "UPDATE TUserGroup " +
                           "SET groupName = @GroupName, dateModified = GETDATE(), isActive = @IsActive " +
                           "WHERE id = @Id";
                string query2 = "INSERT INTO TAudit (action, tableName, dateModified, dateCreated, isActive) VALUES ('Update', 'TUsergroup', GETDATE(), GETDATE(), 1)";

                SqlParameter[] parameters =
                {
                    new SqlParameter("@Id", id),
                    new SqlParameter("@GroupName", usergroup.GroupName),

                    new SqlParameter("@IsActive", usergroup.IsActive)
                };
                SqlParameter[] parameters2 = { };

                _exQuery.ExecuteRawQuery(query, parameters);
                _exQuery.ExecuteRawQuery(query2, parameters2);

                return new JsonResult("Usergroup updated successfully");
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
        [Route("DeleteUsergroup")]
        public JsonResult DeleteUsergroup([FromBody] List<int> usergroupIds)
        {
            ResponseDto response = new ResponseDto();
            try
            {
                if (usergroupIds == null || usergroupIds.Count == 0)
                {
                    return new JsonResult("No user group IDs provided for deletion");
                }

                StringBuilder deleteQuery = new StringBuilder("DELETE FROM TUsergroup WHERE id IN (");
                string query2 = "INSERT INTO TAudit (action, tableName, dateModified, dateCreated, isActive) VALUES ('Delete', 'TUsergroup', GETDATE(), GETDATE(), 1)";


                List<SqlParameter> parameters = new List<SqlParameter>();
                SqlParameter[] parameters2 = { };
                for (int i = 0; i < usergroupIds.Count; i++)
                {
                    string parameterName = "@UsergroupId" + i;
                    deleteQuery.Append(parameterName);

                    if (i < usergroupIds.Count - 1)
                    {
                        deleteQuery.Append(", ");
                    }

                    parameters.Add(new SqlParameter(parameterName, usergroupIds[i]));
                }

                deleteQuery.Append(");");

                _exQuery.ExecuteRawQuery(deleteQuery.ToString(), parameters.ToArray());
                _exQuery.ExecuteRawQuery(query2, parameters2);

                return new JsonResult("User group deleted successfully");
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
        [Route("SearchUsergroup")]
        public JsonResult SearchUsergroup(string searchQuery)
        {
            ResponseDto response = new ResponseDto();
            try
            {
                string query = "SELECT id, groupName, accessRuleId, dateModified, dateCreated, isActive " +
                           "FROM TUserGroup " +
                           "WHERE id LIKE @searchQuery OR " +
                           "groupName LIKE @searchQuery";

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
