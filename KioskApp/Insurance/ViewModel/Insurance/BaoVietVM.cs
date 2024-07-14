using Insurance.Command;
using Insurance.Model;
using Insurance.Utility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using Insurance.ViewModel;


namespace Insurance.ViewModel
{
    class BaoVietVM : ViewModelBase
    {
        public ICommand PackageCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        public ICommand BackCommand { get; set; }

        private ObservableCollection<InsuranceTypeModel> _packageType;
        public ObservableCollection<InsuranceTypeModel> PackageType
        {
            get { return _packageType; }
            set
            {
                _packageType = value;
                OnPropertyChanged(nameof(PackageType));
            }
        }

        public int AnBinhID;
        public int AnGiaID;
        public int InsuranceTypeID;

        public BaoVietVM()
        {
            _FetchInsuranceType();

            PackageCommand = new RelayCommand<object>((p) => { return true; },(p) =>
            {
                int id = (int)p;
                if (id == AnBinhID) MainWindowVM.Instance.CurrentView = new AnBinhYenVuiVM();
                if (id == AnGiaID) MainWindowVM.Instance.CurrentView = new AnGiaVM();

                //InsuranceTypeID = (int)p;
                //MainWindowVM.Instance.CurrentView = new InsurancePackageVM();
            });

            CancelCommand = new RelayCommand<object>((p) => { return true; },(p) =>
            {
                MainWindowVM.Instance.CurrentView = HomeVM.Instance;
            });

            BackCommand = new RelayCommand<object>((p) => { return true; },(p) =>
            {
                MainWindowVM.Instance.BackScreen();
            });

        }

        public void _FetchInsuranceType()
        {
            var a = APIHelper.Instance.FetchInsuranceType();
            if (a == null) { MessageBox.Show("Value can not be null"); return; }
            UpdatePackageType(a);
            UpdatePackageId(a);
        }
        public void UpdatePackageType(List<InsuranceTypeModel> packageType)
        {
            PackageType = new ObservableCollection<InsuranceTypeModel>(packageType);
        }
        public void UpdatePackageId(List<InsuranceTypeModel> packageType)
        {
            for (int i = 0; i < packageType.Count; i++)
            {
                if (i == 0) AnBinhID = packageType[i].id;
                else if (i == 1) AnGiaID = packageType[i].id;
            }
        }
    }
}
