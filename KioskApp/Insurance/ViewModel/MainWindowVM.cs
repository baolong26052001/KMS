using Insurance.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Insurance.ViewModel
{
    class MainWindowVM : ViewModelBase
    {
        public ICommand CancelCommand { get; set; }

        private static MainWindowVM _instance = null;
        public static MainWindowVM Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new MainWindowVM();
                return _instance;
            }
        }

      

        private ViewModelBase _currentView;
        public ViewModelBase CurrentView
        {
            get
            {
                return _currentView;
            }
            set
            {
              
                //PreView = CurrentView;
                _currentView = value;
                OnPropertyChanged(nameof(CurrentView));
                
                // Add the current view to the list when it changes
                ViewList.Add(_currentView);
            }
        }

        private Visibility _visibilityHeader=Visibility.Visible;
        public Visibility VisibilityHeader
        {
            get { return _visibilityHeader; }
            set
            {
                _visibilityHeader = value;
                OnPropertyChanged(nameof(VisibilityHeader));
            }
        }

        private Visibility _visibilityFooter=Visibility.Visible;
        public Visibility VisibilityFooter
        {
            get { return _visibilityFooter; }
            set
            {
                _visibilityFooter = value;
                OnPropertyChanged(nameof(VisibilityFooter));
            }
        }

        private int _rowContentIndex = 1;
        public int RowContentIndex
        {
            get=> _rowContentIndex;
            set
            {
                _rowContentIndex=value;
                OnPropertyChanged(nameof(RowContentIndex));
            }
        }

        private int _rowContentSpan = 0;
        public int RowContentSpan
        {
            get => _rowContentSpan;
            set
            {
                _rowContentSpan=value;
                OnPropertyChanged(nameof(RowContentSpan));
            }
        }

        public void setBlankLayout()
        {
            RowContentIndex=1; RowContentSpan=3;
            VisibilityHeader = Visibility.Collapsed;
            VisibilityFooter = Visibility.Collapsed;
        }

        public void setVisibilityHeader()
        {
            VisibilityHeader = Visibility.Collapsed;
        }

        public void setDefaultLayout()
        {
            RowContentIndex = 1; RowContentSpan = 0;
            VisibilityHeader = Visibility.Visible;
            VisibilityFooter = Visibility.Visible;
        }


        // List to store the views
        public List<ViewModelBase> ViewList { get; set; } = new List<ViewModelBase>();

        BeneficiaryAddVM _beneficiaryAddVM;

        public void BackScreen()
        {
            if (ViewList.Count > 0)
            {
                ViewList.Remove(ViewList.Last());
                CurrentView = ViewList.Last();
                ViewList.Remove(ViewList.Last());
            }
        }



        public void GoBeneficiary()
        {
            MainWindowVM.Instance.CurrentView = _beneficiaryAddVM;
        }

        CashDepositVM _cashDepositVM;
        public void GoCashDeposit()
        {
            MainWindowVM.Instance.CurrentView = _cashDepositVM;
        }

        public MainWindowVM()
        {

            _beneficiaryAddVM = new BeneficiaryAddVM();
            CurrentView = new WelcomeVM();

            CancelCommand = new RelayCommand((parameter) =>
            {
                MainWindowVM.Instance.CurrentView = new WelcomeVM();
            });

        }
    }
}
