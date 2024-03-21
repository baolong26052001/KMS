using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace KMS.Tools
{
    public partial class InsuranceProviderModel
    {
        public int? Id { get; set; }
        public string? Provider { get; set; }
        public string? Email { get; set; }

        public string? ProviderImage { get; set; }

        [NotMapped]
        public IFormFile? ProviderImageFile { get; set; }

        public int? UserId { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
    }
}
