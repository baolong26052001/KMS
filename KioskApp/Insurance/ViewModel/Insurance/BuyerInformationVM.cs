using Insurance.Command;
using Insurance.Model;
using Insurance.Utility;
using Insurance.View;
using Insurance.VirtualKeyboard;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml.Linq;

namespace Insurance.ViewModel
{
    class BuyerInformationVM : ViewModelBase
    {
        public ICommand NextCommand { get; set; }
        public ICommand TextboxCommand { get; set; }
        public ICommand ButtonPopupCommand { get; set; }

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

        private string address { get; set; }
        public string Address
        {
            get { return address; }
            set
            {
                if (address != value)
                {
                    address = value;

                    OnPropertyChanged(nameof(Address));
                }
            }
        }

        private string occupation { get; set; }
        public string Occupation
        {
            get { return occupation; }
            set
            {
                if (occupation != value)
                {
                    occupation = value;

                    OnPropertyChanged(nameof(Occupation));

                }
            }
        }
        private string email { get; set; }
        public string Email
        {
            get { return email; }
            set
            {
                if (email != value)
                {
                    email = value;

                    OnPropertyChanged(nameof(Email));

                }
            }
        }
        private string idNum { get; set; }
        public string IDNum
        {
            get { return idNum; }
            set
            {
                if (idNum != value)
                {
                    idNum = value;

                    OnPropertyChanged(nameof(IDNum));

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
        private string gender { get; set; }
        public string Gender
        {
            get { return gender; }
            set
            {
                if (gender != value)
                {
                    gender = value;

                    OnPropertyChanged(nameof(Gender));

                }
            }
        }

        private string dob { get; set; }
        public string DOB
        {
            get { return dob; }
            set
            {
                if (dob != value)
                {
                    dob = value;

                    OnPropertyChanged(nameof(DOB));

                }
            }
        }

        private string tax { get; set; }
        public string Tax
        {
            get { return tax; }
            set
            {
                if (tax != value)
                {
                    tax = value;

                    OnPropertyChanged(nameof(Tax));

                }
            }
        }

        private bool _isEnabled;

        public bool IsEnabled
        {
            get { return _isEnabled; }
            set
            {
                if (_isEnabled != value)
                {
                    _isEnabled = value;
                    OnPropertyChanged(nameof(IsEnabled));
                }
            }
        }


        public void _FetchUserInfo(int id)
        {
            var a = APIHelper.Instance.FetchUserInfo(id);
            if (a == null) { MessageBox.Show("Value can not be null"); return; }
            LoadUserInfo(a);
        }
        public void LoadUserInfo(List<UserInfoModel> userInfo)
        {
            FullName = UserModel.Instance.FullName;
            Address = userInfo[0].address1;
            Occupation = userInfo[0].occupation;
            Email = userInfo[0].email;
            IDNum = userInfo[0].idenNumber;
            PhoneNum = userInfo[0].phone;
            Gender = userInfo[0].gender;
            DOB = userInfo[0].birthday.ToShortDateString();
            Tax = userInfo[0].taxCode;
            //UserModel.Instance.FullName = FullName;

            UserModel.Instance.Gender = Gender;
            UserModel.Instance.IDCard = IDNum;
            UserModel.Instance.Birthday = DOB;
            UserModel.Instance.Email = Email;
        }

        private Visibility _visibilityTextbox = Visibility.Visible;
        public Visibility VisibilityTextbox
        {
            get { return _visibilityTextbox; }
            set
            {
                _visibilityTextbox = value;
                OnPropertyChanged(nameof(VisibilityTextbox));
            }
        }

        private bool _emptyPopupVisible;
        public bool _EmptyPopupVisible
        {
            get { return _emptyPopupVisible; }
            set
            {
                if (_emptyPopupVisible != value)
                {
                    _emptyPopupVisible = value;
                    OnPropertyChanged(nameof(_EmptyPopupVisible));
                }
            }
        }

        public int d;

        public BuyerInformationVM()
        {
            MainWindowVM.Instance.setVisibilityHeader();

            _FetchUserInfo(UserModel.Instance.UserID);
           
            //Disable DatePicker
            IsEnabled = false;

            //Keyboard
            TextboxCommand = new RelayCommand<object>((p) => { return !_EmptyPopupVisible; }, (p) =>
            {
                TextBox tb = (TextBox)p;
                d = Convert.ToInt32(tb.ToolTip);
                PopupKeyboard();

                VirtualKeyboardVM vm = (VirtualKeyboardVM)virtualkb.DataContext;
                vm.PropertyChanged += VirtualKeyboardVM_PropertyChanged;
                vm.TextblockContent_BuyerInfo();
            });

            NextCommand = new RelayCommand((parameter) =>
            {
                if(TextboxesFilled())
                {
                    UpdateUserInfo();

                    int a = APIHelper.Instance.UpdateUserInformation(UserModel.Instance.UserID);
                    if (a != 200) MessageBox.Show("Not Updated");

                    MainWindowVM.Instance.CurrentView = new ContractInfoVM();
                }
                else
                {
                    _EmptyPopupVisible = true; // de k kich hoat textbox
                    _PopupWindow();
                }
            });


            BackCommand = new RelayCommand((parameter) =>
            {
                MainWindowVM.Instance.BackScreen();
            });


            ButtonPopupCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                _EmptyPopupVisible = false;
                VisibilityTextbox = Visibility.Visible;
            });
        }



        private static VirtualKeyboardView virtualkb;

        public async void PopupKeyboard()
        {
            if (virtualkb == null || !virtualkb.IsVisible)
            {

                virtualkb = new VirtualKeyboardView();

                // Set the WindowStartupLocation to Manual
                virtualkb.WindowStartupLocation = WindowStartupLocation.Manual;

                // Position middle
                virtualkb.Left = (SystemParameters.PrimaryScreenWidth - virtualkb.Width) / 2;
                virtualkb.Top = (SystemParameters.PrimaryScreenHeight - virtualkb.Height) / 2;


                VirtualKeyboardVM vm = (VirtualKeyboardVM)virtualkb.DataContext;
                if (d == 5) vm.DisplayText = Tax;
              
                //virtualkb.Closed += VirtualKeyboardClosed;
                //virtualkb.Show();
                Task.Delay(500).ContinueWith(_ =>
                {
                    virtualkb.Show();
                }, TaskScheduler.FromCurrentSynchronizationContext());
            }
            else
            {
                virtualkb.Focus();
            }
        }


        private void VirtualKeyboardClosed(object sender, EventArgs e)
        {
            VisibilityTextbox = Visibility.Visible;
        }
        public void CloseKeyboard()
        {
            if (virtualkb == null) return;

            VirtualKeyboardVM vm = (VirtualKeyboardVM)virtualkb.DataContext;

            vm.Buyer_ValidateInformation();

            if (vm.ValidationSucceeded == true)
            {
                //virtualkb.Closed -= VirtualKeyboardClosed;
                virtualkb.Close();
                virtualkb = null;
            }

            else
            {
                return;
            }
        }


        private void VirtualKeyboardVM_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // Update the corresponding TextBox when DisplayText changes
            if (sender is VirtualKeyboardVM vm)
            {
                //if (d == 3) Occupation = vm.DisplayText;
                if (d == 5) Tax = vm.DisplayText;
                //else if (d == 6) Email = vm.DisplayText;
                //if (d == 7) PhoneNum = vm.DisplayText;
            }
        }

