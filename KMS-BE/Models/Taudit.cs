using System;
using System.Collections.Generic;

namespace KMS.Models
{
    public partial class Taudit
    {
        public int Id { get; set; }
        public int? KioskId { get; set; }
        public int? UserId { get; set; }
        public string? Action { get; set; }
        public string? Script { get; set; }
        public string? Field { get; set; }
        public string? TableName { get; set; }
        public string? IpAddress { get; set; }
        public string? MacAddress { get; set; }
        public DateTime? DateModified { get; set; }
        public DateTime? DateCreated { get; set; }
        public bool? IsActive { get; set; }
    }
}
