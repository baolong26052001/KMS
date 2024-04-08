using System;
using System.Collections.Generic;

namespace KMS.Models
{
    public partial class SavingTransaction
    {
        public int Id { get; set; }
        public int? MemberId { get; set; }
        //public int? ContractId { get; set; }
        //public int? SavingId { get; set; }
        public int? TopUp { get; set; }
        //public int? Balance { get; set; }
        public int? SavingTerm { get; set; }
        public double? SavingRate { get; set; }
        
        public DateTime? TransactionDate { get; set; }
        public DateTime? DueDate { get; set; }
        public int? Status { get; set; }

    }

    public partial class WithDraw
    {
        //public int WithdrawId { get; set; }
        public int? ContractId { get; set; }
        public int? MemberId { get; set; }
        public int? SavingId { get; set; }
        public int? Withdraw { get; set; }
        //public int? Balance { get; set; }
        public DateTime? TransactionDate { get; set; }

    }

}
