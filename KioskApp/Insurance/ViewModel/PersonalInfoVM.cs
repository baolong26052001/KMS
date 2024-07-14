using Insurance.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using RestSharp;
using static Emgu.CV.VideoCapture;
using System.Diagnostics.Metrics;
using Insurance.Model;
using Emgu.CV;
using System.IO;
using System.Net.Http;
using Insurance.Utility;
using Insurance.VirtualKeyboard;
using System.ComponentModel;
using System.Reflection.Metadata;
using System.Xml.Linq;
using System.Windows.Controls;
using System.Text.RegularExpressions;
using Insurance.View;

namespace Insurance.ViewModel
{
    public class PersonalInfoVM : ViewModelBase
    {
        public ICommand NextCommand { get; set; }
        public ICommand BackCommand { get; set; }
        public ICommand TextboxCommand { get; set; }


        private static PersonalInfoVM _instance;

        public static PersonalInfoVM Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new PersonalInfoVM();
                }
                return _instance;
            }
        }


        private string _fullName;

        public string FullName
        {
            get { return _fullName; }
            set
            {
                if (_fullName != value)
                {
                    _fullName = value;
                    OnPropertyChanged(nameof(FullName));
                }
            }
        }

        private string _birthday;

        public string Birthday
        {
            get { return _birthday; }
            set
            {
                if (_birthday != value)
                {
                    _birthday = value;
                    OnPropertyChanged(nameof(Birthday));
                }
            }
        }
        private string _address;

        public string Address
        {
            get { return _address; }
            set
            {
                if (_address != value)
                {
                    _address = value;
                    OnPropertyChanged(nameof(_address));
                }
            }
        }
        private string _idNum;

        public string IDNum
        {
            get { return _idNum; }
            set
            {
                if (_idNum != value)
                {
                    _idNum = value;
                    OnPropertyChanged(nameof(IDNum));
                }
            }
        }
        private string _idExp;

        public string IDExp
        {
            get { return _idExp; }
            set
            {
                if (_idExp != value)
                {
                    _idExp = value;
                    OnPropertyChanged(nameof(IDExp));
                }
            }
        }
        private string _city;

        public string City
        {
            get { return _city; }
            set
            {
                if (_city != value)
                {
                    _city = value;
                    OnPropertyChanged(nameof(City));
                }
            }
        }
        private string _ward;

        public string Ward
        {
            get { return _ward; }
            set
            {
                if (_ward != value)
                {
                    _ward = value;
                    OnPropertyChanged(nameof(Ward));
                }
            }
        }
        private string _region;

        public string Region
        {
            get { return _region; }
            set
            {
                if (_region != value)
                {
                    _region = value;
                    OnPropertyChanged(nameof(Region));
                }
            }
        }
        private string _gender;

        public string Gender
        {
            get { return _gender; }
            set
            {
                if (_gender != value)
                {
                    _gender = value;
                    OnPropertyChanged(nameof(Gender));
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

        private Visibility _visibilityBackButton = Visibility.Collapsed;
        public Visibility VisibilityBackButton
        {
            get { return _visibilityBackButton; }
            set
            {
                _visibilityBackButton = value;
                OnPropertyChanged(nameof(VisibilityBackButton));
            }
        }
        public int d;


        //change color in contactButton
        private Brush _contactButtonBackground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFF"));
        public Brush ContactButtonBackground
        {
            get { return _contactButtonBackground; }
            set
            {
                if (_contactButtonBackground != value)
                {
                    _contactButtonBackground = value;
                    OnPropertyChanged(nameof(ContactButtonBackground));
                }
            }
        }

        private Brush _contactButtonTextColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#010101"));
        public Brush ContactButtonTextColor
        {
            get { return _contactButtonTextColor; }
            set
            {
                if (_contactButtonTextColor != value)
                {
                    _contactButtonTextColor = value;
                    OnPropertyChanged(nameof(ContactButtonTextColor));
                }
            }
        }

        private ImageSource _contactButtonImage = new BitmapImage(new Uri("/Images/Contact_Green.png", UriKind.Relative));
        public ImageSource ContactButtonImage
        {
            get { return _contactButtonImage; }
            set
            {
                if (_contactButtonImage != value)
                {
                    _contactButtonImage = value;
                    OnPropertyChanged(nameof(ContactButtonImage));
                }
            }
        }

        //change color in inforButton
        private Brush _inforButtonBackground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#0EAB00"));
        public Brush InforButtonBackground
        {
            get { return _inforButtonBackground; }
            set
            {
                if (_inforButtonBackground != value)
                {
                    _inforButtonBackground = value;
                    OnPropertyChanged(nameof(InforButtonBackground));
                }
            }
        }

        private Brush _inforButtonTextColor = Brushes.White;
        public Brush InforButtonTextColor
        {
            get { return _inforButtonTextColor; }
            set
            {
                if (_inforButtonTextColor != value)
                {
                    _inforButtonTextColor = value;
                    OnPropertyChanged(nameof(InforButtonTextColor));
                }
            }
        }

        private ImageSource _inforButtonImage = new BitmapImage(new Uri("/Images/Info_White.png", UriKind.Relative));
        public ImageSource InforButtonImage
        {
            get { return _inforButtonImage; }
            set
            {
                if (_inforButtonImage != value)
                {
                    _inforButtonImage = value;
                    OnPropertyChanged(nameof(InforButtonImage));
                }
            }
        }




        private Visibility _informationviewVisibility = Visibility.Visible;
        public Visibility InformationviewVisibility
        {
            get { return _informationviewVisibility; }
            set
            {
                if (_informationviewVisibility != value)
                {
                    _informationviewVisibility = value;
                    OnPropertyChanged(nameof(InformationviewVisibility));
                }
            }
        }

        private Visibility _contactiewVisibility = Visibility.Collapsed;
        public Visibility ContactViewVisibility
        {
            get { return _contactiewVisibility; }
            set
            {
                if (_contactiewVisibility != value)
                {
                    _contactiewVisibility = value;
                    OnPropertyChanged(nameof(ContactViewVisibility));
                }
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

        public HomeVM HomeViewModel => HomeVM.Instance;
        public ICommand ContactButtonClickCommand => new RelayCommand(ContactButton_Click);
        public ICommand InformationClickCommand => new RelayCommand(InformationButton_Click);




        public PersonalInfoVM()
        {
            if (HomeVM.Instance.ButtonCommandID == 99)
            {
                VisibilityBackButton = Visibility.Visible;// Enable visibility for back button only when open through Edit command
            }
            _FetchUserInfo(UserModel.Instance.UserID);
            //_FetchUserInfo(35);

            NextCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                if (TextboxesFilled())
                {
                    string email = TBEmail;
                    string phoneNumber = TBPhone;

                    UploadOcrFieldsToApi(UserModel.Instance.UserID, email, phoneNumber);

                }
                else
                {
                    _EmptyPopupVisible = true;
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
                vm.TextblockContent_UserInfo();
            });
        }

        private void InformationButton_Click(object parameter)
        {
            // Update visual properties for the Contact Button
            ContactButtonBackground = Brushes.White;
            ContactButtonTextColor = Brushes.Black;
            ContactButtonImage = new BitmapImage(new Uri("/Images/Contact_Green.png", UriKind.Relative));

            // Update visual properties for the Information Button
            InforButtonBackground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#0EAB00"));
            InforButtonTextColor = Brushes.White;
            InforButtonImage = new BitmapImage(new Uri("/Images/Info_White.png", UriKind.Relative));



            // Show Information_form
            InformationviewVisibility = Visibility.Visible;

            // Hide Contact_view
            ContactViewVisibility = Visibility.Collapsed;
        }

        private void ContactButton_Click(object parameter)
        {
            // Update visual properties for the Contact Button
            ContactButtonBackground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#0EAB00"));
            ContactButtonTextColor = Brushes.White;
            ContactButtonImage = new BitmapImage(new Uri("/Images/Contact_White.png", UriKind.Relative));

            // Update visual properties for the Infor Button
            InforButtonBackground = Brushes.White;
            InforButtonTextColor = Brushes.Black;
            InforButtonImage = new BitmapImage(new Uri("/Images/Info_Green.png", UriKind.Relative));

            // Hide Information_form
            InformationviewVisibility = Visibility.Collapsed;

            // Show Contact_view
            ContactViewVisibility = Visibility.Visible;

        }

        //public void UploadOcrFieldsToApi(string ocrFilePath, string idImagepath)
        private RestClient _restClient = new RestClient();
        public void UploadOcrFieldsToApi(int idMember, string email, string phoneNumber)
        {
            try
            {
                string apiEndpoint = $"{KioskModel.Instance.APIPort}api/Member/UpdateMemberEmailAndPhone/";
                //string apiEndpoint = "http://192.168.1.85:88/api/Member/UpdateMemberEmailAndPhone/";
                idMember = UserModel.Instance.UserID;
                string fullApiUrl = Path.Combine(apiEndpoint, idMember.ToString());
                APIHelper.Instance.Log("Info: Update member information");
                var request = new RestRequest(fullApiUrl, Method.Put);

                // Add phone and email parameters to the request body
                request.AddParameter("application/json",
                    Newtonsoft.Json.JsonConvert.SerializeObject(new { phone = phoneNumber, email = email }),
                    ParameterType.RequestBody);
                APIHelper.Instance.Log("API: Update phone and email");
                RestResponse response = _restClient.Execute(request);
                APIHelper.Instance.Log("API: return result");
                if (response.IsSuccessful)
                {
                    Console.WriteLine("Fields uploaded successfully. Server response: " + response.Content);

                    //HomeViewModel.FirstName = UserModel.Instance.FirstName;
                    //HomeViewModel.LastName = UserModel.Instance.LastName;
                    //HomeViewModel.FullName = UserModel.Instance.FullName;
                    //UserModel.Instance.IsLogin = true;
                    APIHelper.Instance.Log("API: Update member information successfull");
                    if (UserModel.Instance.IsEditInfo)
                    {
                        MainWindowVM.Instance.CurrentView = HomeVM.Instance;
                    }
                    else
                    {
                        MainWindowVM.Instance.CurrentView = new RegisterSuccessVM();
                    }
                }
                else
                {
                    Console.WriteLine("Error uploading fields. Status Code: " + response.StatusCode);
                    Console.WriteLine("Response Content: " + response.Content);
                    APIHelper.Instance.Log($"Error when update personal information: {response.Content}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception in uploading fields: " + ex.Message);
                APIHelper.Instance.Log($"Error: API UpdateMemberEmailAndPhone {ex.Message}");
            }
        }

        public void _FetchUserInfo(int id)
        {
            var a = APIHelper.Instance.FetchUserInfo(id);
            if (a == null)
            {
                APIHelper.Instance.Log($"Error: Member id not found");
                return;
            }
            UpdateUserInfo(a);
        }

        public void UpdateUserInfo(List<UserInfoModel> userInfo)
        {

            FullName = userInfo[0].fullName;
            Birthday = userInfo[0].birthday.ToShortDateString();
            Address = userInfo[0].address1;
            IDNum = userInfo[0].idenNumber;
            Ward = userInfo[0].ward;
            City = userInfo[0].city;
            Gender = userInfo[0].gender;
            if (userInfo[0].email != null && userInfo[0].phone != null)
            {
                tbEmail = userInfo[0].email;
                tbPhone = userInfo[0].phone;
            }
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
                if (d == 1) vm.DisplayText = TBEmail;
                else if (d == 2) vm.DisplayText = TBPhone;

                virtualkb.Closed += VirtualKeyboardClosed;
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

            vm.PersonalInfo_ValidateInformation();

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
                if (d == 1) TBEmail = vm.DisplayText;
                else if (d == 2) TBPhone = vm.DisplayText;
            }
        }

        public bool IsEmailValid(string email)
        {
            // Regular expression pattern for validating email addresses
            string pattern = @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$";
            return Regex.IsMatch(email, pattern);
        }

        public bool TextboxesFilled()
        {
            return !string.IsNullOrEmpty(TBEmail)
                && !string.IsNullOrEmpty(TBPhone);

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