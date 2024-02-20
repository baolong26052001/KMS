using System;
using System.Collections.Generic;

namespace KMS.Tools
{
    public partial class EditBenefit
    {
        public int Id { get; set; }
        public string? Content { get; set; }
        public string? Coverage { get; set; }
        public string? Description { get; set; }
        public int? UserId { get; set; }
        public int? PackageId { get; set; }
        public DateTime? DateModified { get; set; }
        public DateTime? DateCreated { get; set; }
    }
}
