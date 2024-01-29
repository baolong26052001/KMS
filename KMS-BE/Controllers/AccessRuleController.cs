using Microsoft.AspNetCore.Mvc;
using KMS.Models;
using KMS.Tools;
using System.Data;

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

    }
}
