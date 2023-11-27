using Microsoft.AspNetCore.Mvc;
using KMS.Models;

namespace KMS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsergroupController : ControllerBase
    {
        private readonly KioskManagementSystemContext _dbcontext;

        

        public UsergroupController(KioskManagementSystemContext _context)
        {
            _dbcontext = _context;
        }

        [HttpGet]
        [Route("ShowUsergroup")]
        public async Task<IActionResult> GetUsergroup()
        {
            
            try
            {
                List<TuserGroup> listUsergroup = _dbcontext.TuserGroups.ToList();
                if (listUsergroup != null) 
                {
                    return Ok(listUsergroup);
                }
                return Ok("There are no user group in the database");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost]
        [Route("AddUsergroup")]
        public IActionResult AddUsergroup([FromBody] TuserGroup usergroup)
        {
            try
            {
                usergroup.DateCreated = DateTime.Now;
                usergroup.IsActive = true;

                _dbcontext.TuserGroups.Add(usergroup);
                _dbcontext.SaveChanges();
                return Ok("User group added successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("UpdateUsergroup/{id}")]
        public IActionResult UpdateUsergroup(int id, [FromBody] TuserGroup updatedUsergroup)
        {
            try
            {
                var existingUsergroup = _dbcontext.TuserGroups.Find(id);
                if (existingUsergroup != null)
                {
                    existingUsergroup.DateModified = DateTime.Now;
                    // Update properties of existingUsergroup with values from updatedUsergroup
                    existingUsergroup.GroupName = updatedUsergroup.GroupName;
                    

                    _dbcontext.SaveChanges();
                    return Ok("User group updated successfully");
                }
                return NotFound("User group not found");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("DeleteUsergroup/{id}")]
        public IActionResult DeleteUsergroup(int id)
        {
            try
            {
                var usergroupToDelete = _dbcontext.TuserGroups.Find(id);
                if (usergroupToDelete != null)
                {
                    _dbcontext.TuserGroups.Remove(usergroupToDelete);
                    _dbcontext.SaveChanges();
                    return Ok("User group deleted successfully");
                }
                return NotFound("User group not found");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}