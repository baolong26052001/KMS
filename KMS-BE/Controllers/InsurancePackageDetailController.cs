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
    public class InsurancePackageDetailController : ControllerBase
    {
        private readonly KioskManagementSystemContext _dbcontext;
        private IConfiguration _configuration;
        private readonly ExecuteQuery _exQuery;

        public InsurancePackageDetailController(IConfiguration configuration, KioskManagementSystemContext _context, ExecuteQuery exQuery)
        {
            _dbcontext = _context;
            _configuration = configuration;
            _exQuery = exQuery;
        }

        [HttpGet]
        [Route("ShowInsurancePackageDetail")]
        public JsonResult GetInsurancePackageDetail()
        {
            string query = "SELECT a.id, b.packageName, a.ageRangeId, N'Từ ' + CAST(d.startAge AS NVARCHAR) + N' đến ' + CAST(d.endAge AS NVARCHAR) + N' tuổi' AS ageRange, a.fee, c.provider, c.email, e.content, a.dateModified, a.dateCreated " +
                "FROM InsurancePackageDetail a " +
                "JOIN InsurancePackageHeader b ON a.packageHeaderId = b.id " +
                "LEFT JOIN InsuranceProvider c ON c.id = b.insuranceProviderId " +
                "LEFT JOIN AgeRange d ON d.id = a.ageRangeId " +
                "LEFT JOIN Term e ON e.id = b.termId";

            DataTable table = _exQuery.ExecuteRawQuery(query);
            return new JsonResult(table);
        }

        [HttpGet]
        [Route("ShowInsurancePackageDetailByAgeRangeId/{id}")]
        public JsonResult GetInsurancePackageDetailByAgeRangeId(int id)
        {
            string query = "SELECT\r\n    a.id,\r\n\tb.id as headerId,\r\n\te.id as termId,\r\n    b.packageName,\r\n    a.ageRangeId,\r\n    N'Từ ' + CAST(d.startAge AS NVARCHAR) + N' đến ' + CAST(d.endAge AS NVARCHAR) + N' tuổi' AS ageRange,\r\n    a.fee,\r\n    c.provider,\r\n    c.email,\r\n    e.content,\r\n    a.dateModified,\r\n    a.dateCreated\r\nFROM\r\n    InsurancePackageDetail a\r\nJOIN\r\n    AgeRange h ON h.id = a.ageRangeId\r\nJOIN\r\n    InsurancePackageHeader b ON a.packageHeaderId = b.id\r\nLEFT JOIN\r\n    InsuranceProvider c ON c.id = b.insuranceProviderId\r\nLEFT JOIN\r\n    AgeRange d ON d.id = a.ageRangeId\r\nLEFT JOIN\r\n    Term e ON e.id = b.termId\r\nwhere d.id = @Id";

            SqlParameter parameter = new SqlParameter("@Id", id);
            DataTable table = _exQuery.ExecuteRawQuery(query, new[] { parameter });

            if (table.Rows.Count > 0)
            {
                return new JsonResult(table);
            }
            else
            {
                return new JsonResult("Age Range ID not found");
            }
        }

        [HttpGet]
        [Route("ShowInsurancePackageDetailByAgeRangeIdAndTermId/{ageRangeId}/{termId}")]
        public JsonResult GetInsurancePackageDetailByAgeRangeIdAndTermId(int ageRangeId, int termId)
        {
            string query = "SELECT\r\n    a.id,\r\n    b.id AS headerId,\r\n    e.id AS termId,\r\n    b.packageName,\r\n    a.ageRangeId,\r\n    N'Từ ' + CAST(d.startAge AS NVARCHAR) + N' đến ' + CAST(d.endAge AS NVARCHAR) + N' tuổi' AS ageRange,\r\n    a.fee,\r\n    c.provider,\r\n    c.email,\r\n    e.content,\r\n\tb.priority,\r\n    a.dateModified,\r\n    a.dateCreated\r\nFROM\r\n    InsurancePackageDetail a\r\nLEFT JOIN\r\n    AgeRange h ON h.id = a.ageRangeId\r\nLEFT JOIN\r\n    InsurancePackageHeader b ON a.packageHeaderId = b.id\r\nLEFT JOIN\r\n    InsuranceProvider c ON c.id = b.insuranceProviderId\r\nLEFT JOIN\r\n    AgeRange d ON d.id = a.ageRangeId\r\nLEFT JOIN\r\n    Term e ON e.id = b.termId\r\nWHERE\r\n    d.id = @AgeRangeId\r\n    AND e.id = @TermId\r\n\tORDER BY b.priority asc";

            SqlParameter[] parameters = 
            {
                new SqlParameter("@AgeRangeId", ageRangeId),
                new SqlParameter("@TermId", termId)
            };

            DataTable table = _exQuery.ExecuteRawQuery(query, parameters);

            if (table.Rows.Count > 0)
            {
                return new JsonResult(table);
            }
            else
            {
                return new JsonResult("Age Range ID or term ID not found");
            }
        }


        [HttpGet]
        [Route("ShowInsurancePackageDetailByTermId/{id}")]
        public JsonResult GetInsurancePackageDetailByTermId(int id)
        {
            string query = "SELECT\r\n    a.id,\r\n\tb.id as headerId,\r\n\te.id as termId,\r\n    b.packageName,\r\n    a.ageRangeId,\r\n    N'Từ ' + CAST(d.startAge AS NVARCHAR) + N' đến ' + CAST(d.endAge AS NVARCHAR) + N' tuổi' AS ageRange,\r\n    a.fee,\r\n    c.provider,\r\n    c.email,\r\n    e.content,\r\n    a.dateModified,\r\n    a.dateCreated\r\nFROM\r\n    InsurancePackageDetail a\r\nJOIN\r\n    AgeRange h ON h.id = a.ageRangeId\r\nJOIN\r\n    InsurancePackageHeader b ON a.packageHeaderId = b.id\r\nLEFT JOIN\r\n    InsuranceProvider c ON c.id = b.insuranceProviderId\r\nLEFT JOIN\r\n    AgeRange d ON d.id = a.ageRangeId\r\nLEFT JOIN\r\n    Term e ON e.id = b.termId\r\nwhere e.id = @Id";

            SqlParameter parameter = new SqlParameter("@Id", id);
            DataTable table = _exQuery.ExecuteRawQuery(query, new[] { parameter });

            if (table.Rows.Count > 0)
            {
                return new JsonResult(table);
            }
            else
            {
                return new JsonResult("Term ID not found");
            }
        }

        [HttpGet]
        [Route("ShowInsurancePackageDetailByHeaderId/{id}")]
        public JsonResult GetInsurancePackageDetailByHeaderId(int id)
        {
            string query = "SELECT a.id, b.packageName, a.ageRangeId, N'Từ ' + CAST(d.startAge AS NVARCHAR) + N' đến ' + CAST(d.endAge AS NVARCHAR) + N' tuổi' AS ageRange, a.fee, c.provider, c.email, e.content, a.dateModified, a.dateCreated " +
                "FROM InsurancePackageDetail a " +
                "JOIN InsurancePackageHeader b ON a.packageHeaderId = b.id " +
                "LEFT JOIN InsuranceProvider c ON c.id = b.insuranceProviderId " +
                "LEFT JOIN AgeRange d ON d.id = a.ageRangeId " +
                "LEFT JOIN Term e ON e.id = b.termId where b.id=@Id";

            SqlParameter parameter = new SqlParameter("@Id", id);
            DataTable table = _exQuery.ExecuteRawQuery(query, new[] { parameter });

            if (table.Rows.Count > 0)
            {
                return new JsonResult(table);
            }
            else
            {
                return new JsonResult("Insurance Package Detail ID not found");
            }
        }

        [HttpGet]
        [Route("ShowInsurancePackageDetail/{id}")]
        public JsonResult GetInsurancePackageDetailById(int id)
        {
            string query = "SELECT a.id, b.packageName, a.ageRangeId, N'Từ ' + CAST(d.startAge AS NVARCHAR) + N' đến ' + CAST(d.endAge AS NVARCHAR) + N' tuổi' AS ageRange, a.fee, c.provider, c.email, e.content, a.dateModified, a.dateCreated " +
                "FROM InsurancePackageDetail a " +
                "JOIN InsurancePackageHeader b ON a.packageHeaderId = b.id " +
                "LEFT JOIN InsuranceProvider c ON c.id = b.insuranceProviderId " +
                "LEFT JOIN AgeRange d ON d.id = a.ageRangeId " +
                "LEFT JOIN Term e ON e.id = b.termId where a.id=@Id";

            SqlParameter parameter = new SqlParameter("@Id", id);
            DataTable table = _exQuery.ExecuteRawQuery(query, new[] { parameter });

            if (table.Rows.Count > 0)
            {
                return new JsonResult(table);
            }
            else
            {
                return new JsonResult("Insurance Package Detail ID not found");
            }
        }

        [HttpPost]
        [Route("AddInsurancePackageDetail")]
        public JsonResult AddInsurancePackageDetail([FromBody] InsurancePackageDetail insurancePackageDetail)
        {
            string query = "INSERT INTO InsurancePackageDetail (packageHeaderId, ageRangeId, fee, dateCreated, dateModified) " +
                           "VALUES (@PackageHeaderId, @AgeRangeId, @Fee, GETDATE(), GETDATE())";

            SqlParameter[] parameters =
            {
                new SqlParameter("@PackageHeaderId", insurancePackageDetail.PackageHeaderId),
                new SqlParameter("@AgeRangeId", (object)insurancePackageDetail.AgeRangeId ?? DBNull.Value),
                new SqlParameter("@Fee", insurancePackageDetail.Fee),
            };

            _exQuery.ExecuteRawQuery(query, parameters);

            return new JsonResult("Insurance Package Detail added successfully");
        }

        [HttpPut]
        [Route("EditInsurancePackageDetail/{id}")]
        public JsonResult EditInsurancePackageDetail(int id, [FromBody] InsurancePackageDetail insurancePackageDetail)
        {
            string query = "UPDATE InsurancePackageDetail " +
                           "SET packageHeaderId = @PackageHeaderId, ageRangeId = @AgeRangeId, fee = @Fee, dateModified = GETDATE() " +
                           "WHERE id = @Id";

            SqlParameter[] parameters =
            {
                new SqlParameter("@Id", id),
                new SqlParameter("@PackageHeaderId", insurancePackageDetail.PackageHeaderId),
                new SqlParameter("@AgeRangeId", (object)insurancePackageDetail.AgeRangeId ?? DBNull.Value),
                new SqlParameter("@Fee", insurancePackageDetail.Fee),
            };


            _exQuery.ExecuteRawQuery(query, parameters);


            return new JsonResult("Insurance Package Detail updated successfully");
        }

        [HttpDelete]
        [Route("DeleteInsurancePackageDetail")]
        public JsonResult DeleteInsurancePackageDetail([FromBody] List<int> insurancePackageDetailIds)
        {
            if (insurancePackageDetailIds == null || insurancePackageDetailIds.Count == 0)
            {
                return new JsonResult("No insurance package detail IDs provided for deletion");
            }

            StringBuilder deleteQuery = new StringBuilder("DELETE FROM InsurancePackageDetail WHERE id IN (");


            List<SqlParameter> parameters = new List<SqlParameter>();

            for (int i = 0; i < insurancePackageDetailIds.Count; i++)
            {
                string parameterName = "@InsurancePackageDetailId" + i;
                deleteQuery.Append(parameterName);

                if (i < insurancePackageDetailIds.Count - 1)
                {
                    deleteQuery.Append(", ");
                }

                parameters.Add(new SqlParameter(parameterName, insurancePackageDetailIds[i]));
            }

            deleteQuery.Append(");");

            _exQuery.ExecuteRawQuery(deleteQuery.ToString(), parameters.ToArray());


            return new JsonResult("Insurance Package Detail deleted successfully");
        }

        [HttpGet]
        [Route("SearchInsurancePackageDetail")]
        public JsonResult SearchInsurancePackageDetail(string searchQuery)
        {
            string query = "SELECT a.id, b.packageName, N'Từ ' + CAST(d.startAge AS NVARCHAR) + N' đến ' + CAST(d.endAge AS NVARCHAR) + N' tuổi' AS ageRange, a.fee, c.provider, c.email, e.content, a.dateModified, a.dateCreated " +
                           "FROM InsurancePackageDetail a " +
                           "LEFT JOIN InsurancePackageHeader b ON a.packageHeaderId = b.id " +
                           "LEFT JOIN InsuranceProvider c ON c.id = b.insuranceProviderId " +
                           "LEFT JOIN AgeRange d ON d.id = a.ageRangeId " +
                           "LEFT JOIN Term e ON e.id = b.termId " +
                           "WHERE a.id LIKE @searchQuery OR " +
                           "b.packageName LIKE @searchQuery OR " +
                           "c.provider LIKE @searchQuery OR " +
                           "CONVERT(VARCHAR(20), a.fee) LIKE @searchQuery OR " +
                           "c.email LIKE @searchQuery OR " +
                           "e.content LIKE @searchQuery";

            SqlParameter parameter = new SqlParameter("@searchQuery", "%" + searchQuery + "%");
            DataTable table = _exQuery.ExecuteRawQuery(query, new[] { parameter });

            return new JsonResult(table);
        }

        [HttpGet]
        [Route("FilterInsurancePackageDetail")]
        public JsonResult FilterInsurancePackageDetail([FromQuery] DateTime? startDate = null, [FromQuery] DateTime? endDate = null)
        {
            string query = "SELECT a.id, b.packageName, N'Từ ' + CAST(d.startAge AS NVARCHAR) + N' đến ' + CAST(d.endAge AS NVARCHAR) + N' tuổi' AS ageRange, a.fee, c.provider, c.email, e.content, a.dateModified, a.dateCreated " +
                           "FROM InsurancePackageDetail a " +
                           "LEFT JOIN InsurancePackageHeader b ON a.packageHeaderId = b.id " +
                           "LEFT JOIN InsuranceProvider c ON c.id = b.insuranceProviderId " +
                           "LEFT JOIN AgeRange d ON d.id = a.ageRangeId " +
                           "LEFT JOIN Term e ON e.id = b.termId ";

            List <SqlParameter> parameters = new List<SqlParameter>();

            if (startDate.HasValue && endDate.HasValue)
            {

                startDate = startDate.Value.Date.AddHours(0).AddMinutes(0).AddSeconds(0);
                endDate = endDate.Value.Date.AddHours(23).AddMinutes(59).AddSeconds(59);

                query += (parameters.Count == 0 ? " WHERE " : " AND ") + "a.dateCreated >= @startDate AND a.dateCreated <= @endDate";
                parameters.Add(new SqlParameter("@startDate", startDate.Value));
                parameters.Add(new SqlParameter("@endDate", endDate.Value));
            }

            DataTable table = _exQuery.ExecuteRawQuery(query, parameters.ToArray());

            return new JsonResult(table);
        }

    }
}
