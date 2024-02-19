using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;

namespace KMS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FaceIDController : ControllerBase
    {
        private readonly string _targetFolder;

        public FaceIDController()
        {
            _targetFolder = @"bin\Debug\net6.0\IdImage";
        }

        [HttpPost]
        [Route("StoreImages")]
        public JsonResult StoreImages(IFormFile file1)
        {
            try
            {
                if (file1 == null || file1.Length == 0)
                {
                    return new JsonResult(new
                    {
                        Error = "Invalid files"
                    })
                    {
                        StatusCode = StatusCodes.Status400BadRequest
                    };
                }

                var uniqueFileName1 = file1.FileName;
                

                var filePath1 = Path.Combine(_targetFolder, uniqueFileName1);
                

                using (var fileStream1 = new FileStream(filePath1, FileMode.Create))
                {
                    file1.CopyTo(fileStream1);
                }

                

                return new JsonResult(new
                {
                    Message = "Face ID image stored successfully"
                })
                {
                    StatusCode = StatusCodes.Status200OK
                };
            }
            catch (Exception ex)
            {
                return new JsonResult(new
                {
                    Error = $"Error: {ex.Message}"
                })
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }
        }
    }
}
