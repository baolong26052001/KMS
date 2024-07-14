using Insurance.Command;
using Insurance.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Insurance.ViewModel
{
    public class SelectLoanVM : ViewModelBase
    {

        public ICommand ButtonCommand { get; set; }
        public ICommand OthersCommand { get; set; }
        public ICommand BackCommand { get; set; }

        private Visibility loanVisibility = Visibility.Collapsed;
        public Visibility LoanVisibility
        {
            get { return loanVisibility; }
            set
            {
                loanVisibility = value;
                OnPropertyChanged(nameof(LoanVisibility));
            }
        }

        private Visibility savingVisibility = Visibility.Collapsed;
        public Visibility SavingVisibility
        {
            get { return savingVisibility; }
            set
            {
                savingVisibility = value;
                OnPropertyChanged(nameof(SavingVisibility));
            }
        }


        public int _LoanAmount; 
        public SelectLoanVM()
        {
            SetVisibility();
            ButtonCommand = new RelayCommand<object>((p) => { return true; }, (p =>
            {
                _LoanAmount = Convert.ToInt32(p);
                UserModel.Instance.SavingLoanAmount = _LoanAmount;
                MainWindowVM.Instance.CurrentView = new LoanTermVM();
            }));

            OthersCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                MainWindowVM.Instance.CurrentView = new EnterLoanAmountVM();
            });

            BackCommand = new RelayCommand<object>((p) => { return true; }, (p) => 
            {
                MainWindowVM.Instance.BackScreen();
            });
        }

        private void SetVisibility()
        {

            if (HomeVM.Instance.ButtonCommandID == 1)
            {
                LoanVisibility = Visibility.Visible;
            }
            else if (HomeVM.Instance.ButtonCommandID == 4)
            {
                SavingVisibility = Visibility.Visible;
            }
        }
    }
}
