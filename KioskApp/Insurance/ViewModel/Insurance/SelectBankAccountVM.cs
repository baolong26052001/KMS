using Insurance.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Insurance.ViewModel
{
    class SelectBankAccountVM:ViewModelBase
    {
        public ICommand AgreeCommand { get; set; }  
        public ICommand CancelCommand { get; set; }
        public ICommand BackCommand { get; set; }


        public SelectBankAccountVM()
        {
            AgreeCommand = new RelayCommand((parameter) =>
            {
                MainWindowVM.Instance.CurrentView = new OTPVM();
            });

            CancelCommand = new RelayCommand((parameter) =>
            {
                MainWindowVM.Instance.CurrentView = new InsuranceHomeVM();
            });
          
            BackCommand = new RelayCommand((parameter) =>
            {
                MainWindowVM.Instance.BackScreen();
            });
        }
    }
}
