using Insurance.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Insurance.ViewModel
{
    public class SavingHomeVM : ViewModelBase
    {
       public ICommand BackCommand { get; set; }
        public SavingHomeVM()
        {
            BackCommand = new RelayCommand<object>((p) => { return true; }, (p) => 
            {
                MainWindowVM.Instance.BackScreen();
            });

        }
    }
}
