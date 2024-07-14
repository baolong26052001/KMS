using Insurance.Command;
using Insurance.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Printing;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace Insurance.ViewModel
{
    class OTPVM : ViewModelBase
    {
        public ICommand EnterCommand { get; set; }
        public ICommand BackCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        public ICommand NumberCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand ClearCommand { get; set; }
        public ICommand ResendOTPCommand { get; set; }

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

        private int otpStatus;
        public int OtpStatus
        {
            get { return otpStatus; }
            set
            {
                if (otpStatus != value)
                {
                    otpStatus = value;
                    OnPropertyChanged(nameof(OtpStatus));
                }
            }
        }

        private int buttonStatus;
        public int ButtonStatus
        {
            get { return buttonStatus; }
            set
            {
                if (buttonStatus != value)
                {
                    buttonStatus = value;
                    OnPropertyChanged(nameof(ButtonStatus));
                }
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

        public OTPVM()
        {
            SendOTP_Email();
            ButtonStatus = 1;
            //OtpStatus = 0;

            EnterCommand = new RelayCommand<object>((p) => { if (string.IsNullOrEmpty(DisplayText4)) return false; else return true; }, (p) =>
            {
                CompareOTP();
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

            ResendOTPCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                if (ButtonStatus == 1 || TimeRemaining == 0)
                {
                    timer.Stop();
                    InitializeTimer();
                    SendOTP_Email();
                    ButtonStatus = -1;
                    OtpStatus = 0;
                }
            });

            //Run timer
            InitializeTimer();

        }



        private void InitializeTimer()
        {
            // Initialize timer with a tick interval of 1 second
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;

            // Set the initial time remaining
            TimeRemaining = 59; // or any initial value you prefer
            timer.Start();
        }

        private int elapsedSeconds = 0;

        private void Timer_Tick(object sender, EventArgs e)
        {
            // Update the time remaining and stop the timer when it reaches 0
            TimeRemaining--;
            elapsedSeconds++;
            if (TimeRemaining == 0)
            {
                timer.Stop();
                ButtonStatus = -1;
            }

            // If 5 seconds have passed, set OTPStatus to 0
            if (elapsedSeconds >= 5)
            {
                OtpStatus = 0;
                elapsedSeconds = 0;
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

        public static String to;
        public string randomCode;

        private async Task SendOTP_Email()
        {
            await Task.Run(() =>
            {
                String from, pass, messageBody;
                Random rand = new Random();
                int randomNumber = rand.Next(10000);
                randomCode = randomNumber.ToString("D4");
                MailMessage message = new MailMessage();
                to = UserModel.Instance.Email;
                from = "davidle2804@gmail.com";
                pass = "hvsj ywws lpwd acmz";
                messageBody = "Your OTP is: " + randomCode;
                message.To.Add(to);
                message.From = new MailAddress(from);
                message.Body = messageBody;
                message.Subject = "ALM OTP Code";
                SmtpClient smtp = new SmtpClient("smtp.gmail.com");
                smtp.EnableSsl = true;
                smtp.Port = 587;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Credentials = new NetworkCredential(from, pass);

                try
                {
                    smtp.Send(message);
                    //MessageBox.Show("Check your Email");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            });
        }

        private int CompareOTP()
        {
            //int status;

            string enteredOTP = $"{DisplayText1}{DisplayText2}{DisplayText3}{DisplayText4}";
            if (TimeRemaining > 0)
            {
                if (enteredOTP == randomCode)
                {
                    //MessageBox.Show("OTP is correct");
                    MainWindowVM.Instance.CurrentView = new BeneficiaryAddVM();
                    MainWindowVM.Instance.setDefaultLayout();
                    MainWindowVM.Instance.setVisibilityHeader();
                    MainWindowVM.Instance.VisibilityBtnHead = Visibility.Collapsed;
                    OtpStatus = 1;
                }
                else
                {
                    //MessageBox.Show("OTP is Incorrect");
                    OtpStatus = -1;
                }
            }


            return OtpStatus;
        }


    }
}