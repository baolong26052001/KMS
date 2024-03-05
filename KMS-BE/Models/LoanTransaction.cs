using System;
using System.Collections.Generic;

namespace KMS.Models
{
    public partial class LoanTransaction
    {
        public int Id { get; set; }
        public int? AccountId { get; set; }
        public int? ContractId { get; set; }
        public int? LoanTerm { get; set; }
        public int? Debt { get; set; }
        public int? TotalDebtMustPay { get; set; }
        public int? DebtPayPerMonth { get; set; }
        public double? LoanRate { get; set; }
        public DateTime? LoanDate { get; set; }
        public DateTime? DueDate { get; set; }
        public bool? Status { get; set; }
    }
}
