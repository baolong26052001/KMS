using System;
using System.Collections.Generic;

namespace KMS.Models
{
    public partial class InsuranceProvider
    {
        public int Id { get; set; }
        public string? Provider { get; set; }
        public string? Email { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
    }
}
