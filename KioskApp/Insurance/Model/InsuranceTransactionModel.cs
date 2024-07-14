using System;

namespace Insurance.Model
{
    public class InsuranceTransactionModel
    {
        public int id { get; set; }
        public int memberId { get; set; }
        public int contractId { get; set; }
        public int packageDetailId { get; set; }
        public DateTime registrationDate { get; set; }
        public DateTime expireDate { get; set; }
        public int annualPay { get; set; }
        public int status { get; set; }
        public DateTime transactionDate { get; set; } 
        public string paymentMethod { get; set; }
    }
}
