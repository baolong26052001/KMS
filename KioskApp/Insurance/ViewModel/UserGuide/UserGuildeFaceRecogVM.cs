using Insurance.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Insurance.ViewModel
{
    public class UserGuildeFaceRecogVM : ViewModelBase
    {
        public ICommand NextCommand { get; set; }
        public ICommand BackCommand { get; set; }
        public UserGuildeFaceRecogVM() {
            BackCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                MainWindowVM.Instance.BackScreen();
            });
            NextCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                MainWindowVM.Instance.CurrentView = new FaceRecogStatusLoginVM();
            });
        }
    }
}
