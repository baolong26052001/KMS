using Insurance.Command;
using Insurance.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Insurance.ViewModel
{
    public class RegisterSuccessVM : ViewModelBase
    {
        public ICommand NextButtonClickCommand => new RelayCommand(NextButtonClick);
        public HomeVM HomeViewModel => HomeVM.Instance;
        public RegisterSuccessVM()
        {
            MainWindowVM.Instance.VisibilityHeader = Visibility.Collapsed;
            MainWindowVM.Instance.VisibilityFooter = Visibility.Collapsed;


        }
        private void NextButtonClick(object parameter)
        {
            MainWindowVM.Instance.setDefaultLayout();
            MainWindowVM.Instance.CurrentView = HomeViewModel;
            HomeViewModel.FirstName = UserModel.Instance.FirstName;
            HomeViewModel.LastName = UserModel.Instance.LastName;
            HomeViewModel.FullName = UserModel.Instance.FullName;
            UserModel.Instance.IsLogin = true;
            UserModel.Instance.IsHomePage = true;
        }

    }
}
