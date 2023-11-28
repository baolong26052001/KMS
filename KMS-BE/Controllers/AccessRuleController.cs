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

        [HttpGet]
        [Route("ShowAccessRules")]
        public async Task<IActionResult> GetAccessRules()
        {

            try
            {
                List<TaccessRule> listAccessRule = _dbcontext.TaccessRules.ToList();
                if (listAccessRule != null)
                {
                    return Ok(listAccessRule);
                }
                return Ok("There are no access rule in the database");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost]
        [Route("AddAccessRule")]
        public async Task<IActionResult> AddAccessRule([FromBody] TaccessRule newAccessRule)
        {
            try
            {
                newAccessRule.DateCreated = DateTime.Now;
                newAccessRule.IsActive = true;
                _dbcontext.TaccessRules.Add(newAccessRule);
                await _dbcontext.SaveChangesAsync();

                return Ok("Access Rule added successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("UpdateAccessRule/{id}")]
        public async Task<IActionResult> UpdateAccessRule(int id, [FromBody] TaccessRule updatedAccessRule)
        {
            try
            {
                var existingAccessRule = await _dbcontext.TaccessRules.FindAsync(id);

                if (existingAccessRule == null)
                {
                    return NotFound($"Access Rule with ID {id} not found");
                }

                existingAccessRule.DateModified = DateTime.Now;
                // Update the properties of the existing access rule
                existingAccessRule.FeatureName = updatedAccessRule.FeatureName;
                // Update other properties as needed

                // Save changes to the database
                await _dbcontext.SaveChangesAsync();

                return Ok($"Access Rule with ID {id} updated successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("DeleteAccessRule/{id}")]
        public async Task<IActionResult> DeleteAccessRule(int id)
        {
            try
            {
                var accessRuleToDelete = await _dbcontext.TaccessRules.FindAsync(id);

                if (accessRuleToDelete == null)
                {
                    return NotFound($"Access Rule with ID {id} not found");
                }

                _dbcontext.TaccessRules.Remove(accessRuleToDelete);
                await _dbcontext.SaveChangesAsync();

                return Ok($"Access Rule with ID {id} deleted successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
    }
}