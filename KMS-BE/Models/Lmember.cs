using System;
using System.Collections.Generic;

namespace KMS.Models
{
    public partial class Lmember
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? FullName { get; set; }
        public DateTime? Birthday { get; set; }
        public string? Phone { get; set; }
        public string? Address1 { get; set; }
        public string? Address2 { get; set; }
        public string? District { get; set; }
        public string? City { get; set; }
        public string? Ward { get; set; }
        public string? ImageIdCard { get; set; }
        public string? Fingerprint1 { get; set; }
        public string? Fingerprint2 { get; set; }
        public string? IdenNumber { get; set; }
        public string? BankName { get; set; }
        public string? BankNumber { get; set; }
        public int? RefCode { get; set; }
        public string? CompanyName { get; set; }
        public string? CompanyAddress { get; set; }
        public string? Department { get; set; }
        public double? SalaryAmount { get; set; }
        public double? CreditLimit { get; set; }
        public DateTime? StatementDate { get; set; }
        public DateTime? SettleDate { get; set; }
        public string? DebtStatus { get; set; }
        public DateTime? DateModified { get; set; }
        public DateTime? DateCreated { get; set; }
        public bool? IsActive { get; set; }
        public string? Gender { get; set; }
        public string? ImageMember { get; set; }
        public string? Occupation { get; set; }
        public string? Email { get; set; }
    }
}
