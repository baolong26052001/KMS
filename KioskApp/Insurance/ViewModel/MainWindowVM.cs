using Insurance.Command;
using Insurance.Model;
using Insurance.View;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows.Threading;
using Emgu.CV;
using System.Text.RegularExpressions;
using System.Diagnostics;
using Insurance.Service;
using Insurance.Utility;
using IniParser.Model;
using IniParser;

namespace Insurance.ViewModel
{
    class MainWindowVM : ViewModelBase
    {
        public ICommand CancelCommand => new RelayCommand(CancelCommand_Click);
        public ICommand ShowPopupExit => new RelayCommand(ShowPopupExit_Click);
        public ICommand HidePopupExit => new RelayCommand(HidePopupExit_Click);
        public ICommand UserGuideCommand => new RelayCommand(UserGuideCommand_Click);
        private static MainWindowVM _instance = null;

        private string _currentTime;
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

        private Visibility _visibilityHeader = Visibility.Visible;
        public Visibility VisibilityHeader
        {
            get { return _visibilityHeader; }
            set
            {
                _visibilityHeader = value;
                OnPropertyChanged(nameof(VisibilityHeader));
            }
        }
        private Visibility _visibilityBtnHead = Visibility.Visible;
        public Visibility VisibilityBtnHead
        {
            get { return _visibilityBtnHead; }
            set
            {
                _visibilityBtnHead = value;
                OnPropertyChanged(nameof(VisibilityBtnHead));
            }
        }

        private Visibility _visibilityFooter = Visibility.Visible;
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
            get => _rowContentIndex;
            set
            {
                _rowContentIndex = value;
                OnPropertyChanged(nameof(RowContentIndex));
            }
        }

        private int _rowContentSpan = 0;
        public int RowContentSpan
        {
            get => _rowContentSpan;
            set
            {
                _rowContentSpan = value;
                OnPropertyChanged(nameof(RowContentSpan));
            }
        }

        public void setBlankLayout()
        {
            RowContentIndex = 1; RowContentSpan = 3;
            VisibilityHeader = Visibility.Collapsed;
            VisibilityFooter = Visibility.Collapsed;
        }

        public void setVisibilityHeader()
        {
            VisibilityHeader = Visibility.Visible;
        }
        public void setVisibilityBtnHead()
        {
            if (UserModel.Instance.IsLogin == false)
            {
                VisibilityBtnHead = Visibility.Collapsed;
            }
            else
            {
                VisibilityBtnHead = Visibility.Visible;
            }
        }

        private bool _isBacktoWelcomeVisible;
        public bool IsBacktoWelcomeVisible
        {
            get { return _isBacktoWelcomeVisible; }
            set
            {
                if (_isBacktoWelcomeVisible != value)
                {
                    _isBacktoWelcomeVisible = value;
                    OnPropertyChanged(nameof(IsBacktoWelcomeVisible));
                }
            }
        }
        private bool _ispopupCancelVisible;
        public bool IspopupCancelVisible
        {
            get { return _ispopupCancelVisible; }
            set
            {
                if (_ispopupCancelVisible != value)
                {
                    _ispopupCancelVisible = value;
                    OnPropertyChanged(nameof(IspopupCancelVisible));
                }
            }
        }
        public void setDefaultLayout()
        {
            RowContentIndex = 1; RowContentSpan = 0;
            VisibilityHeader = Visibility.Visible;
            VisibilityFooter = Visibility.Visible;
            VisibilityBtnHead = Visibility.Visible;
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

        //update the time in Mainwindown
        public string CurrentTime
        {
            get => _currentTime;
            set
            {
                _currentTime = value;
                OnPropertyChanged(nameof(CurrentTime));
            }
        }

        //update the time in Mainwindown
        private void UpdateTime()
        {
            CurrentTime = DateTime.Now.ToString("ddd HH:mm MM/dd/yyyy");
        }

        //update the time in Mainwindown
        private void StartTimer()
        {
            var timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            timer.Tick += (sender, e) => UpdateTime();
            timer.Start();
        }

        //private void Timer_Tick(object sender, EventArgs e)
        //{
        //    // Update the TextBlock with the current time
        //    TimeTextBlock.Text = DateTime.Now.ToString("HH:mm, MMMM dd yyyy");
        //}

        private VideoCapture _capture;
        public MainWindowVM()
        {
            SettingIni();
            CurrentView = new WelcomeVM();
            //CurrentView = new InsuranceTransactionVM();
            _beneficiaryAddVM = new BeneficiaryAddVM();
            //update the time in Mainwindow
            UpdateTime();
            StartTimer();
            setVisibilityBtnHead();
            //ActiveCountDown();
            
        }

        public void SettingIni()
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string settingsFilePath = Path.Combine(baseDirectory, "Settings.ini");
            var MyIni = new IniFile(settingsFilePath);
            var parser = new FileIniDataParser();
            IniData data = parser.ReadFile(settingsFilePath);
            KioskModel.Instance.CameraName = data["Insurance"]["Camera"];
            KioskModel.Instance.ScannerName = data["Insurance"]["Scanner save path"];
            KioskModel.Instance.CashDepositPort = data["Insurance"]["PORT"];
            KioskModel.Instance.APIPort = data["Insurance"]["API"];
        }

