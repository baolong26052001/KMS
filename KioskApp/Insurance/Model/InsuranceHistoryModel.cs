using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insurance.Model
{
    public class InsuranceHistoryModel
    {
        public string transactionDate { get; set; }
        public string id { get; set; }
        public string memberId { get; set; }
        public string fullName { get; set; }
        public string idenNumber { get; set; }
        public string phone { get; set; }
        public string contractId { get; set; }
        public string packageId { get; set; }
        public string packageName { get; set; }
        public string typeName { get; set; }
        public string termName { get; set; }
        public string provider { get; set; }
        public string annualPay { get; set; }
        public string registrationDate { get; set; }
        public string expireDate { get; set; }
        public string status { get; set; }
        public string paymentMethod { get; set; }
        public string beneficiaryName { get; set; }
    }
}
