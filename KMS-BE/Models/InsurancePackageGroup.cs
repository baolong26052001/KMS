using System;
using System.Collections.Generic;

namespace KMS.Models
{
    public partial class InsurancePackageGroup
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public DateTime? DateModified { get; set; }
        public DateTime? DateCreated { get; set; }
    }
}
