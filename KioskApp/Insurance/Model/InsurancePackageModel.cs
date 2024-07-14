using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insurance.Model
{
    public class InsurancePackageModel
    {
        public int id { get; set; }
        public int headerId { get; set; }
        public string packageName { get; set; }
        public string provider { get; set; }
        public string email { get; set; }
        public string content { get; set; }
        public string typeName { get; set; }
        //public bool isActive { get; set; }
        public int fee { get; set; }
        public DateTime dateModified { get; set; }
        public DateTime dateCreated { get; set; }
        public List <InsurancePackageFee> PackageFee { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private Brush _backgroundColor;
        public Brush BackgroundColor
        {
            get { return _backgroundColor; }
            set
            {
                _backgroundColor = value;
                OnPropertyChanged(nameof(BackgroundColor));
            }
        }
        public bool IsSelected { get; set; }
    }

    public class InsurancePackageFee
    {
        public int id;
        public string packageName;
        public int fee;
    }


}
