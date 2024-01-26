using System;
using System.Collections.Generic;

namespace KMS.Models
{
    public partial class InsurancePackageHeader
    {
        public int Id { get; set; }
        public string? PackageName { get; set; }
        public int? InsuranceTypeId { get; set; }
        public int? TermId { get; set; }
        public int? InsuranceProviderId { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public bool? IsActive { get; set; }
        public int? Priority { get; set; }
    }
}
