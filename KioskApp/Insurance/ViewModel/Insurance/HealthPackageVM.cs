using Insurance.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Insurance.ViewModel
{
    class HealthPackageVM : ViewModelBase
    {
         public ICommand PurchaseCommand { get; set; }
       
        public ICommand CancelCommand { get; set; }

        public HealthPackageVM()
        {
            PurchaseCommand = new RelayCommand((parameter) =>
            {
                MainWindowVM.Instance.CurrentView = new BuyerInformationVM();
            });
           
            CancelCommand = new RelayCommand((parameter) =>
            {
                MainWindowVM.Instance.CurrentView = new InsuranceHomeVM();
            });
        }
    }

}
