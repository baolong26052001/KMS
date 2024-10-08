﻿using System;
using System.Collections.Generic;

namespace KMS.Models
{
    public partial class InsurancePackage
    {
        public int Id { get; set; }
        public string? PackageName { get; set; }
        public int? InsuranceType { get; set; }
        public string? Provider { get; set; }
        public int? Duration { get; set; }
        public string? PayType { get; set; }
        public int? Fee { get; set; }
        public DateTime? DateModified { get; set; }
        public DateTime? DateCreated { get; set; }
    }
}
