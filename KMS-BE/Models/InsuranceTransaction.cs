using System;
using System.Collections.Generic;

namespace KMS.Models
{
    public partial class InsuranceTransaction
    {
        public int Id { get; set; }
        public int? MemberId { get; set; }
        public int? ContractId { get; set; }
        public int? BeneficiaryId { get; set; }
        public string? Relationship { get; set; }
        public int? PackageId { get; set; }
        public string? Provider { get; set; }
        public DateTime? RegistrationDate { get; set; }
        public DateTime? ExpireDate { get; set; }
        public double? AnnualPay { get; set; }
        public int? Status { get; set; }
        public DateTime? TransactionDate { get; set; }
    }
}
