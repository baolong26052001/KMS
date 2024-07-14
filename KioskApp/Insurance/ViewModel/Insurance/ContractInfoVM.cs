using Insurance.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Insurance.ViewModel
{
    class ContractInfoVM : ViewModelBase
    {
        public ICommand AgreeCommand { get; set; }
        public ICommand BackCommand { get; set; }
        public ICommand ButtonPopupCommand {  get; set; }
        public ICommand CheckBoxCommand { get; set; }

        private Visibility _visibilityLoanPolicy = Visibility.Collapsed;
        public Visibility VisibilityLoanPolicy
        {
            get { return _visibilityLoanPolicy; }
            set
            {
                _visibilityLoanPolicy = value;
                OnPropertyChanged(nameof(VisibilityLoanPolicy));
            }
        }

        private Visibility _visibilitySavingPolicy = Visibility.Collapsed;
        public Visibility VisibilitySavingPolicy
        {
            get { return _visibilitySavingPolicy; }
            set
            {
                _visibilitySavingPolicy = value;
                OnPropertyChanged(nameof(VisibilitySavingPolicy));
            }
        }

        private Visibility _visibilityInsurancePolicy = Visibility.Collapsed;
        public Visibility VisibilityInsurancePolicy
        {
            get { return _visibilityInsurancePolicy; }
            set
            {
                _visibilityInsurancePolicy = value;
                OnPropertyChanged(nameof(VisibilityInsurancePolicy));
            }
        }
       

        private Visibility _nextVisibility = Visibility.Collapsed;
        public Visibility NextVisibility
        {
            get { return _nextVisibility; }
            set
            {
                _nextVisibility = value;
                OnPropertyChanged(nameof(NextVisibility));
            }
        }

        private bool _isChecked;

        public bool IsChecked
        {
            get { return _isChecked; }
            set
            {
                if (_isChecked != value)
                {
                    _isChecked = value;
                    OnPropertyChanged(nameof(IsChecked));
                }
            }
        }

        private bool _isCheckedPopupVisible;
        public bool _IsCheckedPopupVisible
        {
            get { return _isCheckedPopupVisible; }
            set
            {
                if (_isCheckedPopupVisible != value)
                {
                    _isCheckedPopupVisible = value;
                    OnPropertyChanged(nameof(_IsCheckedPopupVisible));
                }
            }
        }


        public ContractInfoVM() 
        {
            SetVisibilityPolicy();

            AgreeCommand = new RelayCommand<object>((p) => { return true; },(p) =>
            {
                if (IsChecked)
                {
                    _IsCheckedPopupVisible = false;
                    //NextVisibility = Visibility.Visible;

                    //Loan
                    if (HomeVM.Instance.ButtonCommandID == 1)
                    {
                        MainWindowVM.Instance.CurrentView = new OTPVM();
                    }
                    //Saving
                    else if (HomeVM.Instance.ButtonCommandID == 4)
                    {
                        MainWindowVM.Instance.CurrentView = new PaymentMethodVM();
                    }
                    //Insurance
                    else if (HomeVM.Instance.ButtonCommandID == 6)
                    {
                        MainWindowVM.Instance.CurrentView = new DetailedPackageVM();
                    }
                }
                else 
                {
                    _IsCheckedPopupVisible = true;
                    //NextVisibility = Visibility.Collapsed;
                };
            });

            BackCommand = new RelayCommand<object>((p)=> { return true; },(p) =>
            {
                MainWindowVM.Instance.BackScreen();
            });

            ButtonPopupCommand = new RelayCommand<object>((p) => { return true; }, (p) => 
            {
                _IsCheckedPopupVisible = false;
            });

            CheckBoxCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                //if (IsChecked) { NextVisibility = Visibility.Visible; }
                //NextVisibility = Visibility.Collapsed;

                if (IsChecked)
                {
                    NextVisibility = Visibility.Visible;
                }
                else { NextVisibility = Visibility.Collapsed; }
            });
        }

        private void SetVisibilityPolicy()
        {
            
            if(HomeVM.Instance.ButtonCommandID == 1)
            {
                VisibilityLoanPolicy = Visibility.Visible;
            }
            else if (HomeVM.Instance.ButtonCommandID == 4)
            {
                VisibilitySavingPolicy = Visibility.Visible;
            }
            else if(HomeVM.Instance.ButtonCommandID == 6) 
            {
                VisibilityInsurancePolicy = Visibility.Visible;
            }
        }


    }
}