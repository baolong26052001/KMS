using System;
using System.Collections.Generic;

namespace KMS.Models
{
    public partial class Beneficiary
    {
        public int Id { get; set; }
        public int? MemberId { get; set; }
        public string? BeneficiaryName { get; set; }
        public string? BeneficiaryId { get; set; }
        public string? Relationship { get; set; }
        public int? TransactionId { get; set; }

        public DateTime? Birthday { get; set; }
        public string? Gender { get; set; }
        public string? Address { get; set; }
        public string? Occupation { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? TaxCode { get; set; }
    }
}