        private void UpdateUserInfo()
        {
            UserModel.Instance.Occupation = Occupation;
            UserModel.Instance.TaxCode = Tax;
        }

        public void API_Beneficiary()
        {
            int result = APIHelper.Instance.InsuranceBeneficiary();
            if (result != 200)
            {
                return;
            }
        }

        public bool TextboxesFilled()
        {
            return !string.IsNullOrEmpty(Occupation);
                //&& !string.IsNullOrEmpty(Tax);
        }

        //--------------------POPUP WINDOW--------------------------

        private static PopUpWindow popUpWin;


        //public async void _PopupWindow()
        //{
        //    if (popUpWin == null || !popUpWin.IsVisible)
        //    {
        //        UserModel.Instance.PopUpTextbox = "Vui lòng điền đầy đủ thông tin";

        //        popUpWin = new PopUpWindow();

        //        // Set the WindowStartupLocation to Manual
        //        popUpWin.WindowStartupLocation = WindowStartupLocation.Manual;

        //        // Position middle
        //        popUpWin.Left = (SystemParameters.PrimaryScreenWidth - popUpWin.Width) / 2;
        //        popUpWin.Top = (SystemParameters.PrimaryScreenHeight - popUpWin.Height) / 2;

        //        // Set owner window
        //        if (Application.Current.MainWindow != null)
        //        {
        //            popUpWin.Owner = Application.Current.MainWindow;
        //        }

        //        popUpWin.Show();
        //    }
        //    else
        //    {
        //        popUpWin.Focus();
        //    }
        //}
        public void _PopupWindow()
        {
            if (popUpWin == null || !popUpWin.IsVisible)
            {
                // Disable interaction with the main window
                Application.Current.MainWindow.IsEnabled = false;

                UserModel.Instance.PopUpTextbox = "Vui lòng điền đầy đủ thông tin";

                popUpWin = new PopUpWindow();

                // Set the WindowStartupLocation to Manual
                popUpWin.WindowStartupLocation = WindowStartupLocation.Manual;

                // Position middle
                popUpWin.Left = (SystemParameters.PrimaryScreenWidth - popUpWin.Width) / 2;
                popUpWin.Top = (SystemParameters.PrimaryScreenHeight - popUpWin.Height) / 2;

                // Set owner window
                if (Application.Current.MainWindow != null)
                {
                    popUpWin.Owner = Application.Current.MainWindow;
                }

                popUpWin.Closed += (sender, e) =>
                {
                    // Enable interaction with the main window when popup window is closed
                    Application.Current.MainWindow.IsEnabled = true;
                };

                popUpWin.Show();
            }
            else
            {
                popUpWin.Focus();
            }
        }

        public void ClosePopup()
        {
            if (popUpWin == null) return;
            popUpWin.Close();
        }
    }

}
