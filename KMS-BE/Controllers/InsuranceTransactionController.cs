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
            string query = "SELECT itr.transactionDate, itr.id, \r\n\titr.memberId, m.fullName, \r\n\titr.contractId, \r\n\tipa.id as packageId, ipa.packageName, \r\n\tipa.termId, t.content, \r\n\tit.id as typeId, it.typeName, \r\n\tipd.ageRangeId, ar.startAge, ar.endAge,\r\n\tN'Từ ' + CONVERT(VARCHAR(10), ar.startAge) + N' đến ' + CONVERT(VARCHAR(10), ar.endAge) + N' tuổi' as description,\r\n\tipa.insuranceProviderId, p.provider, \r\n\t\r\n\tipd.fee, itr.registrationDate, itr.expireDate\r\nFROM InsuranceTransaction itr \r\nLEFT JOIN InsurancePackageHeader ipa ON ipa.id = itr.packageId \r\nLEFT JOIN InsuranceType it ON it.id = ipa.insuranceTypeId\r\nLEFT JOIN Term t ON t.id = ipa.termId\r\nLEFT JOIN LMember m on m.id = itr.memberId\r\nLEFT JOIN InsuranceProvider p on p.id = ipa.insuranceProviderId\r\nLEFT JOIN InsurancePackageDetail ipd on ipd.packageHeaderId = ipa.id\r\nLEFT JOIN AgeRange ar on ar.id = ipd.ageRangeId";

            DataTable table = _exQuery.ExecuteRawQuery(query);
            return new JsonResult(table);
        }

        [HttpGet]
        [Route("ShowInsuranceTransactionByMemberId/{memberId}")] // coi xem người này đã mua những gói nào và "lịch sử" mua bảo hiểm của người này
        public JsonResult GetInsuranceTransactionByMemberId(int memberId)
        {
            string query = "SELECT itr.transactionDate, itr.id, \r\n\titr.memberId, m.fullName, \r\n\titr.contractId, \r\n\tipa.id as packageId, ipa.packageName, \r\n\tipa.termId, t.content, \r\n\tit.id as typeId, it.typeName, \r\n\tipd.ageRangeId, ar.startAge, ar.endAge,\r\n\tN'Từ ' + CONVERT(VARCHAR(10), ar.startAge) + N' đến ' + CONVERT(VARCHAR(10), ar.endAge) + N' tuổi' as description,\r\n\tipa.insuranceProviderId, p.provider, \r\n\tbf.beneficiaryId, bf.beneficiaryName,\r\n\tipd.fee, itr.registrationDate, itr.expireDate\r\nFROM InsuranceTransaction itr \r\nLEFT JOIN InsurancePackageHeader ipa ON ipa.id = itr.packageId \r\nLEFT JOIN InsuranceType it ON it.id = ipa.insuranceTypeId\r\nLEFT JOIN Term t ON t.id = ipa.termId\r\nLEFT JOIN LMember m on m.id = itr.memberId\r\nLEFT JOIN InsuranceProvider p on p.id = ipa.insuranceProviderId\r\nLEFT JOIN InsurancePackageDetail ipd on ipd.packageHeaderId = ipa.id\r\nLEFT JOIN AgeRange ar on ar.id = ipd.ageRangeId\r\nLEFT JOIN Beneficiary bf on bf.transactionId = itr.id\r\nwhere itr.memberId = @MemberId";

            SqlParameter parameter = new SqlParameter("@MemberId", memberId);
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
        [Route("ShowInsuranceTransaction/{id}")] // coi xem cái giao dịch này là người nào mua và người này đã mua gói nào cho người thụ hưởng nào
        public JsonResult GetInsuranceTransactionById(int id)
        {
            string query = "SELECT itr.transactionDate, itr.id, \r\n\titr.memberId, m.fullName, \r\n\titr.contractId, \r\n\tipa.id as packageId, ipa.packageName, \r\n\tipa.termId, t.content, \r\n\tit.id as typeId, it.typeName, \r\n\tipd.ageRangeId, ar.startAge, ar.endAge,\r\n\tN'Từ ' + CONVERT(VARCHAR(10), ar.startAge) + N' đến ' + CONVERT(VARCHAR(10), ar.endAge) + N' tuổi' as description,\r\n\tipa.insuranceProviderId, p.provider, \r\n\tbf.beneficiaryId, bf.beneficiaryName,\r\n\tipd.fee, itr.registrationDate, itr.expireDate\r\nFROM InsuranceTransaction itr \r\nLEFT JOIN InsurancePackageHeader ipa ON ipa.id = itr.packageId \r\nLEFT JOIN InsuranceType it ON it.id = ipa.insuranceTypeId\r\nLEFT JOIN Term t ON t.id = ipa.termId\r\nLEFT JOIN LMember m on m.id = itr.memberId\r\nLEFT JOIN InsuranceProvider p on p.id = ipa.insuranceProviderId\r\nLEFT JOIN InsurancePackageDetail ipd on ipd.packageHeaderId = ipa.id\r\nLEFT JOIN AgeRange ar on ar.id = ipd.ageRangeId\r\nLEFT JOIN Beneficiary bf on bf.transactionId = itr.id\r\nwhere itr.id = @Id";

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
        [Route("SaveInsuranceTransaction")]
        public JsonResult SaveInsuranceTransaction([FromBody] InsuranceTransaction insuranceTransaction)
        {
            string query = "INSERT INTO InsuranceTransaction (memberId, contractId, packageId, registrationDate, expireDate, annualPay, status, transactionDate) " +
                           "VALUES (@MemberId, @ContractId, @PackageId, GETDATE(), DATEADD(YEAR, 1, GETDATE()), @AnnualPay, @Status, GETDATE()) ";

            SqlParameter[] parameters =
            {
                new SqlParameter("@MemberId", insuranceTransaction.MemberId),
                new SqlParameter("@ContractId", insuranceTransaction.ContractId),
                new SqlParameter("@PackageId", insuranceTransaction.PackageId),
                new SqlParameter("@AnnualPay", insuranceTransaction.AnnualPay),
                new SqlParameter("@Status", insuranceTransaction.Status),

                
            };

            _exQuery.ExecuteRawQuery(query, parameters);

            return new JsonResult("Insurance Transaction saved successfully");
        }

        [HttpPost]
        [Route("SaveBeneficiary")]
        public JsonResult SaveBeneficiary([FromBody] Beneficiary beneficiary)
        {
            string query = "INSERT INTO Beneficiary (memberId, beneficiaryName, beneficiaryId, relationship, packageId, transactionId) " +
                   "VALUES (@MemberId, @BeneficiaryName, @BeneficiaryId, @Relationship, @PackageId, @TransactionId)";

            SqlParameter[] parameters =
            {
                new SqlParameter("@MemberId", beneficiary.MemberId),
                new SqlParameter("@BeneficiaryName", beneficiary.BeneficiaryName),
                new SqlParameter("@BeneficiaryId", beneficiary.BeneficiaryId),
                new SqlParameter("@Relationship", beneficiary.Relationship),
                new SqlParameter("@PackageId", beneficiary.PackageId),
                new SqlParameter("@TransactionId", beneficiary.TransactionId),
            };

            _exQuery.ExecuteRawQuery(query, parameters);

            return new JsonResult("Beneficiary saved successfully");
        }

        [HttpGet]
        [Route("SearchInsuranceTransaction")]
        public JsonResult SearchInsuranceTransaction(string searchQuery)
        {
            string query = "SELECT itr.transactionDate, itr.id, " +
                "\r\n\titr.memberId, m.fullName, " +
                "\r\n\titr.contractId, " +
                "\r\n\tipa.id as packageId, ipa.packageName, " +
                "\r\n\tipa.termId, t.content, \r\n\tit.id as typeId, it.typeName, \r\n\tipd.ageRangeId, ar.startAge, ar.endAge," +
                "\r\n\tN'Từ ' + CONVERT(VARCHAR(10), ar.startAge) + N' đến ' + CONVERT(VARCHAR(10), ar.endAge) + N' tuổi' as description," +
                "\r\n\tipa.insuranceProviderId, p.provider, " +
                
                "\r\n\tipd.fee, itr.registrationDate, itr.expireDate " +
                "\r\nFROM InsuranceTransaction itr " +
                "\r\nLEFT JOIN InsurancePackageHeader ipa ON ipa.id = itr.packageId " +
                "\r\nLEFT JOIN InsuranceType it ON it.id = ipa.insuranceTypeId " +
                "\r\nLEFT JOIN Term t ON t.id = ipa.termId " +
                "\r\nLEFT JOIN LMember m on m.id = itr.memberId " +
                "\r\nLEFT JOIN InsuranceProvider p on p.id = ipa.insuranceProviderId " +
                "\r\nLEFT JOIN InsurancePackageDetail ipd on ipd.packageHeaderId = ipa.id " +
                "\r\nLEFT JOIN AgeRange ar on ar.id = ipd.ageRangeId " +
                
                "\r\nwhere m.fullName LIKE @searchQuery or ipa.packageName LIKE @searchQuery or t.content LIKE @searchQuery or it.typeName LIKE @searchQuery " +
                "\r\nor p.provider LIKE @searchQuery or ipd.fee LIKE @searchQuery " +
                "\r\nor ar.startAge LIKE @searchQuery or ar.endAge LIKE @searchQuery";

            SqlParameter parameter = new SqlParameter("@searchQuery", "%" + searchQuery + "%");
            DataTable table = _exQuery.ExecuteRawQuery(query, new[] { parameter });

            return new JsonResult(table);
        }

        [HttpGet]
        [Route("FilterInsuranceTransaction")]
        public JsonResult FilterInsuranceTransaction([FromQuery] DateTime? startDate = null, [FromQuery] DateTime? endDate = null)
        {
            string query = "SELECT itr.transactionDate, itr.id, \r\n\titr.memberId, m.fullName, \r\n\titr.contractId, " +
                "\r\n\tipa.id as packageId, ipa.packageName, \r\n\tipa.termId, t.content, " +
                "\r\n\tit.id as typeId, it.typeName, \r\n\tipd.ageRangeId, ar.startAge, ar.endAge, " +
                "\r\n\tN'Từ ' + CONVERT(VARCHAR(10), ar.startAge) + N' đến ' + CONVERT(VARCHAR(10), ar.endAge) + N' tuổi' as description, " +
                "\r\n\tipa.insuranceProviderId, p.provider, " +

                "\r\n\tipd.fee, itr.registrationDate, itr.expireDate\r\nFROM InsuranceTransaction itr " +
                "\r\nLEFT JOIN InsurancePackageHeader ipa ON ipa.id = itr.packageId " +
                "\r\nLEFT JOIN InsuranceType it ON it.id = ipa.insuranceTypeId " +
                "\r\nLEFT JOIN Term t ON t.id = ipa.termId\r\nLEFT JOIN LMember m on m.id = itr.memberId " +
                "\r\nLEFT JOIN InsuranceProvider p on p.id = ipa.insuranceProviderId " +
                "\r\nLEFT JOIN InsurancePackageDetail ipd on ipd.packageHeaderId = ipa.id " +
                "\r\nLEFT JOIN AgeRange ar on ar.id = ipd.ageRangeId";

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