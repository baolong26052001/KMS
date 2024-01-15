﻿using System;
using System.Collections.Generic;

namespace KMS.Models
{
    public partial class TuserGroup
    {
        public int Id { get; set; }
        public string? GroupName { get; set; }
        public int? AccessRuleId { get; set; }
        public DateTime? DateModified { get; set; }
        public DateTime? DateCreated { get; set; }
        public bool? IsActive { get; set; }
    }
    public partial class Tusergroup
    {
        public int Id { get; set; }
        public string? GroupName { get; set; }

        public DateTime? DateModified { get; set; }
        public DateTime? DateCreated { get; set; }
        public bool? IsActive { get; set; }
    }
}
