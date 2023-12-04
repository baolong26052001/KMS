using System;
using System.Collections.Generic;

namespace KMS.Models
{
    public partial class TactivityLog
    {
        public int Id { get; set; }
        public int? KioskId { get; set; }
        public string? HardwareName { get; set; }
        public int? Status { get; set; }
        public int? StationId { get; set; }
        public DateTime? DateModified { get; set; }
        public DateTime? DateCreated { get; set; }
        public bool? IsActive { get; set; }
    }
}
