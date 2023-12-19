using System;
using System.Collections.Generic;

namespace KMS.Models
{
    public partial class Benefit
    {
        public int Id { get; set; }
        public string? Content { get; set; }
        public double? Fee { get; set; }
        public string? Description { get; set; }
        public int? PackageId { get; set; }
        public DateTime? DateModified { get; set; }
        public DateTime? DateCreated { get; set; }
    }
}
