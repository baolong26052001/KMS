using System;
using System.Collections.Generic;

namespace KMS.Models
{
    public partial class LoanTransaction
    {
        public int Id { get; set; }
        public int? MemberId { get; set; }
        public int? AccountId { get; set; }
        public int? LoanTerm { get; set; }
        public double? Debt { get; set; }
        public double? Balance { get; set; }
        public string? TransactionType { get; set; }
        public double? InterestRate { get; set; }
        public DateTime? LoanDate { get; set; }
        public DateTime? DueDate { get; set; }
        public bool? IsActive { get; set; }
    }
}
