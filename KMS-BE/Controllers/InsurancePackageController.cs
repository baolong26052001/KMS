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
        [Route("ShowBenefitById/{id}")] // (KMS) hiện thông tin khi ở màn hình edit benefit
        public JsonResult GetBenefitById(int id)
        {
            string query = "select * " +
                "from Benefit " +
                "where id=@id";

            SqlParameter parameter = new SqlParameter("@id", id);
            DataTable table = _exQuery.ExecuteRawQuery(query, new[] { parameter });

            if (table.Rows.Count > 0)
            {
                return new JsonResult(table);
            }
            else
            {
                return new JsonResult("Benefit not found");
            }
        }

        [HttpGet]
        [Route("ShowBenefitDetailById/{id}")] // (KMS) hiện thông tin khi ở màn hình edit benefit detail
        public JsonResult GetBenefitDetailById(int id)
        {
            string query = "select * " +
                "from BenefitDetail " +
                "where id=@id";

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

        [HttpGet]
        [Route("ShowInsurancePackageDetail/{id}")] // (KMS) khi ấn vào view package header A, thì sẽ show ra "benefit" của package header A
                                                   // (Kiosk App) hiện ra benefit của package header A
        public JsonResult GetInsurancePackageDetail(int id)
        {
            string query = "select b.id, b.content, b.coverage, b.description, ipack.packageName, itype.typeName, " +
                "b.dateModified, b.dateCreated " +
                "from Benefit b " +
                "LEFT JOIN InsurancePackageHeader ipack ON b.packageId = ipack.id " +
                "LEFT JOIN InsuranceType itype ON itype.id = ipack.insuranceTypeId " +
                "where ipack.id=@id";

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
            string query = "select id,content,coverage,benefitId,dateCreated,dateModified\r\nfrom BenefitDetail " +
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
        [Route("AddBenefit")]
        public JsonResult AddBenefit([FromBody] Benefit benefit)
        {
            string query = "INSERT INTO Benefit (packageId, content, coverage, description, dateModified, dateCreated) " +
                           "VALUES (@PackageId, @Content, @Coverage, @Description, GETDATE(), GETDATE())";
            string query2 = "INSERT INTO TAudit (action, tableName, dateModified, dateCreated, isActive) VALUES ('Add', 'Benefit', GETDATE(), GETDATE(), 1)";

            SqlParameter[] parameters =
            {
                new SqlParameter("@PackageId", benefit.PackageId),
                new SqlParameter("@Content", benefit.Content),
                new SqlParameter("@Coverage", benefit.Coverage),
                new SqlParameter("@Description", benefit.Description),
 
                
            };
            SqlParameter[] parameters2 = { };

            _exQuery.ExecuteRawQuery(query, parameters);
            _exQuery.ExecuteRawQuery(query2, parameters2);

            return new JsonResult("Benefit added successfully");
        }

        [HttpPost]
        [Route("AddBenefitDetail")]
        public JsonResult AddBenefitDetail([FromBody] BenefitDetail benefitDetail)
        {
            string query = "INSERT INTO BenefitDetail (benefitId, content, coverage, dateModified, dateCreated) " +
                           "VALUES (@BenefitId, @Content, @Coverage, GETDATE(), GETDATE())";
            string query2 = "INSERT INTO TAudit (action, tableName, dateModified, dateCreated, isActive) VALUES ('Add', 'BenefitDetail', GETDATE(), GETDATE(), 1)";
            SqlParameter[] parameters =
            {
                
                new SqlParameter("@BenefitId", benefitDetail.BenefitId),
                new SqlParameter("@Content", benefitDetail.Content),
                new SqlParameter("@Coverage", benefitDetail.Coverage),

            };
            SqlParameter[] parameters2 = { };

            _exQuery.ExecuteRawQuery(query, parameters);
            _exQuery.ExecuteRawQuery(query2, parameters2);

            return new JsonResult("Benefit detail added successfully");
        }

        [HttpPut]
        [Route("EditBenefit/{id}")]
        public JsonResult EditBenefit(int id, [FromBody] Benefit benefit)
        {
            string query = "UPDATE Benefit " +
                           "SET content = @Content, coverage = @Coverage, description = @Description, " +
                           "dateModified = GETDATE() " +
                           "WHERE id = @id";
            string query2 = "INSERT INTO TAudit (action, tableName, dateModified, dateCreated, isActive) VALUES ('Edit', 'Benefit', GETDATE(), GETDATE(), 1)";

            SqlParameter[] parameters =
            {
                new SqlParameter("@id", id),
                new SqlParameter("@Content", benefit.Content),
                new SqlParameter("@Coverage", benefit.Coverage),
                new SqlParameter("@Description", benefit.Description),
                

            };
            SqlParameter[] parameters2 = { };

            _exQuery.ExecuteRawQuery(query, parameters);
            _exQuery.ExecuteRawQuery(query2, parameters2);

            return new JsonResult("Benefit updated successfully");
        }

        [HttpPut]
        [Route("EditBenefitDetail/{id}")]
        public JsonResult EditBenefitDetail(int id, [FromBody] BenefitDetail benefitDetail)
        {
            string query = "UPDATE BenefitDetail " +
                           "SET content = @Content, coverage = @Coverage, " +
                           "dateModified = GETDATE() " +
                           "WHERE id = @id";
            string query2 = "INSERT INTO TAudit (action, tableName, dateModified, dateCreated, isActive) VALUES ('Edit', 'BenefitDetail', GETDATE(), GETDATE(), 1)";

            SqlParameter[] parameters =
            {
                new SqlParameter("@id", id),
                new SqlParameter("@Content", benefitDetail.Content),
                new SqlParameter("@Coverage", benefitDetail.Coverage),
            };
            SqlParameter[] parameters2 = { };

            _exQuery.ExecuteRawQuery(query, parameters);
            _exQuery.ExecuteRawQuery(query2, parameters2);

            return new JsonResult("Benefit detail updated successfully");
        }

        [HttpDelete]
        [Route("DeleteBenefit")]
        public JsonResult DeleteBenefit([FromBody] List<int> benefitIds)
        {
            if (benefitIds == null || benefitIds.Count == 0)
            {
                return new JsonResult("No benefit IDs provided for deletion");
            }

            StringBuilder deleteQuery = new StringBuilder("DELETE FROM Benefit WHERE id IN (");
            string query2 = "INSERT INTO TAudit (action, tableName, dateModified, dateCreated, isActive) VALUES ('Delete', 'Benefit', GETDATE(), GETDATE(), 1)";

            List<SqlParameter> parameters = new List<SqlParameter>();
            SqlParameter[] parameters2 = { };
            for (int i = 0; i < benefitIds.Count; i++)
            {
                string parameterName = "@BenefitId" + i;
                deleteQuery.Append(parameterName);

                if (i < benefitIds.Count - 1)
                {
                    deleteQuery.Append(", ");
                }

                parameters.Add(new SqlParameter(parameterName, benefitIds[i]));
            }

            deleteQuery.Append(");");

            _exQuery.ExecuteRawQuery(deleteQuery.ToString(), parameters.ToArray());
            _exQuery.ExecuteRawQuery(query2, parameters2);

            return new JsonResult("Benefit deleted successfully");
        }

        [HttpDelete]
        [Route("DeleteBenefitDetail")]
        public JsonResult DeleteBenefitDetail([FromBody] List<int> benefitDetailIds)
        {
            if (benefitDetailIds == null || benefitDetailIds.Count == 0)
            {
                return new JsonResult("No benefit detail IDs provided for deletion");
            }

            StringBuilder deleteQuery = new StringBuilder("DELETE FROM BenefitDetail WHERE id IN (");
            string query2 = "INSERT INTO TAudit (action, tableName, dateModified, dateCreated, isActive) VALUES ('Delete', 'BenefitDetail', GETDATE(), GETDATE(), 1)";


            List<SqlParameter> parameters = new List<SqlParameter>();
            SqlParameter[] parameters2 = { };
            for (int i = 0; i < benefitDetailIds.Count; i++)
            {
                string parameterName = "@BenefitDetailId" + i;
                deleteQuery.Append(parameterName);

                if (i < benefitDetailIds.Count - 1)
                {
                    deleteQuery.Append(", ");
                }

                parameters.Add(new SqlParameter(parameterName, benefitDetailIds[i]));
            }

            deleteQuery.Append(");");

            _exQuery.ExecuteRawQuery(deleteQuery.ToString(), parameters.ToArray());
            _exQuery.ExecuteRawQuery(query2, parameters2);
            return new JsonResult("Benefit detail deleted successfully");
        }

    }
}