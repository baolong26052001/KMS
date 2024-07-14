using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Insurance.Utility.APIHelper;
using System.Windows;
using Insurance.Model;
using Insurance.Command;
using System.Windows.Input;
using Insurance.Utility;
using System.Windows.Controls;

namespace Insurance.ViewModel
{
    public class ListWithdrawVM : ViewModelBase
    {
        private ObservableCollection<SavingTransactionModel> _fetchSavingHeader;
        public ObservableCollection<SavingTransactionModel> FetchSavingHeader
        {
            get { return _fetchSavingHeader; }
            set
            {
                _fetchSavingHeader = value;
                OnPropertyChanged(nameof(FetchSavingHeader));
            }
        }
        private string fullName { get; set; }
        public string FullName
        {
            get { return fullName; }
            set
            {
                if (fullName != value)
                {
                    fullName = value;

                    OnPropertyChanged(nameof(FullName));

                }
            }
        }
        private string idenNumber { get; set; }
        public string IdenNumber
        {
            get { return idenNumber; }
            set
            {
                if (idenNumber != value)
                {
                    idenNumber = value;

                    OnPropertyChanged(nameof(IdenNumber));

                }
            }
        }
        private int status { get; set; }
        public int Status
        {
            get { return status; }
            set
            {
                if (status != value)
                {
                    status = value;

                    OnPropertyChanged(nameof(Status));

                }
            }
        }

        private int savingTerm { get; set; }
        public int SavingTerm
        {
            get { return savingTerm; }
            set
            {
                if (savingTerm != value)
                {
                    savingTerm = value;

                    OnPropertyChanged(nameof(SavingTerm));

                }
            }
        }

        private string transactionDate { get; set; }
        public string TransactionDate
        {
            get { return transactionDate; }
            set
            {
                if (transactionDate != value)
                {
                    transactionDate = value;

                    OnPropertyChanged(nameof(TransactionDate));

                }
            }
        }
        private string dueDate { get; set; }
        public string DueDate
        {
            get { return dueDate; }
            set
            {
                if (dueDate != value)
                {
                    dueDate = value;

                    OnPropertyChanged(nameof(DueDate));

                }
            }
        }
        private int topUp { get; set; }
        public int TopUp
        {
            get { return topUp; }
            set
            {
                if (topUp != value)
                {
                    topUp = value;

                    OnPropertyChanged(nameof(TopUp));

                }
            }
        }
        private double savingRate { get; set; }
        public double SavingRate
        {
            get { return savingRate; }
            set
            {
                if (savingRate != value)
                {
                    savingRate = value;

                    OnPropertyChanged(nameof(SavingRate));

                }
            }
        }
        private int interestMoney { get; set; }
        public int InterestMoney
        {
            get { return interestMoney; }
            set
            {
                if (interestMoney != value)
                {
                    interestMoney = value;

                    OnPropertyChanged(nameof(InterestMoney));

                }
            }
        }

        private string balance { get; set; }
        public string  Balance
        {
            get { return FormatWithdraw(balance); }
            set
            {
                if (balance != value)
                {
                    balance = value;
                    OnPropertyChanged(nameof(Balance));
                }
            }
        }

        private string FormatWithdraw(string value)
        {
            if (string.IsNullOrEmpty(value))
                return "";
            if (!decimal.TryParse(value, out decimal withdrawDecimal))
                return "";

            string formattedWithdraw = string.Format("{0:N0}", withdrawDecimal).Replace(",", ".");

            return formattedWithdraw;
        }

        public ICommand UpdateButtonClickCommand => new RelayCommand(UpdateButtonClick);
        public ICommand BackCommand { get; set; }
        public ListWithdrawVM()
        {
            UserModel.Instance.UserID = 37;
            LoadData(UserModel.Instance.UserID, 0);
            BackCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                MainWindowVM.Instance.BackScreen();
            });
        }
        private void LoadData(int memberId, int status)
        {
            try
            {
                List<SavingTransactionModel> headers = Instance.FetchSavingHeader(memberId, status);

                if (headers == null)
                {
                    headers = new List<SavingTransactionModel>();
                }
                else
                {
                    foreach (var header in headers)
                    {
                        header.FormattedtopUp = header.topUp.ToString("N0").Replace(",", ".");
                    }
                }

                FetchSavingHeader = new ObservableCollection<SavingTransactionModel>(headers);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
     
        private void UpdateButtonClick(object parameter)
        {
            if (parameter is SavingTransactionModel selectedHeader)
            {
                UserModel.Instance.IdSavingSelected = selectedHeader.savingId;
                //MessageBox.Show($"{selectedHeader.id}");
                FetchSavingDetail(UserModel.Instance.UserID, 0, UserModel.Instance.IdSavingSelected);
                FetchPersonInfo(UserModel.Instance.UserID);
            }
            else
            {
                MessageBox.Show("Invalid parameter passed.");
            }
        }
        public void FetchPersonInfo(int memberId)
        {
            var b = APIHelper.Instance.FetchUserInfo(memberId);
            if (b == null) { MessageBox.Show("Value can not be null"); return; }
            UpdateUserInfo(b);
        }
        public void UpdateUserInfo(List<UserInfoModel> userInfo)
        {
            FullName = userInfo[0].fullName;
            IdenNumber = userInfo[0].idenNumber;
            UserModel.Instance.FullName = userInfo[0].fullName;
        }
        public void FetchSavingDetail(int memberId,int status, int savingId)
        {
            var a = APIHelper.Instance.FetchSavingDetail(memberId, status, savingId);
            if (a == null) { MessageBox.Show("Value can not be null"); return; }   
            UpdateSavingDetail(a);
        }

        private void UpdateSavingDetail(List<SavingTransactionModel> InfoPack)
        {         
            SavingTerm = InfoPack[0].savingTerm;
            SavingRate = InfoPack[0].savingRate*100;
            TransactionDate = InfoPack[0].transactionDate.ToShortDateString();
            DueDate = InfoPack[0].dueDate.ToShortDateString();
            Balance = InfoPack[0].balance;
        }
       
    }
}
