using Insurance.Command;
using Insurance.Model;
using Insurance.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Insurance.ViewModel
{
    class QrCodeVM:ViewModelBase
    {
        public ICommand BackCommand { get; set; }
        public ICommand NextCommand { get; set; }


        public QrCodeVM() 
        {
            NextCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                MainWindowVM.Instance.CurrentView = new BeneficiaryAddVM();
                MainWindowVM.Instance.VisibilityBtnHead = Visibility.Collapsed;
                APIHelper.Instance.Log($"Insurance Payment Success, MemberID: {UserModel.Instance.UserID}, Member Name: {UserModel.Instance.FullName}, Member ID card number: {UserModel.Instance.IDCard}, Paid: {UserModel.Instance.InsurancePackageName.fee}");
            });
            BackCommand = new RelayCommand<object>((p)=> { return true; }, (p) =>
            {
                MainWindowVM.Instance.BackScreen();
            });
        }
    }
}
