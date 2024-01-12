using Insurance.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Insurance.ViewModel
{
    internal class WelcomeVM :ViewModelBase
    {
        public ICommand CancelCommand { get; set; }

        public WelcomeVM() {
            CancelCommand = new RelayCommand((parameter) =>
            {
                MainWindowVM.Instance.CurrentView = new HomeVM();
            });
        }
    }
}
