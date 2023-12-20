using Microsoft.AspNetCore.Mvc;
using KMS.Models;
using KMS.Tools;
using System.Data;
using Microsoft.Data.SqlClient;

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
        [Route("ShowInsurancePackage")]
        public JsonResult GetInsurancePackage()
        {
            string query = "select ipack.id, ipack.packageName, itype.typeName as insuranceType, ipack.packageGroup, ipack.duration, ipack.payType, ipack.annualFee, ipack.dateModified, ipack.dateCreated " +
                "from InsurancePackage ipack, InsuranceType itype " +
                "where ipack.insuranceType = itype.id;";

            DataTable table = _exQuery.ExecuteRawQuery(query);
            return new JsonResult(table);
        }

        [HttpGet]
        [Route("ShowInsurancePackageDetail/{id}")]
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

    }
}