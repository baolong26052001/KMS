using DirectShowLib;
using Insurance.Command;
using Insurance.Model;
using Insurance.View;
using Insurance.ViewModel;
using RestSharp;
using System;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml.Linq;


namespace Insurance.VirtualKeyboard
{
    class VirtualKeyboardVM : ViewModelBase
    {
        public ICommand NumberCommand { get; set; }
        public ICommand SpaceCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand ShiftCommand { get; set; }
        public ICommand SwitchKeyboardCommand { get; set; }
        public ICommand EnterCommand { get; set; }
        public ICommand ButtonPopupCommand { get; set; }

        private bool _validationSucceeded;
        public bool ValidationSucceeded
        {
            get { return _validationSucceeded; }
            set
            {
                _validationSucceeded = value;
                OnPropertyChanged(nameof(ValidationSucceeded));
            }
        }


        private string _validationText;
        public string ValidationText
        {
            get { return _validationText; }
            set
            {
                _validationText = value;
                OnPropertyChanged(nameof(ValidationText));
            }
        }


        public VirtualKeyboardVM()
        {
            //TextblockContent_BuyerInfo();

            //TextblockContent_BeneInfo();


            NumberCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                AddToDisplay(p.ToString());

            });

            SpaceCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                SpaceText();
            });

            DeleteCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                if (DisplayText != null)
                    DeleteFromDisplay();
            });

            ShiftCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                ToggleShift();
            });

            SwitchKeyboardCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                if (VisibilityAlphabet == Visibility.Visible)
                {
                    VisibilityAlphabet = Visibility.Collapsed;
                    VisibilitySpecial = Visibility.Visible;
                }
                else
                {
                    VisibilitySpecial = Visibility.Collapsed;
                    VisibilityAlphabet = Visibility.Visible;
                }
            });

            EnterCommand = new RelayCommand<object>((p) => { return true; }, (parameter) =>
            {
                HandleEnterCommand();
            });

            ButtonPopupCommand = new RelayCommand<object>((p) => { return true; }, (parameter) =>
            {
                //Visibility
                _ValidatePopupVisible = false;
                VisibilityAlphabet = Visibility.Visible;
                //VisibilitySpecial = Visibility.Visible;
            });

        }


        private string _displayText;
        public string DisplayText
        {
            get { return _displayText; }
            set
            {
                if (_displayText != value)
                {
                    _displayText = value;

                    OnPropertyChanged(nameof(DisplayText));
                }
            }
        }
        private string _displayTextBlock;
        public string DisplayTextBlock
        {
            get { return _displayTextBlock; }
            set
            {
                if (_displayTextBlock != value)
                {
                    _displayTextBlock = value;

                    OnPropertyChanged(nameof(DisplayTextBlock));
                }
            }
        }

        private Visibility _visibilityAlphabet = Visibility.Visible;
        public Visibility VisibilityAlphabet
        {
            get { return _visibilityAlphabet; }
            set
            {
                _visibilityAlphabet = value;
                OnPropertyChanged(nameof(VisibilityAlphabet));
            }
        }

        private Visibility _visibilitySpecial = Visibility.Collapsed;
        public Visibility VisibilitySpecial
        {
            get { return _visibilitySpecial; }
            set
            {
                _visibilitySpecial = value;
                OnPropertyChanged(nameof(VisibilitySpecial));
            }
        }


        private bool _isShifted;
        public bool IsShifted
        {
            get { return _isShifted; }
            set
            {
                if (_isShifted != value)
                {
                    _isShifted = value;
                    OnPropertyChanged(nameof(IsShifted));
                }
            }
        }

        private bool _validatePopupVisible;
        public bool _ValidatePopupVisible
        {
            get { return _validatePopupVisible; }
            set
            {
                if (_validatePopupVisible != value)
                {
                    _validatePopupVisible = value;
                    OnPropertyChanged(nameof(_ValidatePopupVisible));
                }
            }
        }

        private void AddToDisplay(string value)
        {
            value = IsShifted ? value.ToUpper() : value.ToLower();
            DisplayText += value;

            IsShifted = false;
        }

        private void SpaceText()
        {
            DisplayText += " ";
        }


        private void DeleteFromDisplay()
        {
            if (!DisplayText.Equals(string.Empty))
            {
                DisplayText = DisplayText.Substring(0, DisplayText.Length - 1);
            }

            IsShifted = false;
        }

        private void ToggleShift()
        {
            IsShifted = !IsShifted;
        }

        private static VirtualKeyboardView virtualkb;
        public void HandleEnterCommand()
        {
            PersonalInfoVM vm3 = new PersonalInfoVM();
            vm3.CloseKeyboard();
            BuyerInformationVM vm2 = new BuyerInformationVM();
            vm2.CloseKeyboard();

            BeneficiaryInfoVM vm1 = new BeneficiaryInfoVM();
            vm1.CloseKeyboard();
        }

        public void ValidateInformation()
        {
            BeneficiaryInfoVM vm = (BeneficiaryInfoVM)MainWindowVM.Instance.CurrentView;

            if (vm.d == 4)
            {
                if (!string.IsNullOrEmpty(vm.TBID))
                {
                    if (double.TryParse(vm.TBID, out double idNumber))
                    {
                        if (vm.TBID.Length == 12)
                        {
                            //MessageBox.Show("Valid ID");
                            ValidationSucceeded = true;
                            ClosePopup();
                        }
                        else
                        {
                            //MessageBox.Show("Invalid ID");
                            ValidationText = "CCCD không hợp lệ";
                            //DisplayText = null;
                            ValidationSucceeded = false;

                            //Visibility
                            //_ValidatePopupVisible = true;
                            //VisibilityAlphabet = Visibility.Collapsed;
                            //VisibilitySpecial = Visibility.Collapsed;
                            _PopupWindow();
                        }
                    }
                    else
                    {
                        //MessageBox.Show("Invalid ID");
                        ValidationText = "CCCD không hợp lệ";
                        //DisplayText = null;
                        ValidationSucceeded = false;

                        //Visibility
                        //_ValidatePopupVisible = true;
                        //VisibilityAlphabet = Visibility.Collapsed;
                        //VisibilitySpecial = Visibility.Collapsed;
                        _PopupWindow();

                    }
                }
                else
                {
                    ValidationSucceeded = true;
                    ClosePopup();
                }
            }


            else if (vm.d == 6)
            {
                if (string.IsNullOrEmpty(vm.TBEmail) == false)
                {
                    if (vm.IsEmailValid(vm.TBEmail))
                    {
                        ValidationSucceeded = true;
                        ClosePopup();
                    }
                    else
                    {
                        ValidationText = "Email không hợp lệ";
                        //DisplayText = null;
                        ValidationSucceeded = false;

                        //Visibility
                        //_ValidatePopupVisible = true;
                        //VisibilityAlphabet = Visibility.Collapsed;
                        //VisibilitySpecial = Visibility.Collapsed;
                        _PopupWindow();

                    }
                }
                else
                {
                    ValidationSucceeded = true;
                    ClosePopup();
                }

            }

            else if (vm.d == 7)
            {
                if (!string.IsNullOrEmpty(vm.TBPhone))
                {
                    if (double.TryParse(vm.TBPhone, out double idNumber))
                    {
                        if (vm.TBPhone.Length == 10)
                        {
                            //MessageBox.Show("Valid ID");
                            ValidationSucceeded = true;
                            ClosePopup();
                        }
                        else
                        {
                            //MessageBox.Show("Invalid ID");
                            ValidationText = "Số điện thoại không hợp lệ";
                            //DisplayText = null;
                            ValidationSucceeded = false;

                            //Visibility
                            //_ValidatePopupVisible = true;
                            //VisibilityAlphabet = Visibility.Collapsed;
                            //VisibilitySpecial = Visibility.Collapsed;
                            _PopupWindow();

                        }
                    }
                    else
                    {
                        //MessageBox.Show("Invalid ID");
                        ValidationText = "Số điện thoại không hợp lệ";
                        //DisplayText = null;
                        ValidationSucceeded = false;

                        //Visibility
                        //_ValidatePopupVisible = true;
                        //VisibilityAlphabet = Visibility.Collapsed;
                        //VisibilitySpecial = Visibility.Collapsed;
                        _PopupWindow();

                    }
                }
                else
                {
                    ValidationSucceeded = true;
                    ClosePopup();
                }
            }

            else ValidationSucceeded = true;

        }

        //public void Buyer_ValidateInformation()
        //{
        //    BuyerInformationVM vm = (BuyerInformationVM)MainWindowVM.Instance.CurrentView;

        //    if (vm.d == 7)
        //    {
        //        if (!string.IsNullOrEmpty(vm.Tax))
        //        {
        //            if (double.TryParse(vm.TBPhone, out double idNumber))
        //            {
        //                if (vm.TBPhone.Length == 10)
        //                {
        //                    //MessageBox.Show("Valid ID");
        //                    ValidationSucceeded = true;
        //                }
        //                else
        //                {
        //                    //MessageBox.Show("Invalid ID");
        //                    ValidationText = "Số điện thoại không hợp lệ";
        //                    DisplayText = null;
        //                    ValidationSucceeded = false;

        //                    //Visibility
        //                    _ValidatePopupVisible = true;
        //                    VisibilityAlphabet = Visibility.Collapsed;
        //                    VisibilitySpecial = Visibility.Collapsed;
        //                }
        //            }
        //            else
        //            {
        //                //MessageBox.Show("Invalid ID");
        //                ValidationText = "Số điện thoại không hợp lệ";
        //                DisplayText = null;
        //                ValidationSucceeded = false;

        //                //Visibility
        //                _ValidatePopupVisible = true;
        //                VisibilityAlphabet = Visibility.Collapsed;
        //                VisibilitySpecial = Visibility.Collapsed;
        //            }
        //        }
        //        else
        //        {
        //            ValidationSucceeded = true;
        //        }
        //    }

        //    else ValidationSucceeded = true;

        //}

        public void Buyer_ValidateInformation()
        {
            BuyerInformationVM vm = (BuyerInformationVM)MainWindowVM.Instance.CurrentView;

            if (vm.d == 5)
            {
                if (!string.IsNullOrEmpty(vm.Tax))
                {
                    if (vm.Tax.Length == 10 || vm.Tax.Length == 13)
                    {
                        // Tax has either 10 or 13 characters, validation succeeded
                        ValidationSucceeded = true;
                        ClosePopup();
                    }
                    else
                    {
                        // Tax doesn't have the required length
                        ValidationText = "Mã số thuế không hợp lệ";
                        //DisplayText = null;
                        ValidationSucceeded = false;

                        //Visibility
                        //_ValidatePopupVisible = true;
                        //VisibilityAlphabet = Visibility.Collapsed;
                        //VisibilitySpecial = Visibility.Collapsed;
                        _PopupWindow();
                    }
                }
                else
                {
                    // Tax is empty, validation succeeded
                    ValidationSucceeded = true;
                    ClosePopup();
                }
            }
            else
            {
                // For other fields, validation always succeeds
                ValidationSucceeded = true;
            }
        }

        public void PersonalInfo_ValidateInformation()
        {
            PersonalInfoVM vm = (PersonalInfoVM)MainWindowVM.Instance.CurrentView;

            int a = vm.d;

            if (vm.d == 1)
            {
                if (string.IsNullOrEmpty(vm.TBEmail) == false)
                {
                    if (vm.IsEmailValid(vm.TBEmail))
                    {
                        ValidationSucceeded = true;
                        ClosePopup();
                    }
                    else
                    {
                        ValidationText = "Email không hợp lệ";
                        //DisplayText = null;
                        ValidationSucceeded = false;

                        //Visibility
                        //_ValidatePopupVisible = true;
                        //VisibilityAlphabet = Visibility.Collapsed;
                        //VisibilitySpecial = Visibility.Collapsed;
                        _PopupWindow();

                    }
                }
                else
                {
                    ValidationSucceeded = true;
                    ClosePopup();
                }

            }

            else if (vm.d == 2)
            {
                if (!string.IsNullOrEmpty(vm.TBPhone))
                {
                    if (double.TryParse(vm.TBPhone, out double idNumber))
                    {
                        if (vm.TBPhone.Length == 10)
                        {
                            //MessageBox.Show("Valid ID");
                            ValidationSucceeded = true;
                            ClosePopup();
                        }
                        else
                        {
                            //MessageBox.Show("Invalid ID");
                            ValidationText = "Số điện thoại không hợp lệ";
                            //DisplayText = null;
                            ValidationSucceeded = false;

                            //Visibility
                            //_ValidatePopupVisible = true;
                            //VisibilityAlphabet = Visibility.Collapsed;
                            //VisibilitySpecial = Visibility.Collapsed;
                            _PopupWindow();
                        }
                    }
                    else
                    {
                        //MessageBox.Show("Invalid ID");
                        ValidationText = "Số điện thoại không hợp lệ";
                        //DisplayText = null;
                        ValidationSucceeded = false;

                        //Visibility
                        _ValidatePopupVisible = true;
                        //VisibilityAlphabet = Visibility.Collapsed;
                        //VisibilitySpecial = Visibility.Collapsed;
                        _PopupWindow();
                    }
                }
                else
                {
                    ValidationSucceeded = true;
                    ClosePopup();
                }
            }

            else ValidationSucceeded = true;

        }




        public void TextblockContent_BeneInfo()
        {
            BeneficiaryInfoVM vm = (BeneficiaryInfoVM)MainWindowVM.Instance.CurrentView;
            if (vm.d == 1)
            {
                DisplayTextBlock = "Họ và Tên";
                DisplayText = vm.TBName;
            }
            else if (vm.d == 2)
            {
                DisplayTextBlock = "Địa chỉ";
                DisplayText = vm.TBAdress;
            }
            else if (vm.d == 3)
            {
                DisplayTextBlock = "Nghề nghiệp";
                DisplayText = vm.TBJob;
            }
            else if (vm.d == 4)
            {
                DisplayTextBlock = "Số CCCD";
                DisplayText = vm.TBID;
            }
            //else if (vm.d == 5)
            //{
            //    DisplayTextBlock = "Quan hệ với người mua";
            //    DisplayText = vm.TBRelationship;
            //}
            else if (vm.d == 6)
            {
                DisplayTextBlock = "Email";
                DisplayText = vm.TBEmail;
            }
            else if (vm.d == 7)
            {
                DisplayTextBlock = "Số điện thoại";
                DisplayText = vm.TBPhone;
            }

        }

        public void TextblockContent_BuyerInfo()
        {
            BuyerInformationVM vm = (BuyerInformationVM)MainWindowVM.Instance.CurrentView;

            if (vm.d == 3)
            {
                DisplayTextBlock = "Nghề nghiệp";
                DisplayText = vm.Occupation;
            }
            else if (vm.d == 5)
            {
                DisplayTextBlock = "Mã số thuế";
                DisplayText = vm.Tax;
            }
            else if (vm.d == 6)
            {
                DisplayTextBlock = "Email";
                DisplayText = vm.Email;
            }
            else if (vm.d == 7)
            {
                DisplayTextBlock = "Số điện thoại";
                DisplayText = vm.PhoneNum;
            }
        }

        public void TextblockContent_UserInfo()
        {
            PersonalInfoVM vm = (PersonalInfoVM)MainWindowVM.Instance.CurrentView;

            if (vm.d == 1)
            {
                DisplayTextBlock = "Email";
                DisplayText = vm.TBEmail;
            }
            else if (vm.d == 2)
            {
                DisplayTextBlock = "Số điện thoại";
                DisplayText = vm.TBPhone;
            }

        }




        //--------------------POPUP WINDOW--------------------------

        private static PopUpWindow popUpWin;

        //public async void _PopupWindow()
        //{
        //    if (popUpWin == null || !popUpWin.IsVisible)
        //    {
        //        UserModel.Instance.PopUpTextbox = ValidationText;

        //        popUpWin = new PopUpWindow();

        //        //popUpWin.Width = 300;
        //        //popUpWin.Height = 500;
        //        popUpWin.WindowStartupLocation = WindowStartupLocation.Manual;

        //        // Position middle
        //        popUpWin.Left = (SystemParameters.PrimaryScreenWidth - popUpWin.Width) / 2;
        //        popUpWin.Top = (SystemParameters.PrimaryScreenHeight - popUpWin.Height) / 2;

        //            popUpWin.Show();
        //    }
        //    else
        //    {
        //        popUpWin.Focus();
        //    }
        //}

        //public async void _PopupWindow()
        //{
        //    if (popUpWin == null || !popUpWin.IsVisible)
        //    {
        //        UserModel.Instance.PopUpTextbox = ValidationText;

        //        popUpWin = new PopUpWindow();

        //        // Set the WindowStartupLocation to Manual
        //        popUpWin.WindowStartupLocation = WindowStartupLocation.Manual;

        //        // Position middle
        //        popUpWin.Left = (SystemParameters.PrimaryScreenWidth - popUpWin.Width) / 2;
        //        popUpWin.Top = (SystemParameters.PrimaryScreenHeight - popUpWin.Height) / 2;

        //        //// Set owner window
        //        //if (Application.Current.MainWindow != null)
        //        //{
        //        //    popUpWin.Owner = Application.Current.MainWindow;
        //        //}

        //        popUpWin.Show();
        //    }
        //    else
        //    {
        //        popUpWin.Focus();
        //    }
        //}

        //public void _PopupWindow()
        //{
        //    if (popUpWin == null || !popUpWin.IsVisible)
        //    {
        //        // Disable interaction with the main window
        //        Application.Current.MainWindow.IsEnabled = false;

        //        UserModel.Instance.PopUpTextbox = ValidationText;

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

        //        popUpWin.Closed += (sender, e) =>
        //        {
        //            // Enable interaction with the main window when popup window is closed
        //            Application.Current.MainWindow.IsEnabled = true;
        //        };

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

                UserModel.Instance.PopUpTextbox = ValidationText;

                popUpWin = new PopUpWindow();

                // Set the WindowStartupLocation to Manual
                popUpWin.WindowStartupLocation = WindowStartupLocation.Manual;

                // Position middle
                popUpWin.Left = (SystemParameters.PrimaryScreenWidth - popUpWin.Width) / 2;
                popUpWin.Top = (SystemParameters.PrimaryScreenHeight - popUpWin.Height) / 2;

                //// Set owner window
                //if (Application.Current.MainWindow != null)
                //{
                //    popUpWin.Owner = Application.Current.MainWindow;
                //}

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