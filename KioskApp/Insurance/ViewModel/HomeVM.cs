using Insurance.Command;
using Insurance.Model;
using Insurance.Utility;
using Insurance.ViewModel;
using Emgu.CV;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Media3D;
using System.Windows.Threading;
using System.Threading;
using System.Threading.Tasks;

namespace Insurance.ViewModel
{
    public class HomeVM : ViewModelBase
    {
        private static HomeVM _instance;

        public static HomeVM Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new HomeVM();
                }
                return _instance;
            }
        }

        public ICommand InsuranceCommand { get; set; }
        public ICommand LoanCommand { get; set; }
        public ICommand LoanPayBackCommand { get; set; }

        public ICommand SavingCommand { get; set; }
        public ICommand HistoryCommand { get; set; }
        public ICommand WithdrawCommand { get; set; }
        public ICommand ButtonPopupCommand { get; set; }
        public ICommand EditInforCommand { get; set; }

        private string _firstName;

        public string FirstName
        {
            get { return _firstName; }
            set
            {
                if (_firstName != value)
                {
                    _firstName = value;
                    OnPropertyChanged(nameof(FirstName));
                }
            }
        }

        private string _lastName;

        public string LastName
        {
            get { return _lastName; }
            set
            {
                if (_lastName != value)
                {
                    _lastName = value;
                    OnPropertyChanged(nameof(LastName));
                }
            }
        }


        private string _greeting;
        public string Greeting
        {
            get => _greeting;
            set
            {
                _greeting = value;
                OnPropertyChanged(nameof(Greeting));
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
        public int AgeRangeID;

        public int ButtonCommandID;

        protected HomeVM()
        {

            MainWindowVM.Instance.setDefaultLayout();

            //UserModel.Instance.UserID = 37;// For testing

            _FetchUserInfo(UserModel.Instance.UserID);

            UpdateGreeting();


            InsuranceCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                //HomeVM vm = (HomeVM)MainWindowVM.Instance.CurrentView;
                //vm.timerStop();
                ButtonCommandID = Convert.ToInt32(p);
                UserModel.Instance.PrintCheck = 0;
                MainWindowVM.Instance.CurrentView = new InsuranceProviderVM();
                APIHelper.Instance.Log("Info: User select buy Insurance");
            });

            LoanCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                //HomeVM vm = (HomeVM)MainWindowVM.Instance.CurrentView;
                //vm.timerStop();

                ButtonCommandID = Convert.ToInt32(p);
                MainWindowVM.Instance.CurrentView = new SelectLoanVM();
                APIHelper.Instance.Log("Info: User select Loan");
            });

            LoanPayBackCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                ButtonCommandID = Convert.ToInt32(p);
                MainWindowVM.Instance.CurrentView = new PayHomeVM();
            });

            SavingCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                ButtonCommandID = Convert.ToInt32(p);
                MainWindowVM.Instance.CurrentView = new SelectLoanVM();
                APIHelper.Instance.Log("Info: User select Saving");
            });
            WithdrawCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                ButtonCommandID = Convert.ToInt32(p);
                MainWindowVM.Instance.CurrentView = new ListWithdrawVM();

            });

            HistoryCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                //HomeVM vm = (HomeVM)MainWindowVM.Instance.CurrentView;
                //vm.timerStop();

                //ButtonCommandID = Convert.ToInt32(p);
                MainWindowVM.Instance.CurrentView = new InsuranceTransactionVM();
                APIHelper.Instance.Log("Info: User check history transaction");
            });
            UserModel.Instance.IsWelcomeView = false;
            EditInforCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                ButtonCommandID = Convert.ToInt32(p);
                MainWindowVM.Instance.CurrentView = new PersonalInfoVM();
                UserModel.Instance.IsEditInfo = true;
            });
        }

        private void UpdateGreeting()
        {
            var hour = DateTime.Now.Hour;
            if (hour < 12)
                Greeting = "Chào buổi sáng";
            else if (hour < 18)
                Greeting = "Chào buổi chiều";
            else if (hour < 22)
                Greeting = "Chào buổi tối";
            else
                Greeting = "Good night";
        }


        public void _FetchUserInfo(int id)
        {
            var a = APIHelper.Instance.FetchUser(id);
            if (a == null)
            { //MessageBox.Show("Value can not be null"); return;
                //MainWindowVM.Instance.CurrentView = new VerifyIDVM();
            }
            else
            {
                UpdateUserInfo(a);
            }
        }

        public void UpdateUserInfo(List<UserInfoModel> userInfo)
        {
            FullName = userInfo[0].fullName;
            AgeRangeID = userInfo[0].ageRangeId;


            UserModel.Instance.FullName = userInfo[0].fullName;
            UserModel.Instance.AgeRangeID = AgeRangeID;
        }

        private VideoCapture _capture;
        private DispatcherTimer timer;

        private int _timeRemaining;
        public int TimeRemaining
        {
            get { return _timeRemaining; }
            set
            {
                if (_timeRemaining != value)
                {
                    _timeRemaining = value;
                    OnPropertyChanged(nameof(TimeRemaining));
                }
            }
        }

        private bool _isAutoLogoutPopupVisible;
        public bool IsAutoLogoutPopupVisible
        {
            get { return _isAutoLogoutPopupVisible; }
            set
            {
                if (_isAutoLogoutPopupVisible != value)
                {
                    _isAutoLogoutPopupVisible = value;
                    OnPropertyChanged(nameof(IsAutoLogoutPopupVisible));
                }
            }
        }


        public void CleardataHome()
        {
            _instance = null;
        }

    }
}