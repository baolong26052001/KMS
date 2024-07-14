using Insurance.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Insurance.ViewModel
{
    public class UGFaceRecog_SUStatusVM : ViewModelBase
    {
        public ICommand NextCommand { get; set; }
        public ICommand BackCommand { get; set; }
        public UGFaceRecog_SUStatusVM()
        {
            BackCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                MainWindowVM.Instance.BackScreen();
            });
            NextCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                MainWindowVM.Instance.CurrentView = new UGSuccessSUVM();
            });
        }
    }
}
