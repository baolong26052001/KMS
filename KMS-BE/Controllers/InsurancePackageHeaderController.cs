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
    public class InsurancePackageHeaderController : ControllerBase
    {
        private readonly KioskManagementSystemContext _dbcontext;
        private IConfiguration _configuration;
        private readonly ExecuteQuery _exQuery;

        public InsurancePackageHeaderController(IConfiguration configuration, KioskManagementSystemContext _context, ExecuteQuery exQuery)
        {
            _dbcontext = _context;
            _configuration = configuration;
            _exQuery = exQuery;
        }

        [HttpGet]
        [Route("ShowInsurancePackageHeader")]
        public JsonResult GetInsurancePackageHeader()
        {
            string query = "SELECT * FROM InsurancePackageHeader";

            DataTable table = _exQuery.ExecuteRawQuery(query);
            return new JsonResult(table);
        }

        [HttpGet]
        [Route("ShowInsurancePackageHeader/{id}")]
        public JsonResult GetInsurancePackageHeaderById(int id)
        {
            string query = "SELECT * FROM InsurancePackageHeader where id=@Id";

            SqlParameter parameter = new SqlParameter("@Id", id);
            DataTable table = _exQuery.ExecuteRawQuery(query, new[] { parameter });

            if (table.Rows.Count > 0)
            {
                return new JsonResult(table);
            }
            else
            {
                return new JsonResult("Insurance Package Header ID not found");
            }
        }

        [HttpPost]
        [Route("AddInsurancePackageHeader")]
        public JsonResult AddInsurancePackageHeader([FromBody] InsurancePackageHeader insurancePackageHeader)
        {
            string query = "INSERT INTO InsurancePackageHeader (packageName, insuranceTypeId, termId, insuranceProviderId, dateCreated, dateModified) " +
                           "VALUES (@PackageName, @InsuranceTypeId, @TermId, @InsuranceProviderId, GETDATE(), GETDATE())";

            SqlParameter[] parameters =
            {
                new SqlParameter("@PackageName", insurancePackageHeader.PackageName),
                new SqlParameter("@InsuranceTypeId", insurancePackageHeader.InsuranceTypeId),
                new SqlParameter("@TermId", insurancePackageHeader.TermId),
                new SqlParameter("@InsuranceProviderId", insurancePackageHeader.InsuranceProviderId),
            };

            _exQuery.ExecuteRawQuery(query, parameters);

            return new JsonResult("Insurance Package Header added successfully");
        }

        [HttpPut]
        [Route("EditInsurancePackageHeader/{id}")]
        public JsonResult EditInsurancePackageHeader(int id, [FromBody] InsurancePackageHeader insurancePackageHeader)
        {
            string query = "UPDATE InsurancePackageHeader " +
                           "SET packageName = @PackageName, insuranceTypeId = @InsuranceTypeId, termId = @TermId, insuranceProviderId = @InsuranceProviderId, dateModified = GETDATE() " +
                           "WHERE id = @Id";

            SqlParameter[] parameters =
            {
                new SqlParameter("@Id", id),
                new SqlParameter("@PackageName", insurancePackageHeader.PackageName),
                new SqlParameter("@InsuranceTypeId", insurancePackageHeader.InsuranceTypeId),
                new SqlParameter("@TermId", insurancePackageHeader.TermId),
                new SqlParameter("@InsuranceProviderId", insurancePackageHeader.InsuranceProviderId),
            };


            _exQuery.ExecuteRawQuery(query, parameters);


            return new JsonResult("Insurance Package Header updated successfully");
        }

        [HttpDelete]
        [Route("DeleteInsurancePackageHeader")]
        public JsonResult DeleteInsurancePackageHeader([FromBody] List<int> insurancePackageHeaderIds)
        {
            if (insurancePackageHeaderIds == null || insurancePackageHeaderIds.Count == 0)
            {
                return new JsonResult("No insurance package header IDs provided for deletion");
            }

            StringBuilder deleteQuery = new StringBuilder("DELETE FROM InsurancePackageHeader WHERE id IN (");
            

            List<SqlParameter> parameters = new List<SqlParameter>();
            
            for (int i = 0; i < insurancePackageHeaderIds.Count; i++)
            {
                string parameterName = "@InsurancePackageHeaderId" + i;
                deleteQuery.Append(parameterName);

                if (i < insurancePackageHeaderIds.Count - 1)
                {
                    deleteQuery.Append(", ");
                }

                parameters.Add(new SqlParameter(parameterName, insurancePackageHeaderIds[i]));
            }

            deleteQuery.Append(");");

            _exQuery.ExecuteRawQuery(deleteQuery.ToString(), parameters.ToArray());
            

            return new JsonResult("Insurance Package Header deleted successfully");
        }

    }
}
