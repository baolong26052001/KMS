using System;
using System.Collections.Generic;

namespace KMS.Models
{
    public partial class Term
    {
        public int Id { get; set; }
        public string? Content { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
    }
}
