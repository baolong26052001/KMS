using System;
using System.Collections.Generic;

namespace KMS.Models
{
    public partial class AgeRange
    {
        public int Id { get; set; }
        public int? StartAge { get; set; }
        public int? EndAge { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
    }
}
