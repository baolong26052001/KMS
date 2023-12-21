﻿using Microsoft.AspNetCore.Mvc;
using KMS.Models;
using KMS.Tools;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Text;

namespace KMS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InsurancePackageController : ControllerBase
    {
        private readonly KioskManagementSystemContext _dbcontext;
        private IConfiguration _configuration;
        private readonly ExecuteQuery _exQuery;

        public InsurancePackageController(IConfiguration configuration, KioskManagementSystemContext _context, ExecuteQuery exQuery)
        {
            _dbcontext = _context;
            _configuration = configuration;
            _exQuery = exQuery;
        }

        [HttpGet]
        [Route("ShowInsurancePackage")] // show ra có bao nhiêu package (packageA,packageB,packageC,...)
        public JsonResult GetInsurancePackage()
        {
            string query = "select ipack.id, ipack.packageName, itype.typeName as insuranceType, ipack.packageGroup, ipack.duration, ipack.payType, ipack.annualFee, ipack.dateModified, ipack.dateCreated " +
                "from InsurancePackage ipack, InsuranceType itype " +
                "where ipack.insuranceType = itype.id;";

            DataTable table = _exQuery.ExecuteRawQuery(query);
            return new JsonResult(table);
        }

        [HttpGet]
        [Route("ShowInsurancePackageDetail/{id}")] // khi ấn vào view package A, thì sẽ show ra benefit của package A
        public JsonResult GetInsurancePackageDetail(int id)
        {
            string query = "select b.id, b.content, b.coverage, b.description, ipack.packageName, itype.typeName, ipack.annualFee, b.dateModified, b.dateCreated " +
                "from Benefit b, InsurancePackage ipack, InsuranceType itype " +
                "where b.packageId = ipack.id and itype.id = ipack.insuranceType and ipack.id=@id";

            SqlParameter parameter = new SqlParameter("@id", id);
            DataTable table = _exQuery.ExecuteRawQuery(query, new[] { parameter });

            if (table.Rows.Count > 0)
            {
                return new JsonResult(table);
            }
            else
            {
                return new JsonResult("Insurance Package Detail not found");
            }
        }

        [HttpGet]
        [Route("ShowBenefit/{id}")] // ấn vào view benefit, sẽ ra benefit detail
        public JsonResult GetBenefitDetail(int id)
        {
            string query = "select * " +
                "from BenefitDetail " +
                "where benefitId=@id";

            SqlParameter parameter = new SqlParameter("@id", id);
            DataTable table = _exQuery.ExecuteRawQuery(query, new[] { parameter });

            if (table.Rows.Count > 0)
            {
                return new JsonResult(table);
            }
            else
            {
                return new JsonResult("Benefit Detail not found");
            }
        }

        [HttpPost]
        [Route("AddInsurancePackage")]
        public JsonResult AddInsurancePackage([FromBody] InsurancePackage insurancePackage)
        {
            string query = "INSERT INTO InsurancePackage (packageName, insuranceType, duration, payType, annualFee, dateModified, dateCreated) " +
                           "VALUES (@PackageName, @InsuranceType, @Duration, @PayType, @AnnualFee, GETDATE(), GETDATE())";

            SqlParameter[] parameters =
            {
                new SqlParameter("@PackageName", insurancePackage.PackageName),
                new SqlParameter("@InsuranceType", insurancePackage.InsuranceType),
                new SqlParameter("@Duration", insurancePackage.Duration),
                new SqlParameter("@PayType", insurancePackage.PayType),
                new SqlParameter("@AnnualFee", insurancePackage.AnnualFee),
            };

            _exQuery.ExecuteRawQuery(query, parameters);

            return new JsonResult("Insurance Package added successfully");
        }

        [HttpPut]
        [Route("EditInsurancePackage/{id}")]
        public JsonResult EditInsurancePackage(int id, [FromBody] InsurancePackage insurancePackage)
        {
            string query = "UPDATE InsurancePackage " +
                           "SET packageName = @PackageName, insuranceType = @InsuranceType, duration = @Duration, " +
                           "payType = @PayType, annualFee = @AnnualFee, dateModified = GETDATE() " +
                           "WHERE id = @id";

            SqlParameter[] parameters =
            {
                new SqlParameter("@id", id),
                new SqlParameter("@PackageName", insurancePackage.PackageName),
                new SqlParameter("@InsuranceType", insurancePackage.InsuranceType),
                new SqlParameter("@Duration", insurancePackage.Duration),
                new SqlParameter("@PayType", insurancePackage.PayType), // đóng tiền theo năm hoặc tháng
                new SqlParameter("@AnnualFee", insurancePackage.AnnualFee),
                
            };

            _exQuery.ExecuteRawQuery(query, parameters);

            return new JsonResult("Insurance Package updated successfully");
        }

        [HttpDelete]
        [Route("DeleteInsurancePackage")]
        public JsonResult DeleteInsurancePackage([FromBody] List<int> insurancePackageIds)
        {
            if (insurancePackageIds == null || insurancePackageIds.Count == 0)
            {
                return new JsonResult("No insurance package IDs provided for deletion");
            }

            StringBuilder deleteQuery = new StringBuilder("DELETE FROM InsurancePackage WHERE id IN (");


            List<SqlParameter> parameters = new List<SqlParameter>();
            for (int i = 0; i < insurancePackageIds.Count; i++)
            {
                string parameterName = "@InsurancePackageId" + i;
                deleteQuery.Append(parameterName);

                if (i < insurancePackageIds.Count - 1)
                {
                    deleteQuery.Append(", ");
                }

                parameters.Add(new SqlParameter(parameterName, insurancePackageIds[i]));
            }

            deleteQuery.Append(");");

            _exQuery.ExecuteRawQuery(deleteQuery.ToString(), parameters.ToArray());

            return new JsonResult("Insurance Package deleted successfully");
        }

        [HttpGet]
        [Route("SearchInsurancePackage")]
        public JsonResult SearchInsurancePackage(string searchQuery)
        {
            string query = "SELECT ipack.id, ipack.packageName, itype.typeName AS insuranceType, ipack.packageGroup, ipack.duration, ipack.payType, ipack.annualFee, ipack.dateModified, ipack.dateCreated " +
               "FROM InsurancePackage ipack " +
               "JOIN InsuranceType itype ON ipack.insuranceType = itype.id " +
               "WHERE ipack.id LIKE @searchQuery OR " +
               "ipack.packageName LIKE @searchQuery OR " +
               "itype.typeName LIKE @searchQuery OR " +
               "ipack.duration LIKE @searchQuery OR " +
               "ipack.payType LIKE @searchQuery OR " +
               "ipack.annualFee LIKE @searchQuery";



            

            SqlParameter parameter = new SqlParameter("@searchQuery", "%" + searchQuery + "%");
            DataTable table = _exQuery.ExecuteRawQuery(query, new[] { parameter });

            return new JsonResult(table);
        }

        [HttpGet]
        [Route("FilterInsurancePackage")]
        public JsonResult FilterInsurancePackage([FromQuery] DateTime? startDate = null, [FromQuery] DateTime? endDate = null)
        {
            string query = "SELECT ipack.id, ipack.packageName, itype.typeName AS insuranceType, ipack.packageGroup, ipack.duration, ipack.payType, ipack.annualFee, ipack.dateModified, ipack.dateCreated " +
               "FROM InsurancePackage ipack " +
               "JOIN InsuranceType itype ON ipack.insuranceType = itype.id ";

            List <SqlParameter> parameters = new List<SqlParameter>();



            

            if (startDate.HasValue && endDate.HasValue)
            {

                startDate = startDate.Value.Date.AddHours(0).AddMinutes(0).AddSeconds(0);
                endDate = endDate.Value.Date.AddHours(23).AddMinutes(59).AddSeconds(59);

                query += (parameters.Count == 0 ? " WHERE " : " AND ") + "ipack.dateCreated >= @startDate AND ipack.dateCreated <= @endDate";
                parameters.Add(new SqlParameter("@startDate", startDate.Value));
                parameters.Add(new SqlParameter("@endDate", endDate.Value));
            }

            DataTable table = _exQuery.ExecuteRawQuery(query, parameters.ToArray());

            return new JsonResult(table);
        }


    }
}