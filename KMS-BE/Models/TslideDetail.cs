using System;
using System.Collections.Generic;

namespace KMS.Models
{
    public partial class TslideDetail
    {
        public int Id { get; set; }
        public string? Description { get; set; }
        public string? TypeContent { get; set; }
        public string? ContentUrl { get; set; }
        public int? SlideHeaderId { get; set; }
        public DateTime? DateModified { get; set; }
        public DateTime? DateCreated { get; set; }
        public bool? IsActive { get; set; }
    }
}
