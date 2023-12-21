using Microsoft.AspNetCore.Mvc;
using KMS.Models;
using KMS.Tools;
using System.Data;
using Microsoft.Data.SqlClient;

namespace KMS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InsuranceTransactionController : ControllerBase
    {
        private readonly KioskManagementSystemContext _dbcontext;
        private IConfiguration _configuration;
        private readonly ExecuteQuery _exQuery;

        public InsuranceTransactionController(IConfiguration configuration, KioskManagementSystemContext _context, ExecuteQuery exQuery)
        {
            _dbcontext = _context;
            _configuration = configuration;
            _exQuery = exQuery;
        }

        [HttpGet]
        [Route("ShowInsuranceTransaction")]
        public JsonResult GetInsuranceTransaction()
        {
            string query = "select itr.transactionDate, itr.id, itr.memberId, itr.contractId, itr.beneficiaryId, itr.relationship, ipa.packageName, itr.provider, itr.registrationDate, itr.expireDate, itr.annualPay, itr.status FROM InsuranceTransaction itr JOIN InsurancePackage ipa ON ipa.id = itr.packageId";

            DataTable table = _exQuery.ExecuteRawQuery(query);
            return new JsonResult(table);
        }

        [HttpGet]
        [Route("ShowInsuranceTransaction/{id}")]
        public JsonResult GetInsuranceTransactionById(int id)
        {
            string query = "select itr.transactionDate, itr.id, itr.memberId, itr.contractId, itr.beneficiaryId, itr.relationship, ipa.packageName, itr.provider, itr.registrationDate, itr.expireDate, itr.annualPay, itr.status FROM InsuranceTransaction itr JOIN InsurancePackage ipa ON ipa.id = itr.packageId where itr.id=@id";

            SqlParameter parameter = new SqlParameter("@Id", id);
            DataTable table = _exQuery.ExecuteRawQuery(query, new[] { parameter });

            if (table.Rows.Count > 0)
            {
                return new JsonResult(table);
            }
            else
            {
                return new JsonResult("Insurance Transaction not found");
            }
        }


        [HttpPost]
        [Route("AddInsuranceTransaction")]
        public JsonResult AddInsuranceTransaction([FromBody] InsuranceTransaction insuranceTransaction)
        {
            string query = "INSERT INTO InsuranceTransaction (memberId, contractId, beneficiaryId, relationship, packageId, provider, status, transactionDate) " +
                           "VALUES (@MemberId, @ContractId, @BeneficiaryId, @Relationship, @PackageId, @Provider, @Status, GETDATE())";

            SqlParameter[] parameters =
            {
                new SqlParameter("@MemberId", insuranceTransaction.MemberId),
                new SqlParameter("@ContractId", insuranceTransaction.ContractId),
                new SqlParameter("@BeneficiaryId", insuranceTransaction.BeneficiaryId),
                new SqlParameter("@Relationship", insuranceTransaction.Relationship),
                new SqlParameter("@PackageId", insuranceTransaction.PackageId),
                new SqlParameter("@Provider", insuranceTransaction.Provider),
                new SqlParameter("@Status", insuranceTransaction.Status),
                
            };

            _exQuery.ExecuteRawQuery(query, parameters);

            return new JsonResult("Insurance Transaction added successfully");
        }

        [HttpGet]
        [Route("SearchInsuranceTransaction")]
        public JsonResult SearchInsuranceTransaction(string searchQuery)
        {
            string query = "select itr.transactionDate, itr.id, itr.memberId, itr.contractId, itr.beneficiaryId, itr.relationship, ipa.packageName, itr.provider, itr.registrationDate, itr.expireDate, itr.annualPay, itr.status " +
                           "FROM InsuranceTransaction itr " +
                           "JOIN InsurancePackage ipa ON ipa.id = itr.packageId " +
                           "WHERE itr.id LIKE @searchQuery OR " +
                           "itr.memberId LIKE @searchQuery OR " +
                           "itr.contractId LIKE @searchQuery OR " +
                           "itr.beneficiaryId LIKE @searchQuery OR " +
                           "itr.relationship LIKE @searchQuery OR " +
                           "ipa.packageName LIKE @searchQuery OR " +
                           "itr.provider LIKE @searchQuery OR " +
                           "itr.annualPay LIKE @searchQuery OR " +
                           "itr.status LIKE @searchQuery";

            SqlParameter parameter = new SqlParameter("@searchQuery", "%" + searchQuery + "%");
            DataTable table = _exQuery.ExecuteRawQuery(query, new[] { parameter });

            return new JsonResult(table);
        }

        [HttpGet]
        [Route("FilterInsuranceTransaction")]
        public JsonResult FilterInsuranceTransaction([FromQuery] DateTime? startDate = null, [FromQuery] DateTime? endDate = null)
        {
            string query = "select itr.transactionDate, itr.id, itr.memberId, itr.contractId, itr.beneficiaryId, itr.relationship, ipa.packageName, itr.provider, itr.registrationDate, itr.expireDate, itr.annualPay, itr.status " +
                           "FROM InsuranceTransaction itr " +
                           "JOIN InsurancePackage ipa ON ipa.id = itr.packageId ";

            List<SqlParameter> parameters = new List<SqlParameter>();





            if (startDate.HasValue && endDate.HasValue)
            {

                startDate = startDate.Value.Date.AddHours(0).AddMinutes(0).AddSeconds(0);
                endDate = endDate.Value.Date.AddHours(23).AddMinutes(59).AddSeconds(59);

                query += (parameters.Count == 0 ? " WHERE " : " AND ") + "itr.transactionDate >= @startDate AND itr.transactionDate <= @endDate";
                parameters.Add(new SqlParameter("@startDate", startDate.Value));
                parameters.Add(new SqlParameter("@endDate", endDate.Value));
            }

            DataTable table = _exQuery.ExecuteRawQuery(query, parameters.ToArray());

            return new JsonResult(table);
        }


    }
}