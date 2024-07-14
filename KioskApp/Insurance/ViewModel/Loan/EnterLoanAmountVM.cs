using Insurance.Command;
using Insurance.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Globalization;
using System.Windows.Threading;
using System.Windows;

namespace Insurance.ViewModel
{
    public class EnterLoanAmountVM : ViewModelBase
    {

        private Visibility _visibilityLoan = Visibility.Collapsed;
        public Visibility VisibilityLoan
        {
            get { return _visibilityLoan; }
            set
            {
                _visibilityLoan = value;
                OnPropertyChanged(nameof(VisibilityLoan));
            }
        }

        private Visibility _visibilitySaving = Visibility.Collapsed;
        public Visibility VisibilitySaving
        {
            get { return _visibilitySaving; }
            set
            {
                _visibilitySaving = value;
                OnPropertyChanged(nameof(VisibilitySaving));
            }
        }



        public EnterLoanAmountVM()
        {
            SetVisibility();

            EnterCommand = new RelayCommand<object>((p) => { if (string.IsNullOrEmpty(DisplayText1)) return false; else return true; }, (p) =>
            {
                if(Convert.ToInt32(DisplayText1) % 50000 == 0) // && _value <= Loan
                {
                    UserModel.Instance.SavingLoanAmount = Convert.ToInt32(DisplayText1);
                    MainWindowVM.Instance.CurrentView = new LoanTermVM();
                }    
            });


            BackCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                MainWindowVM.Instance.BackScreen();
                MainWindowVM.Instance.setDefaultLayout();
            });

            CancelCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                MainWindowVM.Instance.CurrentView = HomeVM.Instance;
                MainWindowVM.Instance.setDefaultLayout();
            });

            MainWindowVM.Instance.setBlankLayout();

            NumberCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                AddToDisplay(p.ToString());
            });

            DeleteCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                DeleteFromDisplay();
            });

            ClearCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                ClearFromDisplay();
            });
        }
        public ICommand EnterCommand { get; set; }
        public ICommand BackCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        public ICommand NumberCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand ClearCommand { get; set; }


        private string _displayText1;
        public string DisplayText1
        {
            get { return _displayText1; }
            set
            {
                if (_displayText1 != value)
                {
                    _displayText1 = value;
                    OnPropertyChanged(nameof(DisplayText1));
                }
            }
        }


        string _value;
        private void AddToDisplay(string value)
        {
            DisplayText1 += value;

            //if (long.TryParse(DisplayText1, out long number))
            //{
            //    var _Amount = string.Format(CultureInfo.GetCultureInfo("vi-VN"), "{0:#,0.##}", number);
            //    DisplayText1 = _Amount;
            //}
        }

        private void DeleteFromDisplay()
        {
            if (!DisplayText1.Equals(string.Empty))
                DisplayText1 = DisplayText1.Substring(0, DisplayText1.Length - 1);
        }

        private void ClearFromDisplay()
        {
            DisplayText1 = null;
        }

        private void SetVisibility()
        {
            if (HomeVM.Instance.ButtonCommandID == 1)
            {
                VisibilityLoan = Visibility.Visible;
            }
            else if (HomeVM.Instance.ButtonCommandID == 4)
            {
                VisibilitySaving = Visibility.Visible;
            }
        }

        private void HandleEnterAmount() 
        { 
            
        }

    }
}
