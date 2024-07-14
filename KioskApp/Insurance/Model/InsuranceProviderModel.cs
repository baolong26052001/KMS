using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Insurance.Model
{
    public class InsuranceProviderModel
    {
        public int id { get; set; }
        public string provider { get; set; }
        public string email { get; set; }
        public string providerImage { get; set; }
        public BitmapImage ProviderImageBitmap { get; set; }

    }
}
