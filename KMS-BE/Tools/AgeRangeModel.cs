using System;
using System.Collections.Generic;

namespace KMS.Tools
{
    public partial class AgeRangeModel
    {
        public int Id { get; set; }
        public int? StartAge { get; set; }
        public int? EndAge { get; set; }
        public int? UserId { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
    }
}
