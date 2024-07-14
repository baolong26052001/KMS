using Insurance.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Insurance.ViewModel
{
    public class LoanPaymentVM : ViewModelBase
    {
        public ICommand CashDepositCommand { get; set; }
        public ICommand BackCommand { get; set; }
        public LoanPaymentVM()
        {
            CashDepositCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                MainWindowVM.Instance.CurrentView = new CashDepositVM();
            });

            BackCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                MainWindowVM.Instance.BackScreen();
            });

        }
    }
}
