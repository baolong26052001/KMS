using System;
using System.Collections.Generic;

namespace KMS.Models
{
    public partial class LoanStatement
    {
        public int Id { get; set; }
        public int? AccountId { get; set; }
        public int? Debt { get; set; }
        public int? Paid { get; set; }
        public DateTime? TransactionDate { get; set; }
        public string? Description { get; set; }
        public bool? Status { get; set; }
    }
}
