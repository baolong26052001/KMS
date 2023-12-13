using Microsoft.AspNetCore.Mvc;
using KMS.Models;

namespace KMS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccessRuleController : ControllerBase
    {
        private readonly KioskManagementSystemContext _dbcontext;
        private IConfiguration _configuration;

        public AccessRuleController(IConfiguration configuration, KioskManagementSystemContext _context)
        {
            _dbcontext = _context;
            _configuration = configuration;
        }

        
        
    }
}