using System;
using System.Collections.Generic;

namespace KMS.Models
{
    public partial class SavingStatement
    {
        public int Id { get; set; }
        public int? MemberId { get; set; }
        public int? AccountId { get; set; }
        public int? SavingId { get; set; }
        public double? Balance { get; set; }
        public int? Period { get; set; }
        public double? AnnualRate { get; set; }
        public string? Description { get; set; }
        public int? Status { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? DateSaving { get; set; }
    }
}
