using Insurance.Command;
using Insurance.Model;
using Insurance.Utility;
using Insurance.View;
using Insurance.VirtualKeyboard;
using System;
using System.ComponentModel;
using System.Data.Common;
using System.Reflection.Metadata;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;

namespace Insurance.ViewModel
{
    class BeneficiaryInfoVM : ViewModelBase
    {
        public ICommand NextCommand { get; set; }
        public ICommand BackCommand { get; set; }
        public ICommand TextboxCommand { get; set; }
        public ICommand ButtonPopupCommand { get; set; }

        private string tbName { get; set; }
        public string TBName
        {
            get { return tbName; }
            set
            {
                if (tbName != value)
                {
                    tbName = value;

                    OnPropertyChanged(nameof(TBName));

                }
            }
        }
        private DateTime dob { get; set; }
        public DateTime DOB
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

        private string tbAdress { get; set; }
        public string TBAdress
        {
            get { return tbAdress; }
            set
            {
                if (tbAdress != value)
                {
                    tbAdress = value;

                    OnPropertyChanged(nameof(TBAdress));

                }
            }
        }

        private string tbJob { get; set; }
        public string TBJob
        {
            get { return tbJob; }
            set
            {
                if (tbJob != value)
                {
                    tbJob = value;

                    OnPropertyChanged(nameof(TBJob));

                }
            }
        }

        private string tbID { get; set; }
        public string TBID
        {
            get { return tbID; }
            set
            {
                if (tbID != value)
                {
                    tbID = value;

                    OnPropertyChanged(nameof(TBID));

                }
            }
        }

        //private string tbRelationship { get; set; }
        //public string TBRelationship
        //{
        //    get { return tbRelationship; }
        //    set
        //    {
        //        if (tbRelationship != value)
        //        {
        //            tbRelationship = value;

        //            OnPropertyChanged(nameof(TBRelationship));

        //        }
        //    }
        //}

        private string relationship { get; set; }
        public string Relationship
        {
            get { return relationship; }
            set
            {
                if (relationship != value)
                {
                    relationship = value;

                    OnPropertyChanged(nameof(Relationship));

                }
            }
        }

        private string tbEmail { get; set; }
        public string TBEmail
        {
            get { return tbEmail; }
            set
            {
                if (tbEmail != value)
                {
                    tbEmail = value;

                    OnPropertyChanged(nameof(TBEmail));

                }
            }
        }

        private string tbPhone { get; set; }
        public string TBPhone
        {
            get { return tbPhone; }
            set
            {
                if (tbPhone != value)
                {
                    tbPhone = value;

                    OnPropertyChanged(nameof(TBPhone));

                }
            }
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


        public BeneficiaryInfoVM()
        {

            LoadInsuranceBeneficiary();

            MainWindowVM.Instance.setVisibilityHeader();
            DOB = new DateTime(2000, 1, 1);



            NextCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                if (TextboxesFilled())
                {
                    //_EmptyPopupVisible = false;
                    //MainWindowVM.Instance.CurrentView = new InsuranceSuccessVM();
                    SaveInsuranceBeneficiary();
                    MainWindowVM.Instance.CurrentView = new BeneficiaryAddVM();

                    BeneficiaryAddVM vm = (BeneficiaryAddVM)MainWindowVM.Instance.CurrentView;
                    vm.VisibilityDashButton = Visibility.Collapsed;
                    vm.VisibilityEditButton = Visibility.Visible;
                    vm.VisibilityDoneButton = Visibility.Visible;
                }
                else
                {
                    //_EmptyPopupVisible = true; // de k kich hoat textbox command
                    _PopupWindow();

                }
            });

            BackCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                MainWindowVM.Instance.BackScreen();
            });

            TextboxCommand = new RelayCommand<object>((p) => { return !_EmptyPopupVisible; }, (p) =>
            {
                TextBox tb = (TextBox)p;
                d = Convert.ToInt32(tb.ToolTip);

                PopupKeyboard();

                VirtualKeyboardVM vm = (VirtualKeyboardVM)virtualkb.DataContext;
                vm.PropertyChanged += VirtualKeyboardVM_PropertyChanged;
                vm.TextblockContent_BeneInfo();

            });

            ButtonPopupCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                _EmptyPopupVisible = false;
                VisibilityTextbox = Visibility.Visible;
            });

        }

        private static VirtualKeyboardView virtualkb;

        public void PopupKeyboard()
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
                if (d == 1) vm.DisplayText =  TBName;
                else if (d == 2) vm.DisplayText = TBAdress;
                else if (d == 3) vm.DisplayText = TBJob;
                else if (d == 4) vm.DisplayText = TBID;
                else if (d == 6) vm.DisplayText = TBEmail;
                else if (d == 7) vm.DisplayText = TBPhone;
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

        public int a;

        private void VirtualKeyboardClosed(object sender, EventArgs e)
        {
            VisibilityTextbox = Visibility.Visible;
        }
        public void CloseKeyboard()
        {
            if (virtualkb == null) return;

            VirtualKeyboardVM vm = (VirtualKeyboardVM)virtualkb.DataContext;

            vm.ValidateInformation();

            if (vm.ValidationSucceeded == true)
            {
                virtualkb.Closed -= VirtualKeyboardClosed;
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
                if (d == 1) TBName = vm.DisplayText;
                else if (d == 2) TBAdress = vm.DisplayText;
                else if (d == 3) TBJob = vm.DisplayText;
                else if (d == 4) TBID = vm.DisplayText;
                //else if (d == 5) TBRelationship = vm.DisplayText;
                else if (d == 6) TBEmail = vm.DisplayText;
                else if (d == 7) TBPhone = vm.DisplayText;
            }
        }

        //public void API_Beneficiary()
        //{
        //    SaveInsuranceBeneficiary();
        //    int result = APIHelper.Instance.InsuranceBeneficiary();
        //    if (result != 200)
        //    {
        //        return;
        //    }
        //}

        public void LoadInsuranceBeneficiary()
        {

            var lastBeneficiary = UserModel.Instance.InsuranceBeneficiary?.LastOrDefault();
            if (lastBeneficiary != null)
            {
                TBName = lastBeneficiary.beneficiaryName;
                TBID = lastBeneficiary.beneficiaryId;
                Relationship = lastBeneficiary.relationship;
                TBAdress = lastBeneficiary.address;
                DOB = lastBeneficiary.birthday;
                Gender = lastBeneficiary.gender;
                TBJob = lastBeneficiary.occupation;
                TBEmail = lastBeneficiary.email;
                TBPhone = lastBeneficiary.phone;
            }

        }

        public void SaveInsuranceBeneficiary()
        {
            if (UserModel.Instance.InsuranceBeneficiary == null)
            {
                UserModel.Instance.InsuranceBeneficiary = new List<InsuranceBeneficiaryModel>();
            }

            InsuranceBeneficiaryModel ins = new InsuranceBeneficiaryModel();
            ins.memberId = UserModel.Instance.UserID;
            ins.beneficiaryName = TBName;
            ins.beneficiaryId = TBID;
            ins.relationship = Relationship;
            ins.address = TBAdress;
            ins.birthday = DOB;
            ins.gender = Gender;
            ins.occupation = TBJob;
            ins.email = TBEmail;
            ins.phone = TBPhone;
            //ins.taxCode = TBTax;

            UserModel.Instance.InsuranceBeneficiary.Add(ins);
        }


        public bool TextboxesFilled()
        {
            return !string.IsNullOrEmpty(TBName)
                && !string.IsNullOrEmpty(TBAdress)
                && !string.IsNullOrEmpty(TBJob)
                && !string.IsNullOrEmpty(Gender)
                && !string.IsNullOrEmpty(TBID)
                && !string.IsNullOrEmpty(Relationship)
                && !string.IsNullOrEmpty(TBEmail)
                && !string.IsNullOrEmpty(TBPhone);

        }

        public bool IsEmailValid(string email)
        {
            // Regular expression pattern for validating email addresses
            string pattern = @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$";
            return Regex.IsMatch(email, pattern);
        }

        //--------------------POPUP WINDOW--------------------------

        private static PopUpWindow popUpWin;

        //public async void _PopupWindow()
        //{
        //    if (popUpWin == null || !popUpWin.IsVisible)
        //    {
        //        UserModel.Instance.PopUpTextbox = "Vui lòng điền đầy đủ thông tin";

        //        popUpWin = new PopUpWindow();

        //        //popUpWin.Width = 300;
        //        //popUpWin.Height = 500;
        //        popUpWin.WindowStartupLocation = WindowStartupLocation.Manual;

        //        // Position middle
        //        popUpWin.Left = (SystemParameters.PrimaryScreenWidth - popUpWin.Width) / 2;
        //        popUpWin.Top = (SystemParameters.PrimaryScreenHeight - popUpWin.Height) / 2;

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