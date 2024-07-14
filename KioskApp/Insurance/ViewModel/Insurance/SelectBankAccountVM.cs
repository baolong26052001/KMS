using Insurance.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Insurance.ViewModel
{
    class SelectBankAccountVM:ViewModelBase
    {
        public ICommand AgreeCommand { get; set; }  
        public ICommand BackCommand { get; set; }


        public SelectBankAccountVM()
        {
            AgreeCommand = new RelayCommand<object>((p)=> { return true; }, (p) =>
            {
                MainWindowVM.Instance.CurrentView = new OTPVM();
            });

          
            BackCommand = new RelayCommand<object>((p) => { return true; }, (p)=>
            {
                MainWindowVM.Instance.BackScreen();
            });
        }
    }
}
