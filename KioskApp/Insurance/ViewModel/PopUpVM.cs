using Insurance.Command;
using Insurance.Model;
using Insurance.VirtualKeyboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Insurance.ViewModel
{
    
    public class PopUpVM : ViewModelBase
    {
        public ICommand ButtonPopupCommand { get; set; }

        private string _popUpTextbox;
        public string PopupTextBox
        {
            get { return _popUpTextbox; }
            set
            {
                _popUpTextbox = value;
                OnPropertyChanged(nameof(PopupTextBox));
            }
        }

        public PopUpVM()
        {
            PopupTextBox = UserModel.Instance.PopUpTextbox;
            ButtonPopupCommand = new RelayCommand<object>((p) => { return true; },(p) =>
            {
                HandlePopUpCommand();
            });
        }

        private void HandlePopUpCommand()
        {
            VirtualKeyboardVM vm = new VirtualKeyboardVM();
            vm.ClosePopup();

            BeneficiaryInfoVM vm1 = new BeneficiaryInfoVM();
            vm1.ClosePopup();

            BuyerInformationVM vm2 = new BuyerInformationVM();
            vm2.ClosePopup();

            PersonalInfoVM vm3 = new PersonalInfoVM();
            vm3.ClosePopup();
        }
    }
}
