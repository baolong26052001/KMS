using System;
using System.Collections.Generic;

namespace KMS.Tools
{
    public partial class InsuranceTypeModel
    {
        public int Id { get; set; }
        public string? TypeName { get; set; }
        public int? UserId { get; set; }
        public DateTime? DateModified { get; set; }
        public DateTime? DateCreated { get; set; }
    }
}
