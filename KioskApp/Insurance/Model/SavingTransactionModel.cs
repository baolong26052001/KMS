using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insurance.Model
{
    public class SavingTransactionModel
    {
        public int id { get; set; }
        public int memberId { get; set; }
        public int topUp { get; set; }
        public int savingTerm { get; set; }
        public double savingRate { get; set; }
        public int status { get; set; }
        public int savingId {  get; set; }
        public DateTime transactionDate { get; set; }
        public DateTime dueDate { get; set; }
        public int contractId { get; set; }

        public string FormattedtopUp { get; set; }
        public string balance { get; set; }
        public bool IsFirstItem { get; set; }

    }
}
