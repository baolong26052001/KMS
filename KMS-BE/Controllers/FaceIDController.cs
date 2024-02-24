using KMS.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;

namespace KMS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FaceIDController : ControllerBase
    {
        private readonly string _remoteApiUrl = "https://speedpos.facedb.test.verigram.cloud/facedb/find";
        private readonly KioskManagementSystemContext _dbContext;

        public FaceIDController(KioskManagementSystemContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost]
        [Route("FindPersonByImage")]
        public JsonResult FindPersonByImage(IFormFile img_file, int person_id, string image_id)
        {
            try
            {
                if (img_file == null || img_file.Length == 0)
                {
                    return new JsonResult(new { Error = "Invalid image file" })
                    {
                        StatusCode = 400
                    };
                }

                using (var client = new HttpClient())
                {
                    using (var formData = new MultipartFormDataContent())
                    {
                        formData.Add(new StreamContent(img_file.OpenReadStream()), "img_file", img_file.FileName);
                        formData.Add(new StringContent("default"), "list_name");
                        formData.Add(new StringContent(person_id.ToString()), "person_id");
                        formData.Add(new StringContent(image_id), "image_id");

                        var response = client.PostAsync(_remoteApiUrl, formData).Result;
                        response.EnsureSuccessStatusCode();

                        var responseContent = response.Content.ReadAsStringAsync().Result;

                        // Deserialize the JSON response
                        var result = JsonConvert.DeserializeObject<Result>(responseContent);

                        int personId = int.Parse(result.result[0].person_id);

                        // Compare person_id and image_id to the database
                        var person = _dbContext.Lmembers.FirstOrDefault(l => l.Id == personId && l.IdenNumber == image_id);

                        if (person != null)
                        {
                            // If the person exists in the database, execute the query
                            var queryResult = _dbContext.Lmembers.Where(l => l.Id == person_id).ToList();
                            return new JsonResult(queryResult)
                            {
                                StatusCode = 200
                            };
                        }
                        else
                        {
                            return new JsonResult(new { Error = "Person not found in the database" })
                            {
                                StatusCode = 404
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return new JsonResult(new { Error = $"An error occurred: {ex.Message}" })
                {
                    StatusCode = 500
                };
            }
        }

        // Define classes to represent the JSON response
        public class Result
        {
            public List<ResultItem> result { get; set; }
        }

        public class ResultItem
        {
            public string person_id { get; set; }
            public double similarity { get; set; }
        }
    }
}
