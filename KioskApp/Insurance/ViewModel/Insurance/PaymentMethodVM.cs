using Insurance.Command;
using System.Windows.Input;

namespace Insurance.ViewModel
{
    class PaymentMethodVM:ViewModelBase
    {
        public ICommand CashDepositCommand { get; set; }

        public ICommand BankCommand { get; set; }   
        public ICommand MomoCommand { get; set; }
        public ICommand PayMeCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        public ICommand BackCommand { get; set; }




        public PaymentMethodVM() 
        {
            CashDepositCommand = new RelayCommand((parameter) =>
            {
                MainWindowVM.Instance.CurrentView =  new CashDepositVM();
            });

            BankCommand = new RelayCommand((parameter) =>
            {
                MainWindowVM.Instance.CurrentView = new SelectBankAccountVM();
            });

            MomoCommand = new RelayCommand((parameter) =>
            {
                MainWindowVM.Instance.CurrentView = new QrCodeVM();
            });

            PayMeCommand = new RelayCommand((parameter) =>
            {
                MainWindowVM.Instance.CurrentView = new QrCodeVM();
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
