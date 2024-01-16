using Microsoft.AspNetCore.Mvc;
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
            string query = "SELECT * FROM InsurancePackageDetail";

            DataTable table = _exQuery.ExecuteRawQuery(query);
            return new JsonResult(table);
        }

        [HttpGet]
        [Route("ShowInsurancePackageDetail/{id}")]
        public JsonResult GetInsurancePackageDetailById(int id)
        {
            string query = "SELECT * FROM InsurancePackageDetail where id=@Id";

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
                new SqlParameter("@AgeRangeId", insurancePackageDetail.AgeRangeId),
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
                new SqlParameter("@AgeRangeId", insurancePackageDetail.AgeRangeId),
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

    }
}