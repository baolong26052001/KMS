using System;
using System.Collections.Generic;

namespace KMS.Models
{
    public partial class LoanTransaction
    {
        public int LoanId { get; set; }
        public int? TransactionId { get; set; }
        public int? MemberId { get; set; }
        public int? ContractId { get; set; }
        public int? LoanTerm { get; set; }
        public int? Debt { get; set; }
        public int? TotalDebtMustPay { get; set; }
        public double? LoanRate { get; set; }
        public DateTime? TransactionDate { get; set; }
        public DateTime? DueDate { get; set; }
        public int? Status { get; set; }
    }

    public partial class PayBack
    {
        public int PaybackId { get; set; }
        public int? TransactionId { get; set; }
        public int? MemberId { get; set; }
        public int? LoanId { get; set; }
        public int? Payback { get; set; }
        public int? InDebt { get; set; }
        public DateTime? TransactionDate { get; set; }
    }

}
