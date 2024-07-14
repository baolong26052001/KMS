using Emgu.CV.CvEnum;
using Insurance.Command;
using Insurance.Model;
using Insurance.Utility;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Net;
using System.Security.Cryptography.Pkcs;
using System.Windows;
using System.Windows.Input;
using static System.Net.Mime.MediaTypeNames;

namespace Insurance.ViewModel
{
    class DetailedPackageVM : ViewModelBase
    {

        public ICommand NextCommand { get; set; }
        public ICommand BackCommand { get; set; }


        private string packageName;
        public string PackageName
        {
            get { return packageName; }
            set
            {
                if (packageName != value)
                {
                    packageName = value;
                    OnPropertyChanged(nameof(PackageName));
                }
            }
        }

        private string _packageFee;
        public string _PackageFee
        {
            get { return _packageFee; }
            set
            {
                if (_packageFee != value)
                {
                    _packageFee = value;
                    OnPropertyChanged(nameof(_PackageFee));
                }
            }
        }

        private string packageInfo;
        public string PackageInfo
        {
            get { return packageInfo; }
            set
            {
                if (packageInfo != value)
                {
                    packageInfo = value;
                    OnPropertyChanged(nameof(PackageInfo));
                }
            }
        }

        private string packageProvider;
        public string PackageProvider
        {
            get { return packageProvider; }
            set
            {
                if (packageProvider != value)
                {
                    packageProvider = value;
                    OnPropertyChanged(nameof(PackageProvider));
                }
            }
        }

        private string fullName;
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

        private string dateOfBirth;
        public string DateOfBirth
        {
            get { return dateOfBirth; }
            set
            {
                if (dateOfBirth != value)
                {
                    dateOfBirth = value;
                    OnPropertyChanged(nameof(DateOfBirth));
                }
            }
        }

        private string gender;
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

        private string idNum;
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

        private string createdDate;
        public string CreatedDate
        {
            get { return createdDate; }
            set
            {
                if (createdDate != value)
                {
                    createdDate = value;
                    OnPropertyChanged(nameof(CreatedDate));
                }
            }
        }

        private string expiredDate;
        public string ExpiredDate
        {
            get { return expiredDate; }
            set
            {
                if (expiredDate != value)
                {
                    expiredDate = value;
                    OnPropertyChanged(nameof(ExpiredDate));
                }
            }
        }

        //Kiosk and station info
        private string location;
        public string Location
        {
            get { return location; }
            set
            {
                if (location != value)
                {
                    location = value;
                    OnPropertyChanged(nameof(Location));
                }
            }
        }
        private string kioskName;
        public string KioskName
        {
            get { return kioskName; }
            set
            {
                if (kioskName != value)
                {
                    kioskName = value;
                    OnPropertyChanged(nameof(KioskName));
                }
            }
        }
        private int stationCode;
        public int StationCode
        {
            get { return stationCode; }
            set
            {
                if (stationCode != value)
                {
                    stationCode = value;
                    OnPropertyChanged(nameof(StationCode));
                }
            }
        }
        private string stationName;
        public string StationName
        {
            get { return stationName; }
            set
            {
                if (stationName != value)
                {
                    stationName = value;
                    OnPropertyChanged(nameof(StationName));
                }
            }
        }
        private string companyName;
        public string CompanyName
        {
            get { return companyName; }
            set
            {
                if (companyName != value)
                {
                    companyName = value;
                    OnPropertyChanged(nameof(CompanyName));
                }
            }
        }
        private string address;
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

        private string city;
        public string City
        {
            get { return city; }
            set
            {
                if (city != value)
                {
                    city = value;
                    OnPropertyChanged(nameof(City));
                }
            }
        }
        public DetailedPackageVM()
        {
            UserModel.Instance.KioskId = 1;
            FetchUser_PackageInfo();
            Fetch_KioskInfo();
            Fetch_StationInfo();       
            NextCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                MainWindowVM.Instance.CurrentView = new PaymentMethodVM();
            });

            BackCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                MainWindowVM.Instance.BackScreen();
            });
        }

        int packageFee;
        public void FetchUser_PackageInfo()
        {
            int _fee = UserModel.Instance.InsurancePackageName.fee;
            var _Fee = string.Format(CultureInfo.GetCultureInfo("vi-VN"), "{0:#,0.##}", _fee);
            string dateCreated = UserModel.Instance.InsurancePackageName.dateCreated.ToShortDateString();
            DateTime experiedTime = UserModel.Instance.InsurancePackageName.dateCreated.AddYears(1);
            fullName = "Họ và tên: " + UserModel.Instance.FullName;
            idNum = "Số căn cước: " + UserModel.Instance.IDCard;
            gender = "Giới tính: " + UserModel.Instance.Gender;
            dateOfBirth = "Ngày sinh: " + UserModel.Instance.Birthday;
            packageProvider = "Nhà cung cấp: " + UserModel.Instance.InsurancePackageName.provider;
            packageInfo = UserModel.Instance.InsurancePackageName.packageName + " : " + _Fee + "VND";

            _PackageFee = "Giá gói: " + _Fee;
            PackageName = "Tên gói: " + UserModel.Instance.InsurancePackageName.packageName;
            packageFee = _fee;
            CreatedDate = "Ngày mua: " + dateCreated;
            ExpiredDate = "Ngày hết hạn: " + experiedTime.ToShortDateString();
        }
        public void Fetch_KioskInfo()
        {
            var a = APIHelper.Instance.KioskDetail(UserModel.Instance.KioskId);
            if(a == null) { MessageBox.Show("Value can not be null"); return; }
            UpdateKioskInfo(a);
        }
        public void UpdateKioskInfo(List<KioskModel> kioskInfo)
        {
            KioskName = "Tên máy kiosk: " + kioskInfo[0].KioskName;
            StationCode = kioskInfo[0].StationCode;
            Location = "Khu vực: " + kioskInfo[0].Location;
        }

        public void Fetch_StationInfo()
        {
            var a = APIHelper.Instance.StationDetail(StationCode);
            if (a == null) { MessageBox.Show("Value can not be null"); return; }
            UpdateStationInfo(a);
        }
         
        public void UpdateStationInfo(List<StationKisokModel> stationKisok)
        {
            StationName = "Tên trạm: " + stationKisok[0].stationName;
            CompanyName = "Tên công ty: SpeedPOS";
            Address = "Địa chỉ: " + stationKisok[0].address;
            City = "Thành phố: " + stationKisok[0].city;
        }
    }
}