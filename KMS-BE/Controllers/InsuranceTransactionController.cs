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
            ResponseDto response = new ResponseDto();
            try
            {
                string query = @"SELECT 
	                        itr.transactionDate,
	                        itr.id,
	                        itr.memberId,
	                        m.fullName,
	                        itr.contractId,
	                        ipd.id AS packageId,
	                        iph.packageName,
	                        it.typeName,
	                        inspvd.provider,
	                        ipd.ageRangeId,
	                        ar.startAge,
	                        ar.endAge,
	                        N'Từ ' + CONVERT(VARCHAR(10), ar.startAge) + N' đến ' + CONVERT(VARCHAR(10), ar.endAge) + N' tuổi' AS description,
	                        itr.annualPay,
	                        ipd.fee,
	                        itr.registrationDate,
	                        itr.expireDate,
	                        itr.status 
                        FROM 
	                        InsuranceTransaction itr
                        LEFT JOIN 
	                        LMember m ON m.id = itr.memberId
                        LEFT JOIN 
	                        InsurancePackageDetail ipd ON itr.packageDetailId = ipd.id
                        LEFT JOIN 
	                        AgeRange ar ON ar.id = ipd.ageRangeId
                        left join
	                        InsurancePackageHeader iph on iph.id = ipd.packageHeaderId
                        left join
	                        InsuranceProvider inspvd on inspvd.id = iph.insuranceProviderId
                        left join
	                        InsuranceType it on it.id = iph.insuranceTypeId";


                DataTable table = _exQuery.ExecuteRawQuery(query);
                return new JsonResult(table);
            }
            catch (Exception ex)
            {
                response.Code = -1;
                response.Message = ex.Message;
                response.Exception = ex.ToString();
                response.Data = null;
            }
            return new JsonResult(response);
            
        }

        [HttpGet]
        [Route("ShowInsuranceTransactionByMemberId/{memberId}")] // coi xem người này đã mua những gói nào và "lịch sử" mua bảo hiểm của người này
        public JsonResult GetInsuranceTransactionByMemberId(int memberId)
        {
            ResponseDto response = new ResponseDto();
            try
            {
                string query = @"SELECT 
	                        itr.transactionDate,
	                        itr.id,
	                        itr.memberId,
	                        m.fullName,
	                        itr.contractId,
	                        ipd.id AS packageId,
	                        iph.packageName,
	                        it.typeName,
	                        inspvd.provider,
	                        ipd.ageRangeId,
	                        ar.startAge,
	                        ar.endAge,
	                        N'Từ ' + CONVERT(VARCHAR(10), ar.startAge) + N' đến ' + CONVERT(VARCHAR(10), ar.endAge) + N' tuổi' AS description,
	                        itr.annualPay,
	                        ipd.fee,
	                        itr.registrationDate,
	                        itr.expireDate,
	                        itr.status 
                        FROM 
	                        InsuranceTransaction itr
                        LEFT JOIN 
	                        LMember m ON m.id = itr.memberId
                        LEFT JOIN 
	                        InsurancePackageDetail ipd ON itr.packageDetailId = ipd.id
                        LEFT JOIN 
	                        AgeRange ar ON ar.id = ipd.ageRangeId
                        left join
	                        InsurancePackageHeader iph on iph.id = ipd.packageHeaderId
                        left join
	                        InsuranceProvider inspvd on inspvd.id = iph.insuranceProviderId
                        left join
	                        InsuranceType it on it.id = iph.insuranceTypeId where itr.memberId = @MemberId";

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
            catch (Exception ex)
            {
                response.Code = -1;
                response.Message = ex.Message;
                response.Exception = ex.ToString();
                response.Data = null;
            }
            return new JsonResult(response);
            
        }

        [HttpGet]
        [Route("ShowInsuranceTransaction/{id}")] // coi xem cái giao dịch này là người nào mua và người này đã mua gói nào cho người thụ hưởng nào
        public JsonResult GetInsuranceTransactionById(int id)
        {
            ResponseDto response = new ResponseDto();
            try
            {
                string query = @"SELECT 
	                        itr.transactionDate,
	                        itr.id,
	                        itr.memberId,
	                        m.fullName,
	                        itr.contractId,
	                        ipd.id AS packageId,
	                        iph.packageName,
	                        it.typeName,
	                        inspvd.provider,
	                        ipd.ageRangeId,
	                        ar.startAge,
	                        ar.endAge,
	                        N'Từ ' + CONVERT(VARCHAR(10), ar.startAge) + N' đến ' + CONVERT(VARCHAR(10), ar.endAge) + N' tuổi' AS description,
	                        itr.annualPay,
	                        ipd.fee,
	                        itr.registrationDate,
	                        itr.expireDate,
	                        itr.status 
                        FROM 
	                        InsuranceTransaction itr
                        LEFT JOIN 
	                        LMember m ON m.id = itr.memberId
                        LEFT JOIN 
	                        InsurancePackageDetail ipd ON itr.packageDetailId = ipd.id
                        LEFT JOIN 
	                        AgeRange ar ON ar.id = ipd.ageRangeId
                        left join
	                        InsurancePackageHeader iph on iph.id = ipd.packageHeaderId
                        left join
	                        InsuranceProvider inspvd on inspvd.id = iph.insuranceProviderId
                        left join
	                        InsuranceType it on it.id = iph.insuranceTypeId where itr.id = @Id";

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
            catch (Exception ex)
            {
                response.Code = -1;
                response.Message = ex.Message;
                response.Exception = ex.ToString();
                response.Data = null;
            }
            return new JsonResult(response);
            
        }

        [HttpGet]
        [Route("ShowBeneficiaryByInsuranceTransactionId/{insuranceTransactionId}")]
        public JsonResult ShowBeneficiaryByInsuranceTransactionId(int insuranceTransactionId)
        {
            ResponseDto response = new ResponseDto();
            try
            {
                string query = @"select * from Beneficiary where transactionId = @InsuranceTransactionId";

                SqlParameter parameter = new SqlParameter("@InsuranceTransactionId", insuranceTransactionId);
                DataTable table = _exQuery.ExecuteRawQuery(query, new[] { parameter });

                if (table.Rows.Count > 0)
                {
                    return new JsonResult(table);
                }
                else
                {
                    return new JsonResult("Not found");
                }
            }
            catch (Exception ex)
            {
                response.Code = -1;
                response.Message = ex.Message;
                response.Exception = ex.ToString();
                response.Data = null;
            }
            return new JsonResult(response);

        }


        [HttpPost]
        [Route("SaveInsuranceTransaction")]
        public JsonResult SaveInsuranceTransaction([FromBody] InsuranceTransaction insuranceTransaction)
        {
            ResponseDto response = new ResponseDto();
            try
            {
                Random random = new Random();
                int contractId = random.Next(10000000, 99999999);

                string query = "INSERT INTO InsuranceTransaction (memberId, contractId, packageDetailId, registrationDate, expireDate, annualPay, status, transactionDate) " +
                           "VALUES (@MemberId, @ContractId, @PackageDetailId, GETDATE(), DATEADD(YEAR, 1, GETDATE()), @AnnualPay, @Status, GETDATE()) ";

                SqlParameter[] parameters =
                {
                    new SqlParameter("@MemberId", insuranceTransaction.MemberId),
                    new SqlParameter("@ContractId", contractId),
                    new SqlParameter("@PackageDetailId", insuranceTransaction.PackageDetailId),
                    new SqlParameter("@AnnualPay", insuranceTransaction.AnnualPay),
                    new SqlParameter("@Status", insuranceTransaction.Status),


                };

                _exQuery.ExecuteRawQuery(query, parameters);

                return new JsonResult("Insurance Transaction saved successfully");
            }
            catch (Exception ex)
            {
                response.Code = -1;
                response.Message = ex.Message;
                response.Exception = ex.ToString();
                response.Data = null;
            }
            return new JsonResult(response);
            
        }

        [HttpPost]
        [Route("SaveBeneficiary")]
        public JsonResult SaveBeneficiary([FromBody] Beneficiary beneficiary)
        {
            ResponseDto response = new ResponseDto();
            try
            {
                int transactionId;
                using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand("SELECT ISNULL(MAX(id), 0) + 1 FROM InsuranceTransaction", connection);
                    transactionId = (int)cmd.ExecuteScalar();
                }


                string query = "INSERT INTO Beneficiary (memberId, beneficiaryName, beneficiaryId, relationship, transactionId, birthday, gender, address, occupation, email, phone, taxCode) " +
               "VALUES (@MemberId, @BeneficiaryName, @BeneficiaryId, @Relationship, @TransactionId, @Birthday, @Gender, @Address, @Occupation, @Email, @Phone, @TaxCode)";

                SqlParameter[] parameters =
                {
                    new SqlParameter("@MemberId", beneficiary.MemberId),
                    new SqlParameter("@BeneficiaryName", beneficiary.BeneficiaryName),
                    new SqlParameter("@BeneficiaryId", beneficiary.BeneficiaryId),
                    new SqlParameter("@Relationship", beneficiary.Relationship),
                    new SqlParameter("@TransactionId", transactionId),
    
                    // Additional parameters
                    new SqlParameter("@Birthday", beneficiary.Birthday), 
                    new SqlParameter("@Gender", beneficiary.Gender),
                    new SqlParameter("@Address", beneficiary.Address),
                    new SqlParameter("@Occupation", beneficiary.Occupation),
                    new SqlParameter("@Email", beneficiary.Email),
                    new SqlParameter("@Phone", beneficiary.Phone),
                    new SqlParameter("@TaxCode", (object)beneficiary.TaxCode ?? DBNull.Value)
                };


                _exQuery.ExecuteRawQuery(query, parameters);

                return new JsonResult("Beneficiary saved successfully");
            }
            catch (Exception ex)
            {
                response.Code = -1;
                response.Message = ex.Message;
                response.Exception = ex.ToString();
                response.Data = null;
            }
            return new JsonResult(response);
            
        }

        [HttpGet]
        [Route("SearchInsuranceTransaction")]
        public JsonResult SearchInsuranceTransaction(string searchQuery)
        {
            ResponseDto response = new ResponseDto();
            try
            {
                string query = @"SELECT 
	                        itr.transactionDate,
	                        itr.id,
	                        itr.memberId,
	                        m.fullName,
	                        itr.contractId,
	                        ipd.id AS packageId,
	                        iph.packageName,
	                        it.typeName,
	                        inspvd.provider,
	                        ipd.ageRangeId,
	                        ar.startAge,
	                        ar.endAge,
	                        N'Từ ' + CONVERT(VARCHAR(10), ar.startAge) + N' đến ' + CONVERT(VARCHAR(10), ar.endAge) + N' tuổi' AS description,
	                        itr.annualPay,
	                        ipd.fee,
	                        itr.registrationDate,
	                        itr.expireDate,
	                        itr.status 
                        FROM 
	                        InsuranceTransaction itr
                        LEFT JOIN 
	                        LMember m ON m.id = itr.memberId
                        LEFT JOIN 
	                        InsurancePackageDetail ipd ON itr.packageDetailId = ipd.id
                        LEFT JOIN 
	                        AgeRange ar ON ar.id = ipd.ageRangeId
                        left join
	                        InsurancePackageHeader iph on iph.id = ipd.packageHeaderId
                        left join
	                        InsuranceProvider inspvd on inspvd.id = iph.insuranceProviderId
                        left join
	                        InsuranceType it on it.id = iph.insuranceTypeId " +

                "where m.fullName LIKE @searchQuery " +
                "or iph.packageName LIKE @searchQuery " +

                "or it.typeName LIKE @searchQuery " +
                "or inspvd.provider LIKE @searchQuery " +
                "or itr.annualPay LIKE @searchQuery " +
                "or ar.startAge LIKE @searchQuery " +
                "or ar.endAge LIKE @searchQuery";

                SqlParameter parameter = new SqlParameter("@searchQuery", "%" + searchQuery + "%");
                DataTable table = _exQuery.ExecuteRawQuery(query, new[] { parameter });

                return new JsonResult(table);
            }
            catch (Exception ex)
            {
                response.Code = -1;
                response.Message = ex.Message;
                response.Exception = ex.ToString();
                response.Data = null;
            }
            return new JsonResult(response);
            
        }

        [HttpGet]
        [Route("FilterInsuranceTransaction")]
        public JsonResult FilterInsuranceTransaction([FromQuery] DateTime? startDate = null, [FromQuery] DateTime? endDate = null)
        {
            ResponseDto response = new ResponseDto();
            try
            {
                string query = @"SELECT 
	                        itr.transactionDate,
	                        itr.id,
	                        itr.memberId,
	                        m.fullName,
	                        itr.contractId,
	                        ipd.id AS packageId,
	                        iph.packageName,
	                        it.typeName,
	                        inspvd.provider,
	                        ipd.ageRangeId,
	                        ar.startAge,
	                        ar.endAge,
	                        N'Từ ' + CONVERT(VARCHAR(10), ar.startAge) + N' đến ' + CONVERT(VARCHAR(10), ar.endAge) + N' tuổi' AS description,
	                        itr.annualPay,
	                        ipd.fee,
	                        itr.registrationDate,
	                        itr.expireDate,
	                        itr.status 
                        FROM 
	                        InsuranceTransaction itr
                        LEFT JOIN 
	                        LMember m ON m.id = itr.memberId
                        LEFT JOIN 
	                        InsurancePackageDetail ipd ON itr.packageDetailId = ipd.id
                        LEFT JOIN 
	                        AgeRange ar ON ar.id = ipd.ageRangeId
                        left join
	                        InsurancePackageHeader iph on iph.id = ipd.packageHeaderId
                        left join
	                        InsuranceProvider inspvd on inspvd.id = iph.insuranceProviderId
                        left join
	                        InsuranceType it on it.id = iph.insuranceTypeId ";

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
            catch (Exception ex)
            {
                response.Code = -1;
                response.Message = ex.Message;
                response.Exception = ex.ToString();
                response.Data = null;
            }
            return new JsonResult(response);
            
        }


    }
}