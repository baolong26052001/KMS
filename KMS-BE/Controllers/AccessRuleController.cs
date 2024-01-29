using Microsoft.AspNetCore.Mvc;
using KMS.Models;
using KMS.Tools;
using System.Data;
using Microsoft.Data.SqlClient;

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
            string query = "select TUserGroup.groupName, TAccessRule.* from TAccessRule, TUserGroup where TAccessRule.groupId = TUserGroup.id";
            DataTable table = _exQuery.ExecuteRawQuery(query);
            return new JsonResult(table);
        }

        [HttpGet]
        [Route("ShowPermissionInfoInEditPage/{groupId}")]
        public JsonResult ShowPermissionInfoInEditPage(int groupId) // show data in edit page when update permission
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
        }

        [HttpPut]
        [Route("UpdatePermission/{groupId}")]
        public JsonResult UpdatePermission(int groupId, [FromBody] TaccessRule accessRule)
        {
            string query = "UPDATE TAccessRule SET canView = @CanView, canAdd = @CanAdd, canUpdate = @CanUpdate, canDelete = @CanDelete, site = @Site " +
                           "WHERE groupId = @GroupId";

            SqlParameter[] parameters =
            {
                new SqlParameter("@GroupId", groupId),
                new SqlParameter("@CanView", (object)accessRule.CanView ?? DBNull.Value),
                new SqlParameter("@CanAdd", (object)accessRule.CanAdd ?? DBNull.Value),
                new SqlParameter("@CanUpdate", (object)accessRule.CanUpdate ?? DBNull.Value),
                new SqlParameter("@CanDelete", (object)accessRule.CanDelete ?? DBNull.Value),
            };

            _exQuery.ExecuteRawQuery(query, parameters);
            return new JsonResult("Permission updated successfully");
        }

    }
}
