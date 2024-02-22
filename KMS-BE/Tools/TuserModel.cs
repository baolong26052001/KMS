using System;
using System.Collections.Generic;

namespace KMS.Tools
{
    public partial class TuserModel
    {
        public int Id { get; set; }
        public string? Username { get; set; }
        public string? Fullname { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
        public int? UserGroupId { get; set; }
        public DateTime? LastLogin { get; set; }
        public string? CardNum { get; set; }
        public string? RefCode { get; set; }
        public DateTime? DateModified { get; set; }
        public DateTime? DateCreated { get; set; }
        public bool? IsActive { get; set; }
        public int? UserId { get; set; }
    }
}
