using System;
using System.Collections.Generic;

namespace KMS.Models
{
    public partial class LoanTransactionDetail
    {
        public int Id { get; set; }
        public int? LoanHeaderId { get; set; }
        public int? PaidAmount { get; set; }
        public int? DebtRemaining { get; set; }
        public DateTime? TransactionDate { get; set; }
    }
}
