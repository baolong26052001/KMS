using System;
using System.Collections.Generic;

namespace KMS.Models
{
    public partial class LoanTransactionDetail
    {
        public int Id { get; set; }
        public int? AccountId { get; set; }
        public int? MemberId { get; set; }
        public int? ContractId { get; set; }
        public DateTime? PaymentDate { get; set; }
        public double? BeginningBalance { get; set; }
        public double? ScheduledPayment { get; set; }
        public double? ActualPayback { get; set; }
        public double? EndingBalance { get; set; }
        public double? InterestRate { get; set; }
        public int? Status { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? DateCreated { get; set; }
        public int? LoanTransactionId { get; set; }
    }
}
