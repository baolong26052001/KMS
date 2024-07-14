using Insurance.Utility;
using System;
using System.Collections.Generic;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using Insurance.Model;
using System.Windows.Input;
using Insurance.Command;

namespace Insurance.ViewModel
{
    public class InsuranceHistoryVM : ViewModelBase
    {
        public ICommand BackCommand { get; set; }
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
        private string phoneNum { get; set; }
        public string PhoneNum
        {
            get { return phoneNum; }
            set
            {
                if (phoneNum != value)
                {
                    phoneNum = value;

                    OnPropertyChanged(nameof(PhoneNum));

                }
            }
        }
        private string transdate { get; set; }
        public string Transdate
        {
            get { return transdate; }
            set
            {
                if (transdate != value)
                {
                    transdate = value;

                    OnPropertyChanged(nameof(Transdate));

                }
            }
        }
        private string transId { get; set; }
        public string TransId
        {
            get { return transId; }
            set
            {
                if (transId != value)
                {
                    transId = value;

                    OnPropertyChanged(nameof(TransId));

                }
            }
        }
        private string memId { get; set; }
        public string MemId
        {
            get { return memId; }
            set
            {
                if (memId != value)
                {
                    memId = value;

                    OnPropertyChanged(nameof(MemId));

                }
            }
        }
        private string contractId { get; set; }
        public string ContractId
        {
            get { return contractId; }
            set
            {
                if (contractId != value)
                {
                    contractId = value;

                    OnPropertyChanged(nameof(ContractId));

                }
            }
        }
        private string packId { get; set; }
        public string PackId
        {
            get { return packId; }
            set
            {
                if (packId != value)
                {
                    packId = value;

                    OnPropertyChanged(nameof(PackId));

                }
            }
        }
        private string packName { get; set; }
        public string PackName
        {
            get { return packName; }
            set
            {
                if (packName != value)
                {
                    packName = value;

                    OnPropertyChanged(nameof(PackName));

                }
            }
        }
        private string typeName { get; set; }
        public string TypeName
        {
            get { return typeName; }
            set
            {
                if (typeName != value)
                {
                    typeName = value;

                    OnPropertyChanged(nameof(TypeName));

                }
            }
        }
        private string provider { get; set; }
        public string Provider
        {
            get { return provider; }
            set
            {
                if (provider != value)
                {
                    provider = value;

                    OnPropertyChanged(nameof(Provider));

                }
            }
        }
        private string annualPay { get; set; }
        public string AnnualPay
        {
            get { return FormatAnnualPay(annualPay); }
            set
            {
                if (annualPay != value)
                {
                    annualPay = value;
                    OnPropertyChanged(nameof(AnnualPay));
                }
            }
        }

        private string FormatAnnualPay(string value)
        {
            if (string.IsNullOrEmpty(value))
                return "";
            if (!decimal.TryParse(value, out decimal annualPayDecimal))
                return "";

            string formattedAnnualPay = string.Format("{0:N0}", annualPayDecimal).Replace(",", ".");

            return formattedAnnualPay;
        }


        private string resDate { get; set; }
        public string ResDate
        {
            get { return resDate; }
            set
            {
                if (resDate != value)
                {
                    resDate = value;

                    OnPropertyChanged(nameof(ResDate));

                }
            }
        }
        private string expDate { get; set; }
        public string ExpDate
        {
            get { return expDate; }
            set
            {
                if (expDate != value)
                {
                    expDate = value;

                    OnPropertyChanged(nameof(ExpDate));

                }
            }
        }
        private string status { get; set; }
        public string Status
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
        private string benefibeneficiaryName { get; set; }
        public string BeneficiaryName
        {
            get { return benefibeneficiaryName; }
            set
            {
                if (benefibeneficiaryName != value)
                {
                    benefibeneficiaryName = value;

                    OnPropertyChanged(nameof(BeneficiaryName));

                }
            }
        }
        private string termName { get; set; }
        public string TermName
        {
            get { return termName; }
            set
            {
                if (termName != value)
                {
                    termName = value;

                    OnPropertyChanged(nameof(TermName));

                }
            }
        }
        private string paymentMethod { get; set; }
        public string PaymentMethod
        {
            get { return paymentMethod; }
            set
            {
                if (paymentMethod != value)
                {
                    paymentMethod = value;

                    OnPropertyChanged(nameof(PaymentMethod));

                }
            }
        }
        private Visibility _activeVisibility = Visibility.Collapsed;
        public Visibility ActiveVisibility
        {
            get { return _activeVisibility; }
            set
            {
                if (_activeVisibility != value)
                {
                    _activeVisibility = value;
                    OnPropertyChanged(nameof(ActiveVisibility));
                }
            }
        }
        private Visibility _expireVisibility = Visibility.Collapsed;
        public Visibility ExprieVisibility
        {
            get { return _expireVisibility; }
            set
            {
                if (_expireVisibility != value)
                {
                    _expireVisibility = value;
                    OnPropertyChanged(nameof(ExprieVisibility));
                }
            }
        }
        public InsuranceHistoryVM(int id)
        {
            _FetchHisInsurancePackInfo(id);
            BackCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                MainWindowVM.Instance.BackScreen();
            });
        }
        public void _FetchHisInsurancePackInfo(int id)
        {
            var a = APIHelper.Instance.FetchHisInsurancePackInfo(id);
            if (a == null) { MessageBox.Show("Value can not be null"); return; }
            UpdateInsurancePackInfo(a);
        }
        public void UpdateInsurancePackInfo(List<InsuranceHistoryModel> InfoPack)
        {
            FullName = InfoPack[0].fullName;
            IdenNumber = InfoPack[0].idenNumber;
            PhoneNum = InfoPack[0].phone;
            Transdate = InfoPack[0].transactionDate?.Substring(0, Math.Min(InfoPack[0].transactionDate.Length, 10));
            TransId = InfoPack[0].id;
            MemId = InfoPack[0].memberId;
            ContractId = InfoPack[0].contractId;
            PackId = InfoPack[0].packageId;
            PackName = InfoPack[0].packageName;
            TypeName = InfoPack[0].typeName;
            Provider = InfoPack[0].provider;
            AnnualPay = InfoPack[0].annualPay;
            ResDate = InfoPack[0].registrationDate?.Substring(0, Math.Min(InfoPack[0].registrationDate.Length, 10));
            ExpDate = InfoPack[0].expireDate?.Substring(0, Math.Min(InfoPack[0].expireDate.Length, 10));
            Status = InfoPack[0].status;
            BeneficiaryName = InfoPack[0].beneficiaryName;
            TermName = InfoPack[0].termName;
            PaymentMethod = InfoPack[0].paymentMethod;
            if (Status == "1")
            {
                ActiveVisibility = Visibility.Visible;
                ExprieVisibility = Visibility.Collapsed;
            }
            else if (Status == "0")
            {
                ActiveVisibility = Visibility.Collapsed;
                ExprieVisibility = Visibility.Visible;
            }
        }
    }
}
