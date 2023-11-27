using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using KMS.Models;
using KMS.Tools;

namespace KMS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly KioskManagementSystemContext _dbcontext;

        

        public UserController(KioskManagementSystemContext _context)
        {
            _dbcontext = _context;
        }

        [HttpGet]
        [Route("ShowUsers")]
        public async Task<IActionResult> GetUsers()
        {
            
            try
            {
                List<Tuser> listUsers = _dbcontext.Tusers.ToList();
                if (listUsers != null) 
                {
                    return Ok(listUsers);
                }
                return Ok("There are no users in the database");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet]
        [Route("ShowUsers/{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            try
            {
                var user = await _dbcontext.Tusers.FindAsync(id);

                if (user == null)
                {
                    return NotFound("User not found");
                }

                return Ok(user);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> userLogin([FromBody] Tuser user)
        {
            try
            {
                string password = Password.hashPassword(user.Password);
                var dbUser = _dbcontext.Tusers
                    .Where(u => u.Username == user.Username && u.Password == password)
                    .FirstOrDefault();

                if (dbUser == null)
                {
                    return BadRequest("Username or password is incorrect");
                }

                // Update LastLogin when the user logs in
                dbUser.LastLogin = DateTime.Now;
                await _dbcontext.SaveChangesAsync();

                return Ok("Login successful");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        [HttpPost]
        [Route("AddUser")]
        public async Task<IActionResult> userRegistration([FromBody] Tuser user)
        {
            try
            {
                var dbUser = _dbcontext.Tusers.Where(u => u.Username == user.Username).Select(u => new
                {
                    u.Id,
                    u.Username,
                    u.IsActive
                }).FirstOrDefault();

                if (dbUser != null)
                {
                    return BadRequest("Username already exist");
                }
                user.DateCreated = DateTime.Now;

                user.Password = Password.hashPassword(user.Password);
                user.IsActive = true;
                _dbcontext.Tusers.Add(user);
                await _dbcontext.SaveChangesAsync();
                return Ok("user is successfully registered");
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
            

            
        }

        [HttpPut]
        [Route("UpdateUser/{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] Tuser updatedUser)
        {
            try
            {
                var existingUser = await _dbcontext.Tusers.FindAsync(id);

                if (existingUser == null)
                {
                    return NotFound("User not found");
                }

                existingUser.DateModified = DateTime.Now;
                // Update properties of existingUser with values from updatedUser
                existingUser.Email = updatedUser.Email;
                

                await _dbcontext.SaveChangesAsync();

                return Ok("User updated successfully");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete]
        [Route("DeleteUser/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                var userToDelete = await _dbcontext.Tusers.FindAsync(id);

                if (userToDelete == null)
                {
                    return NotFound("User not found");
                }

                _dbcontext.Tusers.Remove(userToDelete);
                await _dbcontext.SaveChangesAsync();

                return Ok("User deleted successfully");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }

    
}