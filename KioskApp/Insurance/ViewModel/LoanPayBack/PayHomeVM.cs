using Insurance.Command;
using Insurance.Model;
using Insurance.Utility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Insurance.ViewModel
{
    public class PayHomeVM : ViewModelBase
    {
        public ICommand PackageCommand { get; set; }
        public ICommand NextCommand { get; set; }
        public ICommand BackCommand { get; set; }

        private List<LoanPaybackModel> loanData;



        private ObservableCollection<LoanPaybackModel> _loanInfo;
        public ObservableCollection<LoanPaybackModel> LoanInfo
        {
            get { return _loanInfo; }
            set
            {
                _loanInfo = value;
                OnPropertyChanged(nameof(LoanInfo));
            }
        }

        private ObservableCollection<LoanPaybackModel> _loanDetails;
        public ObservableCollection<LoanPaybackModel> LoanDetails
        {
            get { return _loanDetails; }
            set
            {
                _loanDetails = value;
                OnPropertyChanged(nameof(LoanDetails));
            }
        }

        public int LoanPackageID = -1;
        public PayHomeVM()
        {
            int _status = 1; // Khoan vay van con hieu luc
            _FetchLoans(UserModel.Instance.UserID , _status);

            //_FetchDefaultLoanPackage();

            NextCommand = new RelayCommand<object>((p) => { if (LoanPackageID != -1) return true; else return false; }, (p) =>
            {
                MainWindowVM.Instance.CurrentView = new PaymentMethodVM();
            });

            BackCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                MainWindowVM.Instance.BackScreen();
            });

            PackageCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                LoanPackageID = (int)p;
                _FetchLoanDetails(LoanPackageID);
            });

        }

        public void _FetchLoans(int memberID, int status)
        {
            var a = APIHelper.Instance.FetchLoans(memberID, status);
            if (a == null) { MessageBox.Show("Value can not be null"); return; }

            loanData = a;
            LoanPackageID = loanData[0].loanId;
     
            UpdatePackageInfo(a);
        }

        public void _FetchLoanDetails(int loanId)
        {
            var a = APIHelper.Instance.FetchLoanDetails(loanId);
            if (a == null) { MessageBox.Show("Value can not be null"); return; }

            UserModel.Instance.DebtAmount = a[0].totalDebtMustPay;

            UpdatePackageDetails(a);
        }

        public void UpdatePackageInfo(List<LoanPaybackModel> loanInfo)
        {
            LoanInfo = new ObservableCollection<LoanPaybackModel>(loanInfo);
        }



        public void UpdatePackageDetails(List<LoanPaybackModel> loanDetails)
        {
            LoanDetails = new ObservableCollection<LoanPaybackModel>(loanDetails);
        }

        private void _FetchDefaultLoanPackage()
        {
            _FetchLoanDetails(LoanPackageID);
        }
    }
}
