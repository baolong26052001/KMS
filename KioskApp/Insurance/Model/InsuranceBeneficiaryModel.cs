using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insurance.Model
{
    public class InsuranceBeneficiaryModel
    {
        public int id { get; set; }
        public int memberId { get; set; }
        public int transactionId { get; set; }

        public string beneficiaryName { get; set; }
        public string beneficiaryId {  get; set; }
        public string relationship { get; set; }
        public DateTime birthday { get; set; }
        public string gender { get; set; }
        public string address { get; set; }
        public string occupation { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string ?taxCode { get; set; }
    }
}
