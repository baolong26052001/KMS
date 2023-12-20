using System;
using System.Collections.Generic;

namespace KMS.Models
{
    public partial class LoanStatement
    {
        public int Id { get; set; }
        public int? MemberId { get; set; }
        public int? AccountId { get; set; }
        public int? LoanId { get; set; }
        public int? LoanTerm { get; set; }
        public double? Balance { get; set; }
        public double? InterestRate { get; set; }
        public int? Status { get; set; }
        public DateTime? DateLoan { get; set; }
        public DateTime? DueDate { get; set; }
    }
}
