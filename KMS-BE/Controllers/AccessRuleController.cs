using Microsoft.AspNetCore.Mvc;
using KMS.Models;
using KMS.Tools;
using System.Data;
using Microsoft.Data.SqlClient;
using IPinfo.Models;

namespace KMS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccessRuleController : ControllerBase
    {
        private readonly KioskManagementSystemContext _dbcontext;
        private IConfiguration _configuration;
        private readonly ExecuteQuery _exQuery;

        public AccessRuleController(IConfiguration configuration, KioskManagementSystemContext _context, ExecuteQuery exQuery)
        {
            _dbcontext = _context;
            _configuration = configuration;
            _exQuery = exQuery;
        }

        [HttpGet]
        [Route("ShowPermission")]
        public JsonResult ShowPermission()
        {
            ResponseDto response = new ResponseDto();
            try
            {
                string query = "select TUserGroup.groupName, TAccessRule.* from TAccessRule, TUserGroup where TAccessRule.groupId = TUserGroup.id";

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
        [Route("ShowPermissionInfoInEditPage/{groupId}")]
        public JsonResult ShowPermissionInfoInEditPage(int groupId) // show data in edit page when update permission
        {
            ResponseDto response = new ResponseDto();
            try
            {
                string query = "select * from TAccessRule where groupId = @GroupId";

                SqlParameter parameter = new SqlParameter("@GroupId", groupId);
                DataTable table = _exQuery.ExecuteRawQuery(query, new[] { parameter });

                if (table.Rows.Count > 0)
                {
                    return new JsonResult(table);
                }
                else
                {
                    return new JsonResult("Group ID not found");
                }
                response.Data = table;
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
        [Route("ShowPermissionBySiteAndGroupId/{groupId}/{site}")]
        public JsonResult ShowPermissionBySiteAndGroupId(int groupId, string site)
        {
            ResponseDto response = new ResponseDto();
            try
            {
                string query = "select * from TAccessRule where groupId = @GroupId and site = @Site";

                SqlParameter[] parameters =
                {
                    new SqlParameter("@GroupId", groupId),
                    new SqlParameter("@Site", site),
                };

                DataTable table = _exQuery.ExecuteRawQuery(query, parameters);

                if (table.Rows.Count > 0)
                {
                    return new JsonResult(table);
                }
                else
                {
                    response.Code = 404;
                    response.Message = "Group ID and site not found";
                    response.Exception = "";
                    response.Data = null;
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
        [Route("AddPermission")]
        public JsonResult AddPermission([FromBody] TaccessRule taccessRule)
        {
            ResponseDto response = new ResponseDto();
            try
            {
                string query = "INSERT INTO TAccessRule (groupId, isActive, canView, canAdd, canUpdate, canDelete, site) " +
                           "VALUES (@GroupId, @IsActive, 0, 0, 0, 0, @Site)";

                SqlParameter[] parameters =
                {
                    new SqlParameter("@GroupId", taccessRule.GroupId),
                    new SqlParameter("@IsActive", taccessRule.IsActive),
                    new SqlParameter("@Site", taccessRule.Site),
                }; 


                _exQuery.ExecuteRawQuery(query, parameters);


                return new JsonResult("Permission added successfully");
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
        [Route("UpdatePermission/{groupId}/{site}")]
        public JsonResult UpdatePermission(int groupId, string site, [FromBody] TaccessRule accessRule)
        {
            ResponseDto response = new ResponseDto();
            try
            {
                string query = "UPDATE TAccessRule SET canView = @CanView, canAdd = @CanAdd, canUpdate = @CanUpdate, canDelete = @CanDelete " +
                           "WHERE groupId = @GroupId and site = @Site";

                SqlParameter[] parameters =
                {
                    new SqlParameter("@GroupId", groupId),
                    new SqlParameter("@Site", site),
                    new SqlParameter("@CanView", (object)accessRule.CanView ?? DBNull.Value),
                    new SqlParameter("@CanAdd", (object)accessRule.CanAdd ?? DBNull.Value),
                    new SqlParameter("@CanUpdate", (object)accessRule.CanUpdate ?? DBNull.Value),
                    new SqlParameter("@CanDelete", (object)accessRule.CanDelete ?? DBNull.Value),

                };

                _exQuery.ExecuteRawQuery(query, parameters);
                return new JsonResult("Permission updated successfully");
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
