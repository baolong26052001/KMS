﻿//using DocumentFormat.OpenXml.Office2010.Excel;
using KMS.Models;
using KMS.Tools;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
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
        private readonly ExecuteQuery _exQuery;

        public FaceIDController(KioskManagementSystemContext dbContext, ExecuteQuery exQuery)
        {
            _dbContext = dbContext;
            _exQuery = exQuery;
        }

        [HttpPost]
        [Route("FindPersonByImage")]
        public JsonResult FindPersonByImage(IFormFile img_file)
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

                        var response = client.PostAsync(_remoteApiUrl, formData).Result;
                        response.EnsureSuccessStatusCode();

                        var responseContent = response.Content.ReadAsStringAsync().Result;

                        // Deserialize the JSON response
                        var result = JsonConvert.DeserializeObject<Result>(responseContent);

                        // Filter results with similarity >= 90
                        var filteredResults = result.result
                            .Where(r => r.similarity >= 90)
                            .ToList();

                        // Iterate over filtered results to find matching IDs in the database
                        foreach (var filteredResult in filteredResults)
                        {
                            int personId = int.Parse(filteredResult.person_id);

                            // Query the database
                            string query = "SELECT * FROM LMember WHERE id = @Id";

                            SqlParameter parameter = new SqlParameter("@Id", personId);
                            DataTable table = _exQuery.ExecuteRawQuery(query, new[] { parameter });

                            if (table.Rows.Count > 0)
                            {
                                // If data found in the database, return the result
                                return new JsonResult(new { Code = 200, Data = table })
                                {
                                    StatusCode = 200
                                };
                            }
                        }

                        // If no matching member found in the database
                        return new JsonResult(new { Code = 404, Data = "No matching result found in the database" })
                        {
                            StatusCode = 404
                        };
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
