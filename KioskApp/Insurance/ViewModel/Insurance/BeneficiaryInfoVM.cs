using Insurance.Command;
using System.Windows.Input;

namespace Insurance.ViewModel
{
    class BeneficiaryInfoVM:ViewModelBase
    {
        public ICommand NextCommand { get; set; }
        public ICommand BackCommand { get; set; }

        public BeneficiaryInfoVM() 
        {
            MainWindowVM.Instance.setVisibilityHeader();

            NextCommand = new RelayCommand((parameter) =>
            {
                MainWindowVM.Instance.CurrentView = new BeneficiaryAddVM();
            });

            BackCommand = new RelayCommand((parameter) =>
            {
                MainWindowVM.Instance.BackScreen();
            });
        }  
    }
}
