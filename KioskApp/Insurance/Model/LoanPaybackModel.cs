using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insurance.Model
{
    public class LoanPaybackModel
    {
        public int id { get; set; }
        public int contractId { get; set; }
        public int loanId { get; set; }
        public int loanTerm { get; set; }
        public double loanRate { get; set;}
        public int debt { get; set; }
        public int totalDebtMustPay { get; set; }   
        public string transactionDate { get; set; }
        public string dueDate { get; set; }
        public int status { get; set; }


        public string LoanPackageName { get; set; }// Cai nay tu them vao de hien ten goi Loan

    }
}
