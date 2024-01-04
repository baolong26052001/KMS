using Insurance.Command;
using System.Windows.Input;

namespace Insurance.ViewModel
{
    class InsuranceHomeVM : ViewModelBase
    {
        public ICommand HealthPackageCommand { get; set; }
        public ICommand AccidentPackageCommand { get; set; }
        public ICommand CancelCommand { get; set; }



        public InsuranceHomeVM()
        {
            HealthPackageCommand = new RelayCommand((parameter) =>
            {
                MainWindowVM.Instance.CurrentView = new HealthPackageVM();
            });

            AccidentPackageCommand = new RelayCommand((parameter) =>
            {
                MainWindowVM.Instance.CurrentView = new AccidentPackageVM();
            });

            CancelCommand = new RelayCommand((parameter) =>
            {
                MainWindowVM.Instance.CurrentView = new HomeVM();
            });

        }
    }
}
