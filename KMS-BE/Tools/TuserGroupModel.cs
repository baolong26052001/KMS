using System;
using System.Collections.Generic;

namespace KMS.Tools
{
    public partial class TuserGroupModel
    {
        public int Id { get; set; }
        public string? GroupName { get; set; }
        public int? AccessRuleId { get; set; }
        public DateTime? DateModified { get; set; }
        public DateTime? DateCreated { get; set; }
        public bool? IsActive { get; set; }
        public int? UserId { get; set; }
    }
    public partial class TusergroupModel
    {
        public int Id { get; set; }
        public string? GroupName { get; set; }

        public DateTime? DateModified { get; set; }
        public DateTime? DateCreated { get; set; }
        public bool? IsActive { get; set; }
        public int? UserId { get; set; }
    }
}
