using Insurance.Command;
using Insurance.Model;
using Insurance.Utility;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Insurance.ViewModel
{
    class AnGiaPackVM : ViewModelBase
    {
        private List<InsurancePackageModel> packageData;

        public ICommand PurchaseCommand { get; set; }
        public ICommand BackCommand { get; set; }
        public ICommand PackageCommand { get; set; }

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

        public int PackageID = -1;

        public AnGiaPackVM()
        {
            AnGiaVM vm = (AnGiaVM)MainWindowVM.Instance.CurrentView;

            int termID = vm.Term_ID;
            int ageID = UserModel.Instance.AgeRangeID;

            _FetchInsurancePackage(ageID, termID);
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

        public void _FetchInsurancePackage(int age, int term)
        {
            var packages = APIHelper.Instance.FetchAnGiaPackage(age, term);
            if (packages == null)
            {
                APIHelper.Instance.Log("Error: Insurance package is null");
                return;
            }
            packageData = packages;
            PackageID = packageData[0].headerId;
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

        public void UpdatePackageDetails(List<InsurancePackageDetailModel> packageDetails)
        {
            PackageDetails = new ObservableCollection<InsurancePackageDetailModel>(packageDetails);
        }

        public void UpdatePackageInfo(List<InsurancePackageModel> packageInfo)
        {
            PackageInfo = new ObservableCollection<InsurancePackageModel>(packageInfo);
        }

        private void FetchDefaultPackageDetail()
        {
            _FetchInsurancePackageDetail(PackageID);
            UserModel.Instance.InsurancePackageName = packageData.FirstOrDefault(m => m.headerId == PackageID);
        }
    }
}
