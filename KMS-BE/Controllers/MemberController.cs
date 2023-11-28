using Microsoft.AspNetCore.Mvc;
using KMS.Models;

namespace KMS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MemberController : ControllerBase
    {
        private readonly KioskManagementSystemContext _dbcontext;
        private IConfiguration _configuration;

        public MemberController(IConfiguration configuration, KioskManagementSystemContext _context)
        {
            _dbcontext = _context;
            _configuration = configuration;
        }

        [HttpGet]
        [Route("ShowMember")]
        public async Task<IActionResult> GetMember()
        {

            try
            {
                List<Lmember> listMember = _dbcontext.Lmembers.ToList();
                if (listMember != null)
                {
                    return Ok(listMember);
                }
                return Ok("There are no members in the database");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost]
        [Route("AddMember")]
        public async Task<IActionResult> CreateMember([FromBody] Lmember newMember)
        {
            try
            {
                newMember.DateCreated = DateTime.Now;
                newMember.IsActive = true;

                _dbcontext.Lmembers.Add(newMember);
                await _dbcontext.SaveChangesAsync();

                return Ok("Member created successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("UpdateMember/{id}")]
        public async Task<IActionResult> UpdateMember(int id, [FromBody] Lmember updatedMember)
        {
            try
            {
                var existingMember = await _dbcontext.Lmembers.FindAsync(id);

                if (existingMember == null)
                {
                    return NotFound($"Member with ID {id} not found");
                }

                existingMember.DateModified = DateTime.Now;
                existingMember.FirstName = updatedMember.FirstName;
                existingMember.LastName = updatedMember.LastName;
                
                await _dbcontext.SaveChangesAsync();

                return Ok($"Member with ID {id} updated successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("DeleteMember/{id}")]
        public async Task<IActionResult> DeleteMember(int id)
        {
            try
            {
                var memberToDelete = await _dbcontext.Lmembers.FindAsync(id);

                if (memberToDelete == null)
                {
                    return NotFound($"Member with ID {id} not found");
                }

                _dbcontext.Lmembers.Remove(memberToDelete);
                await _dbcontext.SaveChangesAsync();

                return Ok($"Member with ID {id} deleted successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}