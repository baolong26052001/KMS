using System;
using System.Collections.Generic;

namespace KMS.Models
{
    public partial class TaccessRule
    {
        public int Id { get; set; }
        public int? GroupId { get; set; }
        public int? FeatureName { get; set; }
        public DateTime? DateModified { get; set; }
        public DateTime? DateCreated { get; set; }
        public bool? IsActive { get; set; }
    }
}
