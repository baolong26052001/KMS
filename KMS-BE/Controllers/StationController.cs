using Microsoft.AspNetCore.Mvc;
using KMS.Models;
using Microsoft.EntityFrameworkCore;

namespace KMS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StationController : ControllerBase
    {
        private readonly KioskManagementSystemContext _dbcontext;



        public StationController(KioskManagementSystemContext _context)
        {
            _dbcontext = _context;
        }

        [HttpGet]
        [Route("ShowStation")]
        public async Task<IActionResult> GetStation()
        {

            try
            {
                List<Tstation> listStation = _dbcontext.Tstations.ToList();
                if (listStation != null)
                {
                    return Ok(listStation);
                }
                return Ok("There are no station in the database");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost]
        [Route("AddStation")]
        public async Task<IActionResult> CreateStation([FromBody] Tstation newStation)
        {
            try
            {
                var isDuplicateName = await _dbcontext.Tstations
                    .AnyAsync(s => s.StationName == newStation.StationName);

                if (isDuplicateName)
                {
                    return BadRequest("Station name already exists");
                }
                newStation.DateCreated = DateTime.Now;
                
                newStation.IsActive = true;
                _dbcontext.Tstations.Add(newStation);
                await _dbcontext.SaveChangesAsync();

                return Ok("Station created successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("UpdateStation/{id}")]
        public async Task<IActionResult> UpdateStation(int id, [FromBody] Tstation updatedStation)
        {
            try
            {
                var existingStation = await _dbcontext.Tstations.FindAsync(id);

                if (existingStation == null)
                {
                    return NotFound("Station not found");
                }

                var isDuplicateName = await _dbcontext.Tstations
                    .Where(s => s.StationName == updatedStation.StationName && s.Id != id)
                    .AnyAsync();

                if (isDuplicateName)
                {
                    return BadRequest("Station name already exists");
                }

                existingStation.DateModified = DateTime.Now;
                existingStation.StationName = updatedStation.StationName;
                existingStation.CompanyName = updatedStation.CompanyName;
                existingStation.Address = updatedStation.Address;
                existingStation.City = updatedStation.City;

                await _dbcontext.SaveChangesAsync();

                return Ok("Station updated successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        

        [HttpDelete]
        [Route("DeleteStation/{id}")]
        public async Task<IActionResult> DeleteStation(int id)
        {
            try
            {
                var existingStation = await _dbcontext.Tstations.FindAsync(id);

                if (existingStation == null)
                {
                    return NotFound("Station not found");
                }

                _dbcontext.Tstations.Remove(existingStation);
                await _dbcontext.SaveChangesAsync();

                return Ok("Station deleted successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }
}