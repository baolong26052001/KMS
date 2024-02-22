using System;
using System.Collections.Generic;

namespace KMS.Tools
{
    public partial class TermModel
    {
        public int Id { get; set; }
        public string? Content { get; set; }
        public int? UserId { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
    }
}
