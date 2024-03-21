using Microsoft.AspNetCore.Mvc;
using KMS.Models;
using KMS.Tools;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.ComponentModel.DataAnnotations;
using ZXing;

namespace KMS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QRController : ControllerBase
    {
        private readonly KioskManagementSystemContext _dbcontext;
        private IConfiguration _configuration;
        private readonly ExecuteQuery _exQuery;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private int number = 1060;

        public QRController(IWebHostEnvironment hostingEnvironment, IConfiguration configuration, KioskManagementSystemContext context, ExecuteQuery exQuery)
        {
            _hostingEnvironment = hostingEnvironment;
            _dbcontext = context;
            _configuration = configuration;
            _exQuery = exQuery;
        }

        

        [HttpPost]
        [Route("ScanQR")]
        public IActionResult ScanQR([FromForm] ScanQRModel scanQrModel)
        {
            try
            {
                if (scanQrModel.file == null || scanQrModel.file.Length == 0)
                {
                    return BadRequest("No file uploaded.");
                }

                int attempts = 300;

                while (attempts > 0)
                {
                    using (var ms = new MemoryStream())
                    {
                        scanQrModel.file.CopyTo(ms);
                        var imageData = ms.ToArray();

                        using (MemoryStream imageStream = new MemoryStream(imageData))
                        {
                            Bitmap originalBitmap = new Bitmap(imageStream);

                            Bitmap croppedBitmap = new Bitmap(originalBitmap.Width, originalBitmap.Height);

                            using (Graphics g = Graphics.FromImage(croppedBitmap))
                            {
                                Rectangle cropArea = new Rectangle(number, 40, 160, 160);

                                g.DrawImage(originalBitmap, new Rectangle(0, 0, 646, 607), cropArea, GraphicsUnit.Pixel);
                            }

                            MemoryStream croppedImageStream = new MemoryStream();
                            croppedBitmap.Save(croppedImageStream, ImageFormat.Jpeg);
                            croppedImageStream.Seek(0, SeekOrigin.Begin);

                            Bitmap bitmap = new Bitmap(croppedImageStream);

                            var reader = new ZXing.Windows.Compatibility.BarcodeReader();
                            var result = reader.Decode(bitmap);

                            if (result != null)
                            {
                                var qrText = result.Text;
                                var qrData = qrText.Split('|');

                                var birthday = DateTime.ParseExact(qrData[3], "ddMMyyyy", null);

                                var jsonResult = new JsonResult(new
                                {
                                    IdenNumber = qrData[0],
                                    FullName = qrData[2],
                                    Birthday = birthday,
                                    Gender = qrData[4],
                                    Address = qrData[5]
                                });

                                return jsonResult;
                            }
                            else
                            {
                                if (number < 1050)
                                {
                                    number = number - 10;
                                }
                                else
                                {
                                    number--;
                                }

                                attempts--;
                                if (number >= 800 && number < 1060)
                                {
                                    continue;
                                }

                            }
                        }
                    }
                }

                return NotFound("No QR code found in the image.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

    }

    public class ScanQRModel
    {
        public IFormFile file { get; set; }
    }
}
