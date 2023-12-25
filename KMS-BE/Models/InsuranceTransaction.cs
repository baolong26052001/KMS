using System;
using System.Collections.Generic;

namespace KMS.Models
{
    public partial class InsuranceTransaction
    {
        public int Id { get; set; }
        public int? MemberId { get; set; }
        public int? ContractId { get; set; }
        public int? PackageId { get; set; }
        public DateTime? RegistrationDate { get; set; }
        public DateTime? ExpireDate { get; set; }
        public double? AnnualPay { get; set; }
        public bool? Status { get; set; }
        public DateTime? TransactionDate { get; set; }
    }
}
