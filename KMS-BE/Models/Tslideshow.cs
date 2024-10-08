﻿using System;
using System.Collections.Generic;

namespace KMS.Models
{
    public partial class Tslideshow
    {
        public int Id { get; set; }
        public int? StationId { get; set; }
        public string? PackageName { get; set; }
        public string? Imagevideo { get; set; }
        public string? FileType { get; set; }
        public string? Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool? IsActive { get; set; }
        public int? Timer { get; set; }
        public int? Sequence { get; set; }
        public string? Scrolltext1 { get; set; }
        public string? Scrolltext2 { get; set; }
        public DateTime? CreateAt { get; set; }
        public int? CreateBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? DateModified { get; set; }
        public DateTime? DateCreated { get; set; }
    }
}
