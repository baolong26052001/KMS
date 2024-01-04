using Insurance.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Insurance.ViewModel
{
    class BuyerInformationVM:ViewModelBase
    {
        public ICommand NextCommand { get; set; }
        
        public ICommand CancelCommand { get; set; }
        public ICommand BackCommand { get; set; }

        public BuyerInformationVM() 
        {
            NextCommand = new RelayCommand((parameter) => 
            { 
                MainWindowVM.Instance.CurrentView = new ContractInfoVM();
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
