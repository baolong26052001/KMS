using Insurance.Command;
using Insurance.Utility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Insurance.Model;

namespace Insurance.ViewModel
{
    public class AnBinhYenVuiVM : ViewModelBase
    {
        private List<InsurancePackageModel> packageData;

        public ICommand PurchaseCommand { get; set; }
        public ICommand BackCommand { get; set; }
        public ICommand PackageCommand { get; set; }

        private ObservableCollection<InsurancePackageModel> _packageInfo;
        public ObservableCollection<InsurancePackageModel> PackageInfo
        {
            get { return _packageInfo; }
            set
            {
                _packageInfo = value;
                OnPropertyChanged(nameof(PackageInfo));
            }
        }

        private ObservableCollection<InsurancePackageDetailModel> _packageDetails;
        public ObservableCollection<InsurancePackageDetailModel> PackageDetails
        {
            get { return _packageDetails; }
            set
            {
                _packageDetails = value;
                OnPropertyChanged(nameof(PackageDetails));
            }
        }

        public string DemoPackageName;
        public AnBinhYenVuiVM()
        {
            BaoVietVM vm = (BaoVietVM)MainWindowVM.Instance.CurrentView;
            packageData = null;
            _FetchInsurancePackage(vm.AnBinhID);

            FetchDefaultPackageDetail();

            PurchaseCommand = new RelayCommand<object>((p) => PackageID != -1, (p) =>
            {
                MainWindowVM.Instance.CurrentView = new BuyerInformationVM();
            });

            BackCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                MainWindowVM.Instance.BackScreen();
            });

            PackageCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                PackageID = (int)p;
                _FetchInsurancePackageDetail(PackageID);
                try
                {
                    UserModel.Instance.InsurancePackageName = packageData.FirstOrDefault(m => m.headerId == PackageID);
                }
                catch
                {
                    APIHelper.Instance.Log("Error: An error occur when handling fetch insurance package details");
                }
            });
        }

        public int PackageID = -1;

        public void _FetchInsurancePackage(int packageID)
        {
            var packages = APIHelper.Instance.FetchInsurancePackage(packageID);
            if (packages == null)
            {
                APIHelper.Instance.Log("Error: Insurance package is null");
                return;
            }
            packageData = packages;
            PackageID = packageData[0].headerId; // Default package
            UpdatePackageInfo(packages);
        }

        public void _FetchInsurancePackageDetail(int packageId)
        {
            var details = APIHelper.Instance.FetchInsurancePackageDetailForKioskApp(packageId);
            if (details == null)
            {
                APIHelper.Instance.Log("Error: Insurance package is null");
                return;
            }
            UpdatePackageDetails(details);
        }

        public void UpdatePackageInfo(List<InsurancePackageModel> packageInfo)
        {
            PackageInfo = new ObservableCollection<InsurancePackageModel>(packageInfo);
        }

        public void UpdatePackageDetails(List<InsurancePackageDetailModel> packageDetails)
        {
            PackageDetails = new ObservableCollection<InsurancePackageDetailModel>(packageDetails);
        }

        private void FetchDefaultPackageDetail()
        {
            _FetchInsurancePackageDetail(PackageID);
            UserModel.Instance.InsurancePackageName = packageData.FirstOrDefault(m => m.headerId == PackageID);
        }
    }
}
