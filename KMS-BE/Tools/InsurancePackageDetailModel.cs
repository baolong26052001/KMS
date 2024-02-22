using System;
using System.Collections.Generic;

namespace KMS.Tools
{
    public partial class InsurancePackageDetailModel
    {
        public int Id { get; set; }
        public int? PackageHeaderId { get; set; }
        public int? AgeRangeId { get; set; }
        public int? Fee { get; set; }
        public int? UserId { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
    }
}
