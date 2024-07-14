using Insurance.Command;
using Insurance.Model;
using System.Windows.Input;

namespace Insurance.ViewModel
{
    class PaymentMethodVM:ViewModelBase
    {
        public ICommand CashDepositCommand { get; set; }

        public ICommand BankCommand { get; set; }   
        public ICommand MomoCommand { get; set; }
        public ICommand PayMeCommand { get; set; }
        public ICommand BackCommand { get; set; }




        public PaymentMethodVM() 
        {
            CashDepositCommand = new RelayCommand<object>((p) => { return true; },(p) =>
            {
                MainWindowVM.Instance.CurrentView =  new CashDepositVM();
                UserModel.Instance.PaymentMethod = "Tiền Mặt";
            });

            BankCommand = new RelayCommand<object>((p)=> { return true; },(p) =>
            {
                MainWindowVM.Instance.CurrentView = new SelectBankAccountVM();
                UserModel.Instance.PaymentMethod = "Tài khoản ngân hàng";
            });

            MomoCommand = new RelayCommand<object>((p)=> { return true; }, (p) =>
            {
                MainWindowVM.Instance.CurrentView = new QrCodeVM();
                UserModel.Instance.PaymentMethod = "Momo";
            });

            PayMeCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                MainWindowVM.Instance.CurrentView = new QrCodeVM();
                UserModel.Instance.PaymentMethod = "PayMe";
            });
            BackCommand = new RelayCommand<object>((p)=> { return true; }, (p) =>
            {
                MainWindowVM.Instance.BackScreen();
            });
        }
    }
}
