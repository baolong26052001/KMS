using System;
using System.Collections.Generic;

namespace KMS.Models
{
    public partial class InsuranceType
    {
        public int Id { get; set; }
        public string? TypeName { get; set; }
        public DateTime? DateModified { get; set; }
        public DateTime? DateCreated { get; set; }
    }
}