        public void StopCamera()
        {
            if (_capture != null && _capture.IsOpened)
            {
                _capture.Stop();
                _capture.Dispose();
            }
        }

        private void CancelCommand_Click(object parameter)
        {

            if (UserModel.Instance.IsHomePage)
            {
                HomeVM.Instance.CleardataHome();
            }
            APIHelper.Instance.Log($"Info: Return Welcome screen");
            IsAutoLogoutPopupVisible = false;
            MainWindowVM.Instance.StopCamera();
            UserModel.Instance.ClearData();
            //HomeVM.Instance.CleardataHome();
            MainWindowVM.Instance.VisibilityBtnHead = Visibility.Collapsed;
            UserModel.Instance.IsWelcomeView = true;
            DeactiveCountDown();
            MainWindowVM.Instance.CurrentView = new WelcomeVM();
            MainWindowVM.Instance.IspopupCancelVisible = false;
            MainWindowVM.Instance.UserGuideVisible = Visibility.Visible;
        }
        private void HidePopupExit_Click(object parameter)
        {
            MainWindowVM.Instance.IspopupCancelVisible = false;
        }
        private void ShowPopupExit_Click(object parameter)
        {
            MainWindowVM.Instance.IspopupCancelVisible = true;
        }
        private void UserGuideCommand_Click(object parameter)
        {
            UserModel.Instance.IsWelcomeView = false;
            MainWindowVM.Instance.CurrentView = new UserGuideVM();
            MainWindowVM.Instance.UserGuideVisible = Visibility.Collapsed;
            MainWindowVM.Instance.VisibilityBtnHead = Visibility.Visible;
            
        }
            public void ActiveCountDown()
        {
            UserModel.Instance.IdActiveCountDown = true;
            if (!UserModel.Instance.IsWelcomeView)
            {
                InitializeTimer();
                Application.Current.MainWindow.TouchDown += MainWindow_TouchDown;
                Application.Current.MainWindow.MouseMove += MainWindow_MouseMove;
            }
        }
        public void DeactiveCountDown()
        {
            if(timer != null)
            {
                timer.Stop();
            }
            Application.Current.MainWindow.TouchDown -= MainWindow_TouchDown;
            Application.Current.MainWindow.MouseMove -= MainWindow_MouseMove;


        }
        private Visibility _loadingOverlayVisibility_Main = Visibility.Collapsed;

        public Visibility LoadingOverlayVisibility_Main
        {
            get { return _loadingOverlayVisibility_Main; }
            set
            {
                if (_loadingOverlayVisibility_Main != value)
                {
                    _loadingOverlayVisibility_Main = value;
                    OnPropertyChanged(nameof(LoadingOverlayVisibility_Main));
                }
            }
        }
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
        private Visibility _userGuideVisible = Visibility.Visible;
        public Visibility UserGuideVisible
        {
            get { return _userGuideVisible; }
            set
            {
                if (_userGuideVisible != value)
                {
                    _userGuideVisible = value;
                    OnPropertyChanged(nameof(UserGuideVisible));
                }
            }
        }
        public int setTimeRemaining = 60;

        public void InitializeTimer()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;

            TimeRemaining = setTimeRemaining;
            timer.Start();
        }

        public void Timer_Tick(object sender, EventArgs e)
        {
            TimeRemaining--;
            if (TimeRemaining == 30)
            {
                IsAutoLogoutPopupVisible = true;
            }
            else if (TimeRemaining == 0)
            {
                IsAutoLogoutPopupVisible = false;
                MainWindowVM.Instance.StopCamera();
                UserModel.Instance.ClearData();
                HomeVM.Instance.CleardataHome();
                DeactiveCountDown();
                MainWindowVM.Instance.VisibilityBtnHead = Visibility.Collapsed;
                UserModel.Instance.IsWelcomeView = true;
                MainWindowVM.Instance.CurrentView = new WelcomeVM();
                MainWindowVM.Instance.UserGuideVisible = Visibility.Visible;
                MainWindowVM.Instance.IspopupCancelVisible = false;
            }
        }
        public void timerStop()
        {
            if (timer != null) { timer.Stop(); }

        }
        private void MainWindow_TouchDown(object sender, TouchEventArgs e)
        {
            ResetTimer();
        }

        private void MainWindow_MouseMove(object sender, MouseEventArgs e)
        {
            ResetTimer();
        }
        private void ResetTimer()
        {
            TimeRemaining = setTimeRemaining;
            IsAutoLogoutPopupVisible = false;
        }

    }
}