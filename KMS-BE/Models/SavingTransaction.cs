using System;
using System.Collections.Generic;

namespace KMS.Models
{
    public partial class SavingTransaction
    {
        public int Id { get; set; }
        public int? AccountId { get; set; }
        public int? MemberId { get; set; }
        public int? SavingTerm { get; set; }
        public double? Balance { get; set; }
        public double? AnnualRate { get; set; }
        public DateTime? DueDate { get; set; }
        public int? Status { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
    }
}
