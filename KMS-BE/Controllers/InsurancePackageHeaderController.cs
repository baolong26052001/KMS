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
            string query = "select b.id, b.packageName, c.provider, c.email, e.content, f.typeName, b.dateModified, b.dateCreated " +
                "from InsurancePackageHeader b, InsuranceProvider c, Term e, InsuranceType f " +
                "where c.id = b.insuranceProviderId and e.id = b.termId and b.insuranceTypeId = f.id";

            DataTable table = _exQuery.ExecuteRawQuery(query);
            return new JsonResult(table);
        }

        [HttpGet]
        [Route("ShowInsurancePackageHeader/{id}")]
        public JsonResult GetInsurancePackageHeaderById(int id)
        {
            string query = "select b.id, f.id as insuranceTypeId, e.id as termId, c.id as insuranceProviderId, b.packageName, c.provider, c.email, e.content, f.typeName, b.dateModified, b.dateCreated " +
                "from InsurancePackageHeader b, InsuranceProvider c, Term e, InsuranceType f " +
                "where c.id = b.insuranceProviderId and e.id = b.termId and b.insuranceTypeId = f.id and b.id=@Id";

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

        [HttpGet]
        [Route("SearchInsurancePackageHeader")]
        public JsonResult SearchInsurancePackageHeader(string searchQuery)
        {
            string query = "SELECT b.id, b.packageName, c.provider, c.email, e.content, f.typeName, b.dateModified, b.dateCreated " +
                           "FROM InsurancePackageHeader b " +
                           "INNER JOIN InsuranceProvider c ON c.id = b.insuranceProviderId " +
                           "INNER JOIN Term e ON e.id = b.termId " +
                           "INNER JOIN InsuranceType f ON b.insuranceTypeId = f.id " +
                           "WHERE b.id LIKE @searchQuery OR " +
                           "b.packageName LIKE @searchQuery OR " +
                           "c.provider LIKE @searchQuery OR " +
                           "c.email LIKE @searchQuery OR " +
                           "f.typeName LIKE @searchQuery OR " +
                           "e.content LIKE @searchQuery";

            SqlParameter parameter = new SqlParameter("@searchQuery", "%" + searchQuery + "%");
            DataTable table = _exQuery.ExecuteRawQuery(query, new[] { parameter });

            return new JsonResult(table);
        }

        [HttpGet]
        [Route("FilterInsurancePackageHeader")]
        public JsonResult FilterInsurancePackageHeader([FromQuery] DateTime? startDate = null, [FromQuery] DateTime? endDate = null)
        {
            string query = "SELECT b.id, b.packageName, c.provider, c.email, e.content, f.typeName, b.dateModified, b.dateCreated " +
                           "FROM InsurancePackageHeader b " +
                           "INNER JOIN InsuranceProvider c ON c.id = b.insuranceProviderId " +
                           "INNER JOIN Term e ON e.id = b.termId " +
                           "INNER JOIN InsuranceType f ON b.insuranceTypeId = f.id ";

            List <SqlParameter> parameters = new List<SqlParameter>();

            if (startDate.HasValue && endDate.HasValue)
            {

                startDate = startDate.Value.Date.AddHours(0).AddMinutes(0).AddSeconds(0);
                endDate = endDate.Value.Date.AddHours(23).AddMinutes(59).AddSeconds(59);

                query += (parameters.Count == 0 ? " WHERE " : " AND ") + "b.dateCreated >= @startDate AND b.dateCreated <= @endDate";
                parameters.Add(new SqlParameter("@startDate", startDate.Value));
                parameters.Add(new SqlParameter("@endDate", endDate.Value));
            }

            DataTable table = _exQuery.ExecuteRawQuery(query, parameters.ToArray());

            return new JsonResult(table);
        }

    }
}
