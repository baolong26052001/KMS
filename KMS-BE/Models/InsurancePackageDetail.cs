using System;
using System.Collections.Generic;

namespace KMS.Models
{
    public partial class InsurancePackageDetail
    {
        public int Id { get; set; }
        public int? PackageHeaderId { get; set; }
        public int? AgeRangeId { get; set; }
        public int? Fee { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
    }
}
