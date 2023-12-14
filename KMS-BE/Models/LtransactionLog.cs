using System;
using System.Collections.Generic;

namespace KMS.Models
{
    public partial class LtransactionLog
    {
        public int Id { get; set; }
        public int? KioskId { get; set; }
        public int? MemberId { get; set; }
        public int? TransactionId { get; set; }
        public int? AccountId { get; set; }
        public int? StationId { get; set; }
        public double? Rate { get; set; }
        public double? Amount { get; set; }
        public string? Period { get; set; }
        public double? MonthlyPaymentAmount { get; set; }
        public DateTime? DueDate { get; set; }
        public double? Debit { get; set; }
        public double? Credit { get; set; }
        public double? Payback { get; set; }
        public string? Currency { get; set; }
        public int? Status { get; set; }
        public string? PaymentMethod { get; set; }
        public string? Content { get; set; }
        public string? TaxType { get; set; }
        public string? TaxName { get; set; }
        public string? TaxCode { get; set; }
        public double? TaxAmount { get; set; }
        public DateTime? TransactionDate { get; set; }
        public DateTime? DateModified { get; set; }
        public DateTime? DateCreated { get; set; }
        public bool? IsActive { get; set; }
        public string? TransactionType { get; set; }
        public double? KioskRemainingMoney { get; set; }
    }
}
