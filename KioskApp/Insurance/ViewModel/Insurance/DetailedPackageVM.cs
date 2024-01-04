using Insurance.Command;
using System.Windows.Input;

namespace Insurance.ViewModel
{
    class DetailedPackageVM : ViewModelBase
    {
        public ICommand NextCommand { get; set; }
        public ICommand BackCommand { get; set; }
        public ICommand CancelCommand { get; set; }

        public DetailedPackageVM()
        {
            NextCommand = new RelayCommand((parameter) =>
            {
                MainWindowVM.Instance.CurrentView = new PaymentMethodVM();
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
