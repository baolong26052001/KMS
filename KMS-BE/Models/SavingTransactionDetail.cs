using System;
using System.Collections.Generic;

namespace KMS.Models
{
    public partial class SavingTransactionDetail
    {
        public int Id { get; set; }
        public int? AccountId { get; set; }
        public int? MemberId { get; set; }
        public int? ContractId { get; set; }
        public DateTime? StartDateOfSaving { get; set; }
        public double? TopUp { get; set; }
        public double? Withdraw { get; set; }
        public double? Balance { get; set; }
        public double? AnnualRate { get; set; }
        public DateTime? DueDate { get; set; }
        public int? Status { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? TransactionDate { get; set; }
        public int? SavingId { get; set; }
    }
}
