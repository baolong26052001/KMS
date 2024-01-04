using Insurance.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Printing;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace Insurance.ViewModel
{
    class OTPVM:ViewModelBase
    {
        public ICommand EnterCommand { get; set; }
        public ICommand BackCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        public ICommand NumberCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand ClearCommand { get; set; }
        public ICommand ResendOTPCommand { get; set; }

        private DispatcherTimer timer;
        private int timeRemaining;

        public OTPVM()
        {
            EnterCommand = new RelayCommand((parameter) =>
            {
                MainWindowVM.Instance.CurrentView = new BeneficiaryAddVM();
                MainWindowVM.Instance.setDefaultLayout();
                MainWindowVM.Instance.setVisibilityHeader();
            }, CanEnterCommandExecute);


            BackCommand = new RelayCommand((parameter) =>
            {
                MainWindowVM.Instance.BackScreen();
                MainWindowVM.Instance.setDefaultLayout();
            });

            CancelCommand = new RelayCommand((parameter) =>
            {
                MainWindowVM.Instance.CurrentView = new InsuranceHomeVM();
                MainWindowVM.Instance.setDefaultLayout();
            });

            MainWindowVM.Instance.setBlankLayout();

            NumberCommand = new RelayCommand((parameter) =>
            {
                AddToDisplay(parameter.ToString());
            });

            DeleteCommand = new RelayCommand((parameter) =>
            {
                DeleteFromDisplay();
            });

            ClearCommand = new RelayCommand((parameter) =>
            {
                ClearFromDisplay();
            });

            ResendOTPCommand = new RelayCommand((parameter) =>
            {
                //Logic OTP later

                //Run timer
                InitializeTimer();
            },CanResendOTPCommandExecute);
        
            //Run timer
         InitializeTimer();

        }



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

        private void InitializeTimer()
        {
            // Initialize timer with a tick interval of 1 second
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;

            // Set the initial time remaining
            TimeRemaining = 10; // or any initial value you prefer
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            // Update the time remaining and stop the timer when it reaches 0
            TimeRemaining--;
            if (TimeRemaining == 0)
            {
                timer.Stop();
                // Add Logic later: Enable resend OTP
                //MessageBox.Show("OTP Expired");
            }
        }




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
        private string _displayText2;
        public string DisplayText2
        {
            get { return _displayText2; }
            set
            {
                if (_displayText2 != value)
                {
                    _displayText2 = value;
                    OnPropertyChanged(nameof(DisplayText2));
                }
            }
        }
        private string _displayText3;
        public string DisplayText3
        {
            get { return _displayText3; }
            set
            {
                if (_displayText3 != value)
                {
                    _displayText3 = value;
                    OnPropertyChanged(nameof(DisplayText3));
                }
            }
        }
        private string _displayText4;
        public string DisplayText4
        {
            get { return _displayText4; }
            set
            {
                if (_displayText4 != value)
                {
                    _displayText4 = value;
                    OnPropertyChanged(nameof(DisplayText4));
                }
            }
        }

        private void AddToDisplay(string value)
        {
            if (DisplayText1 == null)
            {
                DisplayText1 += value;
            }
            else if (DisplayText1 != null && DisplayText2 == null)
            {
                DisplayText2 += value;
            }
            else if (DisplayText1 != null && DisplayText2 != null && DisplayText3 == null)
            {
                DisplayText3 += value;
            }
            else if (DisplayText1 != null && DisplayText2 != null && DisplayText3 != null && DisplayText4 == null)
            {
                DisplayText4 += value;
            }
        }

        private void DeleteFromDisplay()
        {
            if (!string.IsNullOrEmpty(DisplayText4))
            {
                DisplayText4 = null;
            }
            else if (!string.IsNullOrEmpty(DisplayText3))
            {
                DisplayText3 = null;
            }
            else if (!string.IsNullOrEmpty(DisplayText2))
            {
                DisplayText2 = null;
            }
            else if (!string.IsNullOrEmpty(DisplayText1))
            {
                DisplayText1 = null;
            }
        }

        private void ClearFromDisplay()
        {
            DisplayText1 = null;
            DisplayText2 = null;
            DisplayText3 = null;
            DisplayText4 = null;
        }

        private bool CanEnterCommandExecute(object parameter)
        {
            // Return false if the DisplayTextbox4 is Empty
            return !string.IsNullOrEmpty(DisplayText4);
        }

        private bool CanResendOTPCommandExecute(object parameter)
        {
            return TimeRemaining == 0;
        }


    }
}
