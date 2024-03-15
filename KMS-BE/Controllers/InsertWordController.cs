using System;
using System.IO;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Mvc;
using KMS.Models;
using KMS.Tools;
using Microsoft.Extensions.Configuration;
using DocumentFormat.OpenXml;
using Microsoft.AspNetCore.Authorization;
using DocumentFormat.OpenXml.Office2010.Excel;

namespace KMS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InsertWordController : ControllerBase
    {
        private readonly KioskManagementSystemContext _dbcontext;
        private IConfiguration _configuration;
        private readonly ExecuteQuery _exQuery;

        public InsertWordController(IConfiguration configuration, KioskManagementSystemContext _context, ExecuteQuery exQuery)
        {
            _dbcontext = _context;
            _configuration = configuration;
            _exQuery = exQuery;
        }

        
        [HttpPost]
        [Route("AddBuyerInfoToWordFile/{id}")]
        public IActionResult AddBuyerInfoToWordFile(int id, [FromBody] List<BeneficiaryInfo> beneficiaryInfos)
        {
            ResponseDto response = new ResponseDto();
            try
            {
                
                var member = _dbcontext.Lmembers.FirstOrDefault(m => m.Id == id);
                if (member == null)
                {
                    // Handle case where member is not found
                    return new JsonResult("Member not found.");
                }
                
                string buyerName = member.FullName;
                string buyerCCCD = member.IdenNumber;
                DateTime birthday = DateTime.ParseExact(member.Birthday.ToString(), "dd-MMM-yy hh:mm:ss tt", System.Globalization.CultureInfo.InvariantCulture);
                string buyerDOB = birthday.ToString("dd-MM-yyyy");
                string buyerPhone = member.Phone;
                string buyerMail = member.Email;
                string buyerAddress = member.Address1;
                

                string sourceFilePath = @"a.docx";
                string targetFilePath = @"" + buyerCCCD + ".docx";

                // Clone the original document to create a new document
                using (WordprocessingDocument sourceDoc = WordprocessingDocument.Open(sourceFilePath, false))
                using (WordprocessingDocument targetDoc = WordprocessingDocument.Create(targetFilePath, WordprocessingDocumentType.Document))
                {
                    foreach (var part in sourceDoc.Parts)
                    {
                        targetDoc.AddPart(part.OpenXmlPart, part.RelationshipId);
                    }
                }

                // Open the new document for editing
                using (WordprocessingDocument doc = WordprocessingDocument.Open(targetFilePath, true))
                {
                    var textBoxes = doc.MainDocumentPart.Document.Body;
                    foreach (var textBox in textBoxes)
                    {
                        var textNodes = textBox.Descendants<Text>();
                        foreach (var node in textNodes)
                        {
                            Dictionary<string, string> replacements = new Dictionary<string, string>
                            {
                                { "buyerName", buyerName },
                                { "buyerCCCD", buyerCCCD },
                                { "buyerDOB", buyerDOB },
                                { "buyerPhone", buyerPhone },
                                { "buyerMail", buyerMail },
                                { "buyerAddress", buyerAddress },

                                

                                { "Date1", DateTime.Now.Day.ToString() },
                                { "Month1", DateTime.Now.Month.ToString() },
                                { "Year1", DateTime.Now.Year.ToString() },
                                { "Date2", DateTime.Now.AddYears(1).Day.ToString() },
                                { "Month2", DateTime.Now.AddYears(1).Month.ToString() },
                                { "Year2", DateTime.Now.AddYears(1).Year.ToString() },

                                
                                

                            };

                            


                            for (int i = 0; i < beneficiaryInfos.Count; i++)
                            {
                                

                                replacements.Add($"beneficiaryName{i + 1}", beneficiaryInfos[i].BeneficiaryName);
                                replacements.Add($"beneGender{i + 1}", beneficiaryInfos[i].Gender);
                                DateTime benebirthday = DateTime.ParseExact(beneficiaryInfos[i].Birthday.ToString(), "dd-MMM-yy hh:mm:ss tt", System.Globalization.CultureInfo.InvariantCulture);
                                replacements.Add($"beneficiaryDOB{i + 1}", benebirthday.ToString("dd-MM-yyyy"));
                                replacements.Add($"beneficiaryCCCD{i + 1}", beneficiaryInfos[i].BeneficiaryCCCD);
                                replacements.Add($"beneficiaryRelation{i + 1}", beneficiaryInfos[i].Relationship);
                            }

                            if (beneficiaryInfos.Count < 5)
                            {
                                int totalBeneficiaries = beneficiaryInfos.Count;
                                int startValue = totalBeneficiaries + 1;

                                for (int i = startValue; i <= 5; i++)
                                {
                                    replacements.Add($"beneficiaryName{i}", "");
                                    replacements.Add($"beneGender{i}", "");
                                    replacements.Add($"beneficiaryDOB{i}", "");
                                    replacements.Add($"beneficiaryCCCD{i}", "");
                                    replacements.Add($"beneficiaryRelation{i}", "");
                                }
                            }

                            foreach (var replacement in replacements)
                            {
                                if (node.Text.Contains(replacement.Key))
                                {
                                    node.Text = node.Text.Replace(replacement.Key, replacement.Value);
                                }
                            }

                        }
                    }
                }

                return new JsonResult("Text inserted success.");
                
            }
            catch (Exception ex)
            {
                response.Code = -1;
                response.Message = ex.Message;
                response.Exception = ex.ToString();
                response.Data = null;
                return new JsonResult(response);
            }
        }
    }
}
