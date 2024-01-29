using System;
using System.Collections.Generic;

namespace KMS.Models
{
    public partial class TaccessRule
    {
        public int Id { get; set; }
        public int? GroupId { get; set; }
        public string? FeatureName { get; set; }
        public DateTime? DateModified { get; set; }
        public DateTime? DateCreated { get; set; }
        public bool? IsActive { get; set; }
        public bool? CanView { get; set; }
        public bool? CanAdd { get; set; }
        public bool? CanUpdate { get; set; }
        public bool? CanDelete { get; set; }
        public string? Site { get; set; }
    }
}
