using Microsoft.AspNetCore.Mvc;
using KMS.Models;
using KMS.Tools;
using System;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.ComponentModel.DataAnnotations;
using ZXing;
//using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using static KMS.Controllers.MemberController;

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
        private int initialVerticalPosition = 40;
        private readonly string _remoteApiUrl = "https://speedpos.facedb.test.verigram.cloud/facedb/insert";

        public QRController(IWebHostEnvironment hostingEnvironment, IConfiguration configuration, KioskManagementSystemContext context, ExecuteQuery exQuery)
        {
            _hostingEnvironment = hostingEnvironment;
            _dbcontext = context;
            _configuration = configuration;
            _exQuery = exQuery;
        }

        

        [HttpPost]
        [Route("ScanQR")]
        public async Task<IActionResult> ScanQR([FromForm] ScanQRModel scanQrModel)
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
                                Rectangle cropArea = new Rectangle(number, initialVerticalPosition, 160, 160);

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
                                    idenNumber = qrData[0],
                                    fullName = qrData[2],
                                    birthday = birthday,
                                    gender = qrData[4],
                                    address1 = qrData[5]
                                });

                                string checkIfExistsQuery = "SELECT COUNT(*) FROM LMember WHERE idenNumber = @IdenNumber";
                                SqlParameter[] checkIfExistsParameters = { new SqlParameter("@IdenNumber", qrData[0]) };
                                int count = _exQuery.ExecuteScalar<int>(checkIfExistsQuery, checkIfExistsParameters);
                                if (count > 0)
                                {
                                    return new JsonResult(new
                                    {
                                        Code = 300,
                                        Message = "This person already exists",
                                        idenNumber = qrData[0],
                                        fullName = qrData[2],
                                        birthday = birthday,
                                        gender = qrData[4],
                                        address1 = qrData[5]
                                    });
                                }

                                string query = @"
                                    IF NOT EXISTS (SELECT 1 FROM LMember WHERE idenNumber = @IdenNumber)
                                    BEGIN
                                        INSERT INTO LMember (imageIdCard, gender, fullName, birthday, idenNumber, address1, isActive, dateCreated, dateModified)
                                        VALUES (@ImageIdCard, @Gender, @FullName, @Birthday, @IdenNumber, @Address1, 0, GETDATE(), GETDATE());
                                        SELECT SCOPE_IDENTITY();
                                    END
                                ";

                                SqlParameter[] parameters =
                                {
                                    new SqlParameter("@ImageIdCard", scanQrModel.file.FileName),
                                    new SqlParameter("@IdenNumber", qrData[0]),
                                    new SqlParameter("@FullName", qrData[2]),
                                    new SqlParameter("@Birthday", birthday),
                                    new SqlParameter("@Gender", qrData[4]),
                                    new SqlParameter("@Address1", qrData[5]),
                                };

                                //_exQuery.ExecuteRawQuery(query, parameters);

                                int insertedId = _exQuery.ExecuteScalar<int>(query, parameters);
                                
                                

                                using (var client = new HttpClient())
                                {
                                    MultipartFormDataContent content = new MultipartFormDataContent
                                    {
                                        { new StreamContent(scanQrModel.file.OpenReadStream()), "img_file", scanQrModel.file.FileName },
                                        { new StringContent(insertedId.ToString()), "person_id" },
                                        { new StringContent(qrData[0]), "image_id" }
                                    };

                                    var response = await client.PostAsync(_remoteApiUrl, content);

                                    if (response.IsSuccessStatusCode)
                                    {
                                        return new JsonResult(new
                                        {
                                            Code = 200,
                                            Message = "Member added successfully",
                                            PersonId = insertedId.ToString(),
                                            ImageId = qrData[0],
                                            fullName = qrData[2],
                                            birthday = birthday,
                                            gender = qrData[4],
                                            address1 = qrData[5]
                                        });
                                    }
                                    else
                                    {
                                        return new JsonResult(new ResponseDto
                                        {
                                            Code = (int)response.StatusCode,
                                            Message = "Failed to add member. Remote API request failed."
                                        });
                                    }
                                }

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
                                else if (number < 800 && initialVerticalPosition == 40)
                                {
                                    initialVerticalPosition = 83;
                                    number = 1060;
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
