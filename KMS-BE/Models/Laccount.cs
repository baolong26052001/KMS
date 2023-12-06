using System;
using System.Collections.Generic;

namespace KMS.Models
{
    public partial class Laccount
    {
        public int Id { get; set; }
        public string? ContractId { get; set; }
        public int? MemberId { get; set; }
        public string? AccountName { get; set; }
        public string? AccountType { get; set; }
        public double? Balance { get; set; }
        public double? Rate { get; set; }
        public DateTime? DateDue { get; set; }
        public int? Status { get; set; }
        public DateTime? DateModified { get; set; }
        public DateTime? DateCreated { get; set; }
        public bool? IsActive { get; set; }
    }
}
