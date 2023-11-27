using System;
using System.Collections.Generic;

namespace KMS.Models
{
    public partial class Tstation
    {
        public int Id { get; set; }
        public string? StationName { get; set; }
        public string? CompanyName { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public DateTime? DateModified { get; set; }
        public DateTime? DateCreated { get; set; }
        public bool? IsActive { get; set; }
    }
}
