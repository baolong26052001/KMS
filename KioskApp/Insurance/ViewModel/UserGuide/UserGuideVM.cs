using Insurance.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Insurance.ViewModel
{
    public class UserGuideVM:ViewModelBase
    {
        public ICommand NextCommand { get; set; }
        public UserGuideVM() {
           
            NextCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                MainWindowVM.Instance.CurrentView = new UserGuildeFaceRecogVM();
            });
        }
    }
}
