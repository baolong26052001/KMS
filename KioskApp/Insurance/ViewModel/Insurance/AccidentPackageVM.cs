using Insurance.Command;
using Insurance.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;

namespace Insurance.ViewModel
{
    class AccidentPackageVM : ViewModelBase
    {
        public ICommand PurchaseCommand { get; set; }
        public ICommand CancelCommand { get; set; }


        public AccidentPackageVM()
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




