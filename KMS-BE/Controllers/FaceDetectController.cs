using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;

namespace KMS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FaceDetectController : ControllerBase
    {
        private readonly string _targetFolder;

        public FaceDetectController()
        {
            _targetFolder = @"bin\Debug\net6.0\FaceDetect";
        }

        [HttpPost]
        [Route("StoreImages")]
        public JsonResult StoreImages(IFormFile file1, IFormFile file2, IFormFile file3)
        {
            try
            {
                if (file1 == null || file1.Length == 0 || file2 == null || file2.Length == 0 || file3 == null || file3.Length == 0)
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
                var uniqueFileName2 = file2.FileName;
                var uniqueFileName3 = file3.FileName;

                var filePath1 = Path.Combine(_targetFolder, uniqueFileName1);
                var filePath2 = Path.Combine(_targetFolder, uniqueFileName2);
                var filePath3 = Path.Combine(_targetFolder, uniqueFileName3);

                using (var fileStream1 = new FileStream(filePath1, FileMode.Create))
                {
                    file1.CopyTo(fileStream1);
                }

                using (var fileStream2 = new FileStream(filePath2, FileMode.Create))
                {
                    file2.CopyTo(fileStream2);
                }

                using (var fileStream3 = new FileStream(filePath3, FileMode.Create))
                {
                    file3.CopyTo(fileStream3);
                }

                return new JsonResult(new
                {
                    Message = "Face Detect images stored successfully"
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
