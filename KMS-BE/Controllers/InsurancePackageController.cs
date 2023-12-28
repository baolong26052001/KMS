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
        [Route("ShowInsurancePackage")] // show ra có bao nhiêu package (packageA,packageB,packageC,...)
        public JsonResult GetInsurancePackage()
        {
            string query = "select ipack.id, ipack.packageName, itype.typeName, ipack.duration, ipack.payType, ipack.fee, ipack.dateModified, ipack.dateCreated " +
                "from InsurancePackage ipack, InsuranceType itype " +
                "where ipack.insuranceType = itype.id;";

            DataTable table = _exQuery.ExecuteRawQuery(query);
            return new JsonResult(table);
        }

        [HttpGet]
        [Route("ShowInsurancePackage/{id}")] // hiện thông tin khi ở màn hình edit package
        public JsonResult GetInsurancePackageById(int id)
        {
            string query = "select ipack.id, ipack.packageName, itype.typeName, ipack.duration, ipack.payType, ipack.fee, ipack.dateModified, ipack.dateCreated " +
                "from InsurancePackage ipack, InsuranceType itype " +
                "where ipack.insuranceType = itype.id and ipack.id=@id ";

            SqlParameter parameter = new SqlParameter("@id", id);
            DataTable table = _exQuery.ExecuteRawQuery(query, new[] { parameter });

            if (table.Rows.Count > 0)
            {
                return new JsonResult(table);
            }
            else
            {
                return new JsonResult("Insurance Package not found");
            }
        }

        [HttpGet]
        [Route("ShowBenefitById/{id}")] // hiện thông tin khi ở màn hình edit benefit
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
        [Route("ShowBenefitDetailById/{id}")] // hiện thông tin khi ở màn hình edit benefit detail
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
        [Route("ShowInsurancePackageDetail/{id}")] // khi ấn vào view package A, thì sẽ show ra benefit của package A
        public JsonResult GetInsurancePackageDetail(int id)
        {
            string query = "select b.id, b.content, b.coverage, b.description, ipack.packageName, itype.typeName, ipack.fee, b.dateModified, b.dateCreated " +
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
            string query = "INSERT INTO InsurancePackage (packageName, insuranceType, duration, payType, fee, dateModified, dateCreated) " +
                           "VALUES (@PackageName, @InsuranceType, @Duration, @PayType, @Fee, GETDATE(), GETDATE())";
            string query2 = "INSERT INTO TAudit (action, tableName, dateModified, dateCreated, isActive) VALUES ('Add', 'InsurancePackage', GETDATE(), GETDATE(), 1)";

            SqlParameter[] parameters =
            {
                new SqlParameter("@PackageName", insurancePackage.PackageName),
                new SqlParameter("@InsuranceType", insurancePackage.InsuranceType),
                new SqlParameter("@Duration", insurancePackage.Duration),
                new SqlParameter("@PayType", insurancePackage.PayType),
                new SqlParameter("@Fee", insurancePackage.Fee),
            };
            SqlParameter[] parameters2 = { };

            _exQuery.ExecuteRawQuery(query, parameters);
            _exQuery.ExecuteRawQuery(query2, parameters2);

            return new JsonResult("Insurance Package added successfully");
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
                           "packageId = @PackageId, dateModified = GETDATE() " +
                           "WHERE id = @id";
            string query2 = "INSERT INTO TAudit (action, tableName, dateModified, dateCreated, isActive) VALUES ('Edit', 'Benefit', GETDATE(), GETDATE(), 1)";

            SqlParameter[] parameters =
            {
                new SqlParameter("@id", id),
                new SqlParameter("@Content", benefit.Content),
                new SqlParameter("@Coverage", benefit.Coverage),
                new SqlParameter("@Description", benefit.Description),
                new SqlParameter("@PackageId", benefit.PackageId),

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
                           "benefitId = @BenefitId, dateModified = GETDATE() " +
                           "WHERE id = @id";
            string query2 = "INSERT INTO TAudit (action, tableName, dateModified, dateCreated, isActive) VALUES ('Edit', 'BenefitDetail', GETDATE(), GETDATE(), 1)";

            SqlParameter[] parameters =
            {
                new SqlParameter("@id", id),
                new SqlParameter("@Content", benefitDetail.Content),
                new SqlParameter("@Coverage", benefitDetail.Coverage),
                new SqlParameter("@BenefitId", benefitDetail.BenefitId),

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

        [HttpPut]
        [Route("EditInsurancePackage/{id}")]
        public JsonResult EditInsurancePackage(int id, [FromBody] InsurancePackage insurancePackage)
        {
            string query = "UPDATE InsurancePackage " +
                           "SET packageName = @PackageName, insuranceType = @InsuranceType, duration = @Duration, " +
                           "payType = @PayType, fee = @Fee, dateModified = GETDATE() " +
                           "WHERE id = @id";
            string query2 = "INSERT INTO TAudit (action, tableName, dateModified, dateCreated, isActive) VALUES ('Edit', 'InsurancePackage', GETDATE(), GETDATE(), 1)";

            SqlParameter[] parameters =
            {
                new SqlParameter("@id", id),
                new SqlParameter("@PackageName", insurancePackage.PackageName),
                new SqlParameter("@InsuranceType", insurancePackage.InsuranceType),
                new SqlParameter("@Duration", insurancePackage.Duration),
                new SqlParameter("@PayType", insurancePackage.PayType), // đóng tiền theo năm hoặc tháng
                new SqlParameter("@Fee", insurancePackage.Fee),
                
            };
            SqlParameter[] parameters2 = { };

            _exQuery.ExecuteRawQuery(query, parameters);
            _exQuery.ExecuteRawQuery(query2, parameters2);

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
            string query2 = "INSERT INTO TAudit (action, tableName, dateModified, dateCreated, isActive) VALUES ('Delete', 'InsurancePackage', GETDATE(), GETDATE(), 1)";


            List<SqlParameter> parameters = new List<SqlParameter>();
            SqlParameter[] parameters2 = { };
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
            _exQuery.ExecuteRawQuery(query2, parameters2);

            return new JsonResult("Insurance Package deleted successfully");
        }

        [HttpGet]
        [Route("SearchInsurancePackage")]
        public JsonResult SearchInsurancePackage(string searchQuery)
        {
            string query = "SELECT ipack.id, ipack.packageName, itype.typeName, ipack.duration, ipack.payType, ipack.fee, ipack.dateModified, ipack.dateCreated " +
               "FROM InsurancePackage ipack " +
               "JOIN InsuranceType itype ON ipack.insuranceType = itype.id " +
               "WHERE ipack.id LIKE @searchQuery OR " +
               "ipack.packageName LIKE @searchQuery OR " +
               "itype.typeName LIKE @searchQuery OR " +
               "ipack.duration LIKE @searchQuery OR " +
               "ipack.payType LIKE @searchQuery OR " +
               "ipack.fee LIKE @searchQuery";



            

            SqlParameter parameter = new SqlParameter("@searchQuery", "%" + searchQuery + "%");
            DataTable table = _exQuery.ExecuteRawQuery(query, new[] { parameter });

            return new JsonResult(table);
        }


        [HttpGet]
        [Route("FilterInsurancePackage")]
        public JsonResult FilterInsurancePackage([FromQuery] DateTime? startDate = null, [FromQuery] DateTime? endDate = null)
        {
            string query = "SELECT ipack.id, ipack.packageName, itype.typeName, ipack.duration, ipack.payType, ipack.fee, ipack.dateModified, ipack.dateCreated " +
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