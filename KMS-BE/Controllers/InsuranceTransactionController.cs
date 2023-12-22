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
            string query = "SELECT\r\n    itr.transactionDate,\r\n    itr.id,\r\n    itr.memberId,\r\n    itr.contractId,\r\n    ipa.packageName,\r\n\tit.typeName,\r\n    itr.provider,\r\n    itr.registrationDate,\r\n    itr.expireDate,\r\n    itr.annualPay,\r\n    itr.status\r\nFROM\r\n    InsuranceTransaction itr\r\nJOIN\r\n    InsurancePackage ipa ON ipa.id = itr.packageId\r\nJOIN\r\n    InsuranceType it ON it.id = itr.packageId";

            DataTable table = _exQuery.ExecuteRawQuery(query);
            return new JsonResult(table);
        }

        [HttpGet]
        [Route("ShowInsuranceTransaction/{id}")]
        public JsonResult GetInsuranceTransactionById(int id)
        {
            string query = "SELECT\r\n    itr.transactionDate,\r\n    itr.id,\r\n    itr.memberId,\r\n    itr.contractId,\r\n    ipa.packageName,\r\n\tit.typeName,\r\n    itr.provider,\r\n    itr.registrationDate,\r\n    itr.expireDate,\r\n    itr.annualPay,\r\n    itr.status\r\nFROM\r\n    InsuranceTransaction itr\r\nJOIN\r\n    InsurancePackage ipa ON ipa.id = itr.packageId\r\nJOIN\r\n    InsuranceType it ON it.id = itr.packageId where itr.id=@id";

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

        [HttpGet]
        [Route("ShowBeneficiaryOfMember/{id}")]
        public JsonResult GetBeneficiaryOfMember(int id) // id này là memberId
        {
            string query = "select beneficiaryName, beneficiaryId, relationship\r\nfrom InsuranceTransaction\r\nwhere memberId = @id";

            SqlParameter parameter = new SqlParameter("@Id", id);
            DataTable table = _exQuery.ExecuteRawQuery(query, new[] { parameter });

            if (table.Rows.Count > 0)
            {
                return new JsonResult(table);
            }
            else
            {
                return new JsonResult("Beneficiary not found");
            }
        }


        [HttpPost]
        [Route("AddInsuranceTransaction")]
        public JsonResult AddInsuranceTransaction([FromBody] InsuranceTransaction insuranceTransaction)
        {
            string query = "INSERT INTO InsuranceTransaction (memberId, contractId, beneficiaryName, beneficiaryId, relationship, packageId, provider, status, transactionDate) " +
                           "VALUES (@MemberId, @ContractId, @BeneficiaryId, @Relationship, @PackageId, @Provider, @Status, GETDATE())";

            SqlParameter[] parameters =
            {
                new SqlParameter("@MemberId", insuranceTransaction.MemberId),
                new SqlParameter("@ContractId", insuranceTransaction.ContractId),
                new SqlParameter("@BeneficiaryName", insuranceTransaction.BeneficiaryName),
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
            string query = "SELECT\r\n    itr.transactionDate,\r\n    itr.id,\r\n    itr.memberId,\r\n    itr.contractId,\r\n    ipa.packageName,\r\n\tit.typeName,\r\n    itr.provider,\r\n    itr.registrationDate,\r\n    itr.expireDate,\r\n    itr.annualPay,\r\n    itr.status\r\nFROM\r\n    InsuranceTransaction itr\r\nJOIN\r\n    InsurancePackage ipa ON ipa.id = itr.packageId\r\nJOIN\r\n    InsuranceType it ON it.id = itr.packageId " +
                           "WHERE itr.id LIKE @searchQuery OR " +
                           "itr.memberId LIKE @searchQuery OR " +
                           "itr.contractId LIKE @searchQuery OR " +
                           "it.typeName LIKE @searchQuery OR " +
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
            string query = "SELECT\r\n    itr.transactionDate,\r\n    itr.id,\r\n    itr.memberId,\r\n    itr.contractId,\r\n    ipa.packageName,\r\n\tit.typeName,\r\n    itr.provider,\r\n    itr.registrationDate,\r\n    itr.expireDate,\r\n    itr.annualPay,\r\n    itr.status\r\nFROM\r\n    InsuranceTransaction itr\r\nJOIN\r\n    InsurancePackage ipa ON ipa.id = itr.packageId\r\nJOIN\r\n    InsuranceType it ON it.id = itr.packageId ";

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