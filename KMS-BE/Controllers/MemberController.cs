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

namespace KMS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MemberController : ControllerBase
    {
        private readonly KioskManagementSystemContext _dbcontext;
        private IConfiguration _configuration;
        private readonly ExecuteQuery _exQuery;

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
            string query = "select * from LMember";
            DataTable table = _exQuery.ExecuteRawQuery(query);
            return new JsonResult(table);
        }

        [HttpGet]
        [Route("ShowMember/{id}")]
        public JsonResult GetMemberById(int id)
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

        [HttpPut]
        [Route("UpdateMemberEmailAndPhone/{id}")]
        public JsonResult EditMember(int id, [FromBody] Lmember lmember)
        {
            string query = "UPDATE LMember " +
                           "SET email = @Email, phone = @Phone " +
                           "WHERE id = @Id";

            SqlParameter[] parameters =
            {
                new SqlParameter("@Id", id),
                new SqlParameter("@Email", lmember.Email),
                new SqlParameter("@Phone", lmember.Phone),
                
            };


            _exQuery.ExecuteRawQuery(query, parameters);


            return new JsonResult("Member updated successfully");
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
                        IsActive = true, // Assuming default value for IsActive
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
        public JsonResult AddMember2(IFormFile file, IFormFile imageIdCardFile)
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
                            ImageIdCard = imageIdCardFile?.FileName,
                            Address1 = $"{cardInfo.address_split?.village}, {cardInfo.address_split?.ward}, {cardInfo.address_split?.district}, {cardInfo.address_split?.province}".TrimEnd(',', ' '),
                            Address2 = $"{cardInfo.domicile_split?.village}, {cardInfo.domicile_split?.ward}, {cardInfo.domicile_split?.district}, {cardInfo.domicile_split?.province}".TrimEnd(',', ' '),
                            // ... Map other properties as needed
                            IsActive = true, // Assuming default value for IsActive
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

                        return new JsonResult("Member added successfully");
                    }

                    return new JsonResult("Invalid JSON data");
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., file not found, invalid JSON format, etc.)
                return new JsonResult($"Error: {ex.Message}");
            }
        }




        [HttpPut]
        [Route("UpdateMember/{id}")]
        public JsonResult UpdateMember(int id, [FromBody] Lmember member)
        {
            string query = "UPDATE LMember SET phone=@Phone, department=@Department, companyName=@CompanyName, " +
                           "bankName=@BankName, address1=@Address1, dateModified = GETDATE(), isActive=@IsActive " +
                           "WHERE id=@Id";

            SqlParameter[] parameters =
            {
                new SqlParameter("@Id", id),
                new SqlParameter("@Phone", member.Phone),
                new SqlParameter("@Department", member.Department),
                new SqlParameter("@CompanyName", member.CompanyName),
                new SqlParameter("@BankName", member.BankName),
                new SqlParameter("@Address1", member.Address1),
                new SqlParameter("@IsActive", member.IsActive)
            };

            _exQuery.ExecuteRawQuery(query, parameters);

            return new JsonResult("Member updated successfully");
        }

        

        [HttpGet]
        [Route("SearchMember")]
        public JsonResult SearchMember(string searchQuery)
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

    }
}
