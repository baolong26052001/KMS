using Insurance.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insurance.Model.HTTPResponses
{
    public class HTTPResponses
    {
        public int Status { get; set; }
        public string Message { get; set; }
        public object RData { get; set; }
        public string Description { get; set; }
    }
}
