﻿using Microsoft.AspNetCore.Mvc;
using KMS.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using System.Data;

namespace KMS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class KioskController : ControllerBase
    {
        private readonly KioskManagementSystemContext _dbcontext;
        private IConfiguration _configuration;

        public KioskController(IConfiguration configuration, KioskManagementSystemContext _context)
        {
            _dbcontext = _context;
            _configuration = configuration;
        }

        [HttpGet]
        [Route("ShowKioskSetup")]
        public JsonResult GetKiosk()
        {
            string query = "select k.id,  k.kioskName, k.location, st.stationName, ss.packageName, k.kioskStatus, k.cameraStatus, k.cashDepositStatus, k.scannerStatus, k.printerStatus\r\nfrom TKiosk k, TStation st, TSlideshow ss\r\nwhere k.stationCode = st.id and ss.id = k.slidePackage";
            DataTable table = new DataTable();
            string sqlDatasource = _configuration.GetConnectionString("DefaultConnection");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDatasource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult(table);
        }

        [HttpGet]
        [Route("ShowKioskSetup/{slidePackage}")]
        public async Task<IActionResult> GetKiosksBySlidePackage(int slidePackage)
        {
            try
            {
                List<Tkiosk> kiosks = await _dbcontext.Tkiosks
                    .Where(k => k.SlidePackage == slidePackage)
                    .ToListAsync();

                if (kiosks != null && kiosks.Count > 0)
                {
                    return Ok(kiosks);
                }

                return Ok($"No kiosks found with SlidePackage = {slidePackage}");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost]
        [Route("AddKiosk")]
        public async Task<IActionResult> AddKiosk([FromBody] Tkiosk kiosk)
        {
            try
            {
                var dbKiosk = _dbcontext.Tkiosks.Where(k => k.KioskName == kiosk.KioskName).Select(k => new
                {
                    k.Id,
                    k.KioskName,
                    k.IsActive
                }).FirstOrDefault();

                if (dbKiosk != null)
                {
                    return BadRequest("Kiosk name already exist");
                }
                kiosk.DateCreated = DateTime.Now;

                kiosk.IsActive = true;
                _dbcontext.Tkiosks.Add(kiosk);
                await _dbcontext.SaveChangesAsync();
                return Ok("Kiosk is add successfully");
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("UpdateKiosk/{id}")]
        public async Task<IActionResult> UpdateKiosk(int id, [FromBody] Tkiosk updatedKiosk)
        {
            try
            {
                var existingKiosk = await _dbcontext.Tkiosks.FindAsync(id);

                if (existingKiosk == null)
                {
                    return NotFound("Kiosk not found");
                }

                
                var isDuplicateName = await _dbcontext.Tkiosks
                    .Where(k => k.KioskName == updatedKiosk.KioskName && k.Id != id)
                    .AnyAsync();

                if (isDuplicateName)
                {
                    return BadRequest("Kiosk name already exists");
                }

                existingKiosk.DateModified = DateTime.Now;
                existingKiosk.KioskName = updatedKiosk.KioskName;
                existingKiosk.IsActive = updatedKiosk.IsActive;

                
                await _dbcontext.SaveChangesAsync();

                return Ok("Kiosk updated successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("DeleteKiosk/{id}")]
        public async Task<IActionResult> DeleteKiosk(int id)
        {
            try
            {
                var existingKiosk = await _dbcontext.Tkiosks.FindAsync(id);

                if (existingKiosk == null)
                {
                    return NotFound("Kiosk not found");
                }

                _dbcontext.Tkiosks.Remove(existingKiosk);
                await _dbcontext.SaveChangesAsync();

                return Ok("Kiosk deleted successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }
}