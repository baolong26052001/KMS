using System;
using System.Collections.Generic;

namespace KMS.Tools
{
    public partial class TslideHeaderModel
    {
        public int Id { get; set; }
        public string? Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool? IsActive { get; set; }
        public int? TimeNext { get; set; }
        public int? UserId { get; set; }
        public DateTime? DateModified { get; set; }
        public DateTime? DateCreated { get; set; }
    }
}
