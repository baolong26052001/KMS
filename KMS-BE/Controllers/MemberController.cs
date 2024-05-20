using Microsoft.AspNetCore.Mvc;
using KMS.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Components.Authorization;
using KMS.Tools;
using System.Globalization;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Diagnostics.Metrics;
using Microsoft.EntityFrameworkCore;
using System.Text;
using Twilio.Rest.Api.V2010.Account.Usage.Record;

namespace KMS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MemberController : ControllerBase
    {
        private readonly KioskManagementSystemContext _dbcontext;
        private IConfiguration _configuration;
        private readonly ExecuteQuery _exQuery;
        private readonly string _remoteApiUrl = "https://speedpos.facedb.test.verigram.cloud/facedb/insert";

        public MemberController(IConfiguration configuration, KioskManagementSystemContext _context, ExecuteQuery exQuery)
        {
            _dbcontext = _context;
            _configuration = configuration;
            _exQuery = exQuery;
        }

        [HttpGet]
        [Route("ShowMember")]
        public JsonResult GetMember()
        {
            ResponseDto response = new ResponseDto();
            try
            {
                string query = "select * from LMember";
                DataTable table = _exQuery.ExecuteRawQuery(query);
                return new JsonResult(table);
            }
            catch (Exception ex)
            {
                response.Code = -1;
                response.Message = ex.Message;
                response.Exception = ex.ToString();
                response.Data = null;
            }
            return new JsonResult(response);

        }

        [HttpGet]
        [Route("ShowMember/{id}")]
        public JsonResult GetMemberById(int id)
        {
            ResponseDto response = new ResponseDto();
            try
            {
                string query = "select * from LMember where id=@Id";

                SqlParameter parameter = new SqlParameter("@Id", id);
                DataTable table = _exQuery.ExecuteRawQuery(query, new[] { parameter });

                if (table.Rows.Count > 0)
                {
                    return new JsonResult(table);
                }
                else
                {
                    return new JsonResult("Member not found");
                }
            }
            catch (Exception ex)
            {
                response.Code = -1;
                response.Message = ex.Message;
                response.Exception = ex.ToString();
                response.Data = null;
            }
            return new JsonResult(response);

        }

        [HttpPut]
        [Route("UpdateMemberEmailAndPhone/{id}")]
        public JsonResult EditMember(int id, [FromBody] Lmember lmember)
        {
            ResponseDto response = new ResponseDto();
            try
            {
                string updateQuery = "UPDATE LMember " +
                                     "SET email = @Email, phone = @Phone, isActive = 1 " +
                                     "WHERE id = @Id";

                string insertQuery = @"IF NOT EXISTS (SELECT 1 FROM LAccount WHERE memberId = @MemberId)
                                        BEGIN
                                            INSERT INTO LAccount (memberId, balance, dateCreated, dateModified, isActive, status)
                                            VALUES (@MemberId, 0, GETDATE(), GETDATE(), 1, 1)
                                        END";

                SqlParameter[] updateParameters =
                {
                    new SqlParameter("@Id", id),
                    new SqlParameter("@Email", lmember.Email),
                    new SqlParameter("@Phone", lmember.Phone)
                };

                SqlParameter[] insertParameters =
                {
                    new SqlParameter("@MemberId", id)
                };

                _exQuery.ExecuteRawQuery(updateQuery, updateParameters);
                //_exQuery.ExecuteRawQuery(insertQuery, insertParameters);

                response.Code = 200;
                response.Message = "Member updated successfully";

            }
            catch (Exception ex)
            {
                response.Code = -1;
                response.Message = ex.Message;
                response.Exception = ex.ToString();
                response.Data = null;
            }
            return new JsonResult(response);
        }


        [HttpPost]
        [Route("GetMemberInformationFromScanner/{fileName}")]
        public JsonResult AddMember(string fileName)
        {
            string filePath = fileName;

            try
            {
                string jsonData = System.IO.File.ReadAllText(filePath);
                var jsonModel = JsonConvert.DeserializeObject<OcrModel>(jsonData);

                var cardInfo = jsonModel.cards.FirstOrDefault()?.info;
                string fullName = cardInfo.name;
                int spaceCount = fullName.Count(c => c == ' ');

                if (cardInfo != null)
                {
                    Lmember member = new Lmember
                    {
                        FirstName = cardInfo.name?.Split(' ')[0],
                        LastName = cardInfo.name?.Split(' ')[spaceCount],
                        FullName = cardInfo.name,
                        Gender = cardInfo.gender,
                        Birthday = DateTime.Parse(cardInfo.birthday),
                        IdenNumber = cardInfo.id,
                        Ward = cardInfo.address_split.ward,
                        District = cardInfo.address_split.district,
                        City = cardInfo.address_split.province,

                        Address1 = $"{cardInfo.address_split?.village}, {cardInfo.address_split?.ward}, {cardInfo.address_split?.district}, {cardInfo.address_split?.province}".TrimEnd(',', ' '),
                        Address2 = $"{cardInfo.domicile_split?.village}, {cardInfo.domicile_split?.ward}, {cardInfo.domicile_split?.district}, {cardInfo.domicile_split?.province}".TrimEnd(',', ' '),
                        // ... Map other properties as needed
                        IsActive = false, // Assuming default value for IsActive
                    };

                    // Now, insert the member into the database using your existing code
                    string query = "INSERT INTO LMember (gender, firstName, lastName, fullName, birthday, idenNumber, ward, district, city, address1, address2, " +
                                   "isActive, dateCreated, dateModified) " +
                                   "VALUES (@Gender, @FirstName, @LastName, @FullName, @Birthday, @IdenNumber, @Ward, @District, @City, @Address1, @Address2, " +
                                   "@IsActive, GETDATE(), GETDATE())";

                    SqlParameter[] parameters =
                    {
                        new SqlParameter("@Gender", member.Gender),
                        new SqlParameter("@FirstName", member.FirstName),
                        new SqlParameter("@LastName", member.LastName),
                        new SqlParameter("@FullName", member.FullName),
                        new SqlParameter("@Birthday", member.Birthday),

                        new SqlParameter("@IdenNumber", member.IdenNumber),
                        new SqlParameter("@Address1", member.Address1),
                        new SqlParameter("@Address2", member.Address2),
                        new SqlParameter("@District", member.District),
                        new SqlParameter("@City", member.City),
                        new SqlParameter("@Ward", member.Ward),

                        new SqlParameter("@IsActive", member.IsActive)
                    };

                    _exQuery.ExecuteRawQuery(query, parameters);

                    return new JsonResult("Member added successfully");
                }

                return new JsonResult("Invalid JSON data");
            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., file not found, invalid JSON format, etc.)
                return new JsonResult($"Error: {ex.Message}");
            }
        }

        [HttpPost]
        [Route("GetMemberInformationFromScanner2")]
        public async Task<JsonResult> AddMember2(IFormFile file, IFormFile imageIdCardFile)
        {

            try
            {
                using (StreamReader reader = new StreamReader(file.OpenReadStream()))
                {
                    string jsonData = reader.ReadToEnd();
                    var jsonModel = JsonConvert.DeserializeObject<OcrModel>(jsonData);

                    var cardInfo = jsonModel.cards.FirstOrDefault()?.info;
                    string fullName = cardInfo.name;
                    int spaceCount = fullName.Count(c => c == ' ');

                    if (cardInfo != null)
                    {
                        // Check if cardInfo.id already exists
                        List<string> existingIdenNumbers = IdenNumbersExist(cardInfo.id);

                        if (existingIdenNumbers.Count > 0)
                        {
                            // cardInfo.id already exists in the database
                            return new JsonResult(new ResponseDto
                            {
                                Code = 400,
                                Message = "IdenNumber already exists in the database"
                            });

                        }
                        else
                        {



                            Lmember member = new Lmember
                            {
                                FirstName = cardInfo.name?.Split(' ')[0],
                                LastName = cardInfo.name?.Split(' ')[spaceCount],
                                FullName = cardInfo.name,
                                Gender = cardInfo.gender,
                                Birthday = DateTime.ParseExact(cardInfo.birthday, "dd/MM/yyyy", CultureInfo.CurrentCulture),
                                IdenNumber = cardInfo.id,
                                Ward = cardInfo.address_split.ward,
                                District = cardInfo.address_split.district,
                                City = cardInfo.address_split.province,
                                ImageIdCard = imageIdCardFile?.FileName,
                                Address1 = $"{cardInfo.address_split?.village}, {cardInfo.address_split?.ward}, {cardInfo.address_split?.district}, {cardInfo.address_split?.province}".TrimEnd(',', ' '),
                                Address2 = $"{cardInfo.domicile_split?.village}, {cardInfo.domicile_split?.ward}, {cardInfo.domicile_split?.district}, {cardInfo.domicile_split?.province}".TrimEnd(',', ' '),
                                // ... Map other properties as needed
                                IsActive = false, // Assuming default value for IsActive
                            };



                            // Now, insert the member into the database using your existing code
                            string query = "INSERT INTO LMember (imageIdCard, gender, firstName, lastName, fullName, birthday, idenNumber, ward, district, city, address1, address2, " +
                                           "isActive, dateCreated, dateModified) " +
                                           "VALUES (@ImageIdCard, @Gender, @FirstName, @LastName, @FullName, @Birthday, @IdenNumber, @Ward, @District, @City, @Address1, @Address2, " +
                                           "@IsActive, GETDATE(), GETDATE())";

                            SqlParameter[] parameters =
                            {
                            new SqlParameter("@ImageIdCard", member.ImageIdCard),
                            new SqlParameter("@Gender", member.Gender),
                            new SqlParameter("@FirstName", member.FirstName),
                            new SqlParameter("@LastName", member.LastName),
                            new SqlParameter("@FullName", member.FullName),
                            new SqlParameter("@Birthday", member.Birthday),

                            new SqlParameter("@IdenNumber", member.IdenNumber),
                            new SqlParameter("@Address1", member.Address1),
                            new SqlParameter("@Address2", member.Address2),
                            new SqlParameter("@District", member.District),
                            new SqlParameter("@City", member.City),
                            new SqlParameter("@Ward", member.Ward),

                            new SqlParameter("@IsActive", member.IsActive)
                        };

                            _exQuery.ExecuteRawQuery(query, parameters);

                            int highestId = await _dbcontext.Lmembers.MaxAsync(m => m.Id);

                            string queryNew = $"INSERT INTO LAccount (memberId) VALUES ({highestId})";
                            _exQuery.ExecuteRawQuery(queryNew);


                            using (var client = new HttpClient())
                            {

                                MultipartFormDataContent content = new MultipartFormDataContent
                            {
                                { new StreamContent(imageIdCardFile.OpenReadStream()), "img_file", imageIdCardFile.FileName },
                                { new StringContent(highestId.ToString()), "person_id" },
                                { new StringContent(member.IdenNumber), "image_id" }
                            };

                                var response = await client.PostAsync(_remoteApiUrl, content);

                                if (response.IsSuccessStatusCode)
                                {
                                    return new JsonResult(new
                                    {
                                        Code = 200,
                                        Message = "Member added successfully",
                                        PersonId = highestId.ToString(),
                                        ImageId = member.IdenNumber,
                                        FullName = member.FullName,
                                        Birthday = member.Birthday,
                                        Gender = member.Gender,
                                        Address1 = member.Address1,
                                        IdenNumber = member.IdenNumber,
                                        Ward = member.Ward,
                                        District = member.District,
                                        City = member.City,
                                        FirstName = member.FirstName,
                                        LastName = member.LastName,
                                        Phone = member.Phone,
                                        Email = member.Email,
                                        TaxCode = member.TaxCode,
                                    });
                                }
                                else
                                {
                                    using (var client2 = new HttpClient())
                                    {

                                        MultipartFormDataContent content2 = new MultipartFormDataContent
                                    {
                                        { new StreamContent(imageIdCardFile.OpenReadStream()), "img_file", imageIdCardFile.FileName },
                                        { new StringContent(highestId.ToString()), "person_id" },
                                        { new StringContent(member.IdenNumber + GenerateRandomNumber()), "image_id" }
                                    };

                                        var response2 = await client.PostAsync(_remoteApiUrl, content2);

                                        if (response2.IsSuccessStatusCode)
                                        {
                                            return new JsonResult(new
                                            {
                                                Code = 200,
                                                Message = "Member added successfully",
                                                PersonId = highestId.ToString(),
                                                ImageId = member.IdenNumber + GenerateRandomNumber(),
                                                FullName = member.FullName,
                                                Birthday = member.Birthday,
                                                Gender = member.Gender,
                                                Address1 = member.Address1,
                                                IdenNumber = member.IdenNumber,
                                                Ward = member.Ward,
                                                District = member.District,
                                                City = member.City,
                                                FirstName = member.FirstName,
                                                LastName = member.LastName,
                                                Phone = member.Phone,
                                                Email = member.Email,
                                                TaxCode = member.TaxCode,
                                            });
                                        }
                                        else
                                        {
                                            return new JsonResult(new ResponseDto
                                            {
                                                Code = (int)response2.StatusCode,
                                                Message = "Failed to add."
                                            });
                                        }
                                    }
                                }
                            }

                        }


                    }

                    else
                    {
                        return new JsonResult(new ResponseDto
                        {
                            Code = 400,
                            Message = "Invalid JSON data"
                        });
                    }

                }
            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., file not found, invalid JSON format, etc.)
                return new JsonResult(new ResponseDto
                {
                    Code = 500,
                    Message = $"Error: {ex.Message}"
                });
            }
        }
        public class ResponseDtoResult
        {
            public int Code { get; set; } = 200;
            public string Message { get; set; } = "Add member successfully";
            public string PersonId { get; set; }
            public string ImageId { get; set; }
        }

        private string GenerateRandomNumber()
        {
            Random random = new Random();
            return random.Next(10, 99).ToString();
        }

        private List<string> IdenNumbersExist(string idenNumber)
        {
            string query = "SELECT idenNumber FROM LMember WHERE idenNumber = @IdenNumber";
            SqlParameter[] parameters =
            {
                new SqlParameter("@IdenNumber", idenNumber),
            };

            DataTable table = _exQuery.ExecuteRawQuery(query, parameters);

            List<string> existingIdenNumbers = new List<string>();

            foreach (DataRow row in table.Rows)
            {
                existingIdenNumbers.Add(row["idenNumber"].ToString());
            }

            return existingIdenNumbers;
        }



        [HttpPut]
        [Route("UpdateMember/{id}")]
        public JsonResult UpdateMember(int id, [FromBody] Lmember member)
        {
            ResponseDto response = new ResponseDto();
            try
            {
                StringBuilder queryBuilder = new StringBuilder("UPDATE LMember SET ");
                List<SqlParameter> parameters = new List<SqlParameter>();

                if (member.FirstName != null && member.FirstName != "string")
                {
                    queryBuilder.Append("FirstName = @FirstName, ");
                    parameters.Add(new SqlParameter("@FirstName", member.FirstName));
                }
                if (member.LastName != null && member.LastName != "string")
                {
                    queryBuilder.Append("LastName = @LastName, ");
                    parameters.Add(new SqlParameter("@LastName", member.LastName));
                }
                if (member.FullName != null && member.FullName != "string")
                {
                    queryBuilder.Append("FullName = @FullName, ");
                    parameters.Add(new SqlParameter("@FullName", member.FullName));
                }

                if (member.Phone != null && member.Phone != "string")
                {
                    queryBuilder.Append("Phone = @Phone, ");
                    parameters.Add(new SqlParameter("@Phone", member.Phone));
                }
                if (member.Address1 != null && member.Address1 != "string")
                {
                    queryBuilder.Append("Address1 = @Address1, ");
                    parameters.Add(new SqlParameter("@Address1", member.Address1));
                }
                if (member.Address2 != null && member.Address2 != "string")
                {
                    queryBuilder.Append("Address2 = @Address2, ");
                    parameters.Add(new SqlParameter("@Address2", member.Address2));
                }
                if (member.District != null && member.District != "string")
                {
                    queryBuilder.Append("District = @District, ");
                    parameters.Add(new SqlParameter("@District", member.District));
                }
                if (member.City != null && member.City != "string")
                {
                    queryBuilder.Append("City = @City, ");
                    parameters.Add(new SqlParameter("@City", member.City));
                }
                if (member.Ward != null && member.Ward != "string")
                {
                    queryBuilder.Append("Ward = @Ward, ");
                    parameters.Add(new SqlParameter("@Ward", member.Ward));
                }
                if (member.ImageIdCard != null && member.ImageIdCard != "string")
                {
                    queryBuilder.Append("ImageIdCard = @ImageIdCard, ");
                    parameters.Add(new SqlParameter("@ImageIdCard", member.ImageIdCard));
                }
                if (member.Fingerprint1 != null && member.Fingerprint1 != "string")
                {
                    queryBuilder.Append("Fingerprint1 = @Fingerprint1, ");
                    parameters.Add(new SqlParameter("@Fingerprint1", member.Fingerprint1));
                }
                if (member.Fingerprint2 != null && member.Fingerprint2 != "string")
                {
                    queryBuilder.Append("Fingerprint2 = @Fingerprint2, ");
                    parameters.Add(new SqlParameter("@Fingerprint2", member.Fingerprint2));
                }
                if (member.IdenNumber != null && member.IdenNumber != "string")
                {
                    queryBuilder.Append("IdenNumber = @IdenNumber, ");
                    parameters.Add(new SqlParameter("@IdenNumber", member.IdenNumber));
                }
                if (member.BankName != null && member.BankName != "string")
                {
                    queryBuilder.Append("BankName = @BankName, ");
                    parameters.Add(new SqlParameter("@BankName", member.BankName));
                }
                if (member.BankNumber != null && member.BankNumber != "string")
                {
                    queryBuilder.Append("BankNumber = @BankNumber, ");
                    parameters.Add(new SqlParameter("@BankNumber", member.BankNumber));
                }
                if (member.RefCode != null && member.RefCode != 0)
                {
                    queryBuilder.Append("RefCode = @RefCode, ");
                    parameters.Add(new SqlParameter("@RefCode", member.RefCode));
                }
                if (member.CompanyName != null && member.CompanyName != "string")
                {
                    queryBuilder.Append("CompanyName = @CompanyName, ");
                    parameters.Add(new SqlParameter("@CompanyName", member.CompanyName));
                }
                if (member.CompanyAddress != null && member.CompanyAddress != "string")
                {
                    queryBuilder.Append("CompanyAddress = @CompanyAddress, ");
                    parameters.Add(new SqlParameter("@CompanyAddress", member.CompanyAddress));
                }
                if (member.Department != null && member.Department != "string")
                {
                    queryBuilder.Append("Department = @Department, ");
                    parameters.Add(new SqlParameter("@Department", member.Department));
                }
                if (member.SalaryAmount != null && member.SalaryAmount != 0)
                {
                    queryBuilder.Append("SalaryAmount = @SalaryAmount, ");
                    parameters.Add(new SqlParameter("@SalaryAmount", member.SalaryAmount));
                }
                if (member.CreditLimit != null && member.CreditLimit != 0)
                {
                    queryBuilder.Append("CreditLimit = @CreditLimit, ");
                    parameters.Add(new SqlParameter("@CreditLimit", member.CreditLimit));
                }

                if (member.DebtStatus != null && member.DebtStatus != "string")
                {
                    queryBuilder.Append("DebtStatus = @DebtStatus, ");
                    parameters.Add(new SqlParameter("@DebtStatus", member.DebtStatus));
                }
                if (member.DateModified != null || member.DateModified == null)
                {
                    queryBuilder.Append("DateModified = GETDATE(), ");

                }

                if (member.IsActive != null)
                {
                    queryBuilder.Append("IsActive = @IsActive, ");
                    parameters.Add(new SqlParameter("@IsActive", member.IsActive));
                }
                if (member.Gender != null && member.Gender != "string")
                {
                    queryBuilder.Append("Gender = @Gender, ");
                    parameters.Add(new SqlParameter("@Gender", member.Gender));
                }
                if (member.ImageMember != null && member.ImageMember != "string")
                {
                    queryBuilder.Append("ImageMember = @ImageMember, ");
                    parameters.Add(new SqlParameter("@ImageMember", member.ImageMember));
                }
                if (member.Occupation != null && member.Occupation != "string")
                {
                    queryBuilder.Append("Occupation = @Occupation, ");
                    parameters.Add(new SqlParameter("@Occupation", member.Occupation));
                }
                if (member.Email != null && member.Email != "string")
                {
                    queryBuilder.Append("Email = @Email, ");
                    parameters.Add(new SqlParameter("@Email", member.Email));
                }
                if (member.TaxCode != null && member.TaxCode != "string")
                {
                    queryBuilder.Append("taxCode = @TaxCode, ");
                    parameters.Add(new SqlParameter("@TaxCode", member.TaxCode));
                }

                // Remove the trailing comma and space
                if (parameters.Count > 0)
                {
                    queryBuilder.Remove(queryBuilder.Length - 2, 2);

                    // Add the condition for updating a specific member by Id
                    queryBuilder.Append(" WHERE Id = @Id");
                    parameters.Add(new SqlParameter("@Id", id));

                    string query = queryBuilder.ToString();

                    _exQuery.ExecuteRawQuery(query, parameters.ToArray());

                    return new JsonResult("Member updated successfully");
                }
                else
                {
                    return new JsonResult("No properties provided for update");
                }
            }
            catch (Exception ex)
            {
                response.Code = -1;
                response.Message = ex.Message;
                response.Exception = ex.ToString();
                response.Data = null;
            }
            return new JsonResult(response);
        }




        [HttpGet]
        [Route("SearchMember")]
        public JsonResult SearchMember(string searchQuery)
        {
            ResponseDto response = new ResponseDto();
            try
            {
                string query = "SELECT id, phone, department, companyName, bankName, address1, isActive, dateCreated " +
                           "FROM LMember " +
                           "WHERE id LIKE @searchQuery OR " +
                           "phone LIKE @searchQuery OR " +
                           "department LIKE @searchQuery OR " +
                           "companyName LIKE @searchQuery OR " +
                           "bankName LIKE @searchQuery OR " +
                           "address1 LIKE @searchQuery";

                SqlParameter parameter = new SqlParameter("@searchQuery", "%" + searchQuery + "%");
                DataTable table = _exQuery.ExecuteRawQuery(query, new[] { parameter });

                return new JsonResult(table);
            }
            catch (Exception ex)
            {
                response.Code = -1;
                response.Message = ex.Message;
                response.Exception = ex.ToString();
                response.Data = null;
            }
            return new JsonResult(response);

        }

    }
}
