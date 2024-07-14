using Insurance.Command;
using Insurance.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Insurance.ViewModel
{
    public class LoanTermVM : ViewModelBase
    {
        public ICommand FirstTermCommand { get; set; }
        public ICommand SecondTermCommand { get; set; }
        public ICommand ThirdTermCommand { get; set; }
        public ICommand FourthTermCommand { get; set; }
        public ICommand ConfirmCommand { get; set; }
        public ICommand BackCommand { get; set; }

        private string member;
        public string Member
        {
            get { return member; }
            set
            {
                if (member != value)
                {
                    member = value;
                    OnPropertyChanged(nameof(Member));
                }
            }
        }

        private string loanAmount;
        public string LoanAmount
        {
            get { return loanAmount; }
            set
            {
                if (loanAmount != value)
                {
                    loanAmount = value;
                    OnPropertyChanged(nameof(LoanAmount));
                }
            }
        }

        private string periodText;
        public string PeriodText
        {
            get { return periodText; }
            set
            {
                if (periodText != value)
                {
                    periodText = value;
                    OnPropertyChanged(nameof(PeriodText));
                }
            }
        }

        private string principalAmount;
        public string PrincipalAmount
        {
            get { return principalAmount; }
            set
            {
                if (principalAmount != value)
                {
                    principalAmount = value;
                    OnPropertyChanged(nameof(PrincipalAmount));
                }
            }
        }

        private string interestRate;
        public string InterestRate
        {
            get { return interestRate; }
            set
            {
                if (interestRate != value)
                {
                    interestRate = value;
                    OnPropertyChanged(nameof(InterestRate));
                }
            }
        }

        private double totalDebtMustPay;
        public double TotalDebtMustPay
        {
            get { return totalDebtMustPay; }
            set
            {
                if (totalDebtMustPay != value)
                {
                    totalDebtMustPay = value;
                    OnPropertyChanged(nameof(TotalDebtMustPay));
                }
            }
        }

        private double totalReceive;
        public double TotalReceive
        {
            get { return totalReceive; }
            set
            {
                if (totalReceive != value)
                {
                    totalReceive = value;
                    OnPropertyChanged(nameof(TotalReceive));
                }
            }
        }

        private string formattedTotalDebt;
        public string FormattedTotalDebt
        {
            get { return formattedTotalDebt; }
            set
            {
                if (formattedTotalDebt != value)
                {
                    formattedTotalDebt = value;
                    OnPropertyChanged(nameof(FormattedTotalDebt));
                }
            }
        }

        private string formattedTotalReceive;
        public string FormattedTotalReceive
        {
            get { return formattedTotalReceive; }
            set
            {
                if (formattedTotalReceive != value)
                {
                    formattedTotalReceive = value;
                    OnPropertyChanged(nameof(FormattedTotalReceive));
                }
            }
        }

        private string regDate;
        public string RegDate
        {
            get { return regDate; }
            set
            {
                if (regDate != value)
                {
                    regDate = value;
                    OnPropertyChanged(nameof(RegDate));
                }
            }
        }

        private string endDate;
        public string EndDate
        {
            get { return endDate; }
            set
            {
                if (endDate != value)
                {
                    endDate = value;
                    OnPropertyChanged(nameof(EndDate));
                }
            }
        }

        private string paymentDate;
        public string PaymentDate
        {
            get { return paymentDate; }
            set
            {
                if (paymentDate != value)
                {
                    paymentDate = value;
                    OnPropertyChanged(nameof(PaymentDate));
                }
            }
        }

        private Visibility loanDetailVisibility = Visibility.Collapsed;
        public Visibility LoanDetailVisibility
        {
            get { return loanDetailVisibility; }
            set
            {
                loanDetailVisibility = value;
                OnPropertyChanged(nameof(LoanDetailVisibility));
            }
        }

        private Visibility savingDetailVisibility = Visibility.Collapsed;
        public Visibility SavingDetailVisibility
        {
            get { return savingDetailVisibility; }
            set
            {
                savingDetailVisibility = value;
                OnPropertyChanged(nameof(SavingDetailVisibility));
            }
        }


        public int parameters;

        public LoanTermVM()
        {
            SetVisibility();
            
            var Amount = string.Format(CultureInfo.GetCultureInfo("vi-VN"), "{0:#,0.##}", UserModel.Instance.SavingLoanAmount);
            LoanAmount = Amount;


            FetchLoanTerm(1);
            FirstTermCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                parameters = Convert.ToInt32(p);
                FetchLoanTerm(parameters);
            });
            SecondTermCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                parameters = Convert.ToInt32(p);
                FetchLoanTerm(parameters);
            });
            ThirdTermCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                parameters = Convert.ToInt32(p);
                FetchLoanTerm(parameters);
            });
            FourthTermCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                parameters = Convert.ToInt32(p);
                FetchLoanTerm(parameters);
            });

            ConfirmCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                MainWindowVM.Instance.CurrentView = new ContractInfoVM();
                SaveSavingDetails();
            });

            BackCommand = new RelayCommand<object>((p) => { return true; }, (p) => 
            {
                MainWindowVM.Instance.BackScreen();
            });

        }
        int Period;
        double _InterestRate;

        public void FetchLoanTerm(int p)
        {
            if (p == 1)
            {
                Member = UserModel.Instance.FullName;
                PeriodText = "1 tháng";
                PrincipalAmount = "";
                InterestRate = "7%";
                TotalDebtMustPay = UserModel.Instance.SavingLoanAmount + UserModel.Instance.SavingLoanAmount * 0.07 * 1;
                TotalReceive = UserModel.Instance.SavingLoanAmount + UserModel.Instance.SavingLoanAmount * 0.05 * 1;
                FormattedTotalDebt = string.Format(CultureInfo.GetCultureInfo("vi-VN"), "{0:#,0.##}", TotalDebtMustPay);
                FormattedTotalReceive = string.Format(CultureInfo.GetCultureInfo("vi-VN"), "{0:#,0.##}", TotalReceive);
                RegDate = DateTime.Now.ToShortDateString();
                EndDate = DateTime.Now.AddMonths(1).ToShortDateString();
                //PaymentDate = DateTime.FromOADate();
            }
            else if (p == 2)
            {
                Member = UserModel.Instance.FullName;
                PeriodText = "2 tháng";
                PrincipalAmount = "";
                InterestRate = "8%";
                TotalDebtMustPay = UserModel.Instance.SavingLoanAmount + UserModel.Instance.SavingLoanAmount * 0.08 * 2;
                TotalReceive = UserModel.Instance.SavingLoanAmount + UserModel.Instance.SavingLoanAmount * 0.05 * 2;
                FormattedTotalDebt = string.Format(CultureInfo.GetCultureInfo("vi-VN"), "{0:#,0.##}", TotalDebtMustPay);
                FormattedTotalReceive = string.Format(CultureInfo.GetCultureInfo("vi-VN"), "{0:#,0.##}", TotalReceive);
                RegDate = DateTime.Now.ToShortDateString();
                EndDate = DateTime.Now.AddMonths(2).ToShortDateString();
            }
            else if (p == 3)
            {
                Member = UserModel.Instance.FullName;
                PeriodText = "3 tháng";
                PrincipalAmount = "";
                InterestRate = "9%";
                TotalDebtMustPay = UserModel.Instance.SavingLoanAmount + UserModel.Instance.SavingLoanAmount * 0.09 * 3;
                TotalReceive = UserModel.Instance.SavingLoanAmount + UserModel.Instance.SavingLoanAmount * 0.05 * 3;
                FormattedTotalDebt = string.Format(CultureInfo.GetCultureInfo("vi-VN"), "{0:#,0.##}", TotalDebtMustPay);
                FormattedTotalReceive = string.Format(CultureInfo.GetCultureInfo("vi-VN"), "{0:#,0.##}", TotalReceive);
                RegDate = DateTime.Now.ToShortDateString();
                EndDate = DateTime.Now.AddMonths(3).ToShortDateString();
            }
            else if (p == 4)
            {
                Member = UserModel.Instance.FullName;
                PeriodText = "6 tháng";
                PrincipalAmount = "";
                InterestRate = "9%";
                TotalDebtMustPay = UserModel.Instance.SavingLoanAmount + UserModel.Instance.SavingLoanAmount * 0.09 * 6;
                TotalReceive = UserModel.Instance.SavingLoanAmount + UserModel.Instance.SavingLoanAmount * 0.05 * 6;
                FormattedTotalDebt = string.Format(CultureInfo.GetCultureInfo("vi-VN"), "{0:#,0.##}", TotalDebtMustPay);
                FormattedTotalReceive = string.Format(CultureInfo.GetCultureInfo("vi-VN"), "{0:#,0.##}", TotalReceive);
                RegDate = DateTime.Now.ToShortDateString();
                EndDate = DateTime.Now.AddMonths(6).ToShortDateString();
            }
        }

        public void SaveSavingDetails()
        {
            SavingTransactionModel saving = new SavingTransactionModel();
            saving.memberId = UserModel.Instance.UserID;
            saving.topUp = UserModel.Instance.SavingLoanAmount;
            saving.savingTerm = Period;
            saving.savingRate = _InterestRate;
            //saving.transactionDate = RegDate;
            //saving.dueDate = EndDate;
            
            UserModel.Instance.SavingTransaction = saving;
        }
        private void SetVisibility()
        {
            if (HomeVM.Instance.ButtonCommandID == 1)
            {
                LoanDetailVisibility = Visibility.Visible;
            }
            else if (HomeVM.Instance.ButtonCommandID == 4)
            {
                SavingDetailVisibility = Visibility.Visible;
            }
        }


    }
}
