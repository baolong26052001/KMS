using Insurance.Command;
using System.Windows.Input;

namespace Insurance.ViewModel
{
    public class HomeVM : ViewModelBase
    {
        public ICommand InsuranceCommand { get; set; }
        public void Nav2InsuranceView(object parameter)
        {
            MainWindowVM.Instance.CurrentView = new InsuranceHomeVM();
        }
        public HomeVM()
        {
            InsuranceCommand = new RelayCommand(Nav2InsuranceView);
        }

    }
}
