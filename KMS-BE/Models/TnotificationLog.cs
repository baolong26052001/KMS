using System;
using System.Collections.Generic;

namespace KMS.Models
{
    public partial class TnotificationLog
    {
        public int Id { get; set; }
        public int? Type { get; set; }
        public string? SendType { get; set; }
        public int? MemberId { get; set; }
        public string? SocketKey { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public int? Status { get; set; }
        public DateTime? DateModified { get; set; }
        public DateTime? DateCreated { get; set; }
        public bool? IsActive { get; set; }
    }
}
