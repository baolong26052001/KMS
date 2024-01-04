using Insurance.Command;
using System.Windows.Input;

namespace Insurance.ViewModel
{
    class BeneficiaryAddVM : ViewModelBase
    {
        public ICommand AddBeneficiaryCommand { get; set; }

        public ICommand DoneCommand { get; set; }

        //public void RR(object parameter)
        //{

        //}
        public BeneficiaryAddVM()
        {            

            // BackCommand = new RelayCommand(RR);
            AddBeneficiaryCommand = new RelayCommand((parameter) =>
            {
                //MainWindowVM.Instance.CurrentView = new InsuranceHomeVM();
                MainWindowVM.Instance.CurrentView = new BeneficiaryInfoVM();
            });

            DoneCommand = new RelayCommand((parameter) =>
            {
                MainWindowVM.Instance.CurrentView = new InsuranceSuccessVM();
            });
        }
    }
}
