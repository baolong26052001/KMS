using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Insurance.Command;
using Insurance.Model;
using Insurance.Utility;
using Insurance.ViewModel;

namespace Insurance.ViewModel
{
    class InsuranceProviderVM : ViewModelBase
    {
        public ICommand ProviderCommand { get; set; }
        public ICommand FWDCommand { get; set; }
        public ICommand BackCommand { get; set; }

        private ObservableCollection<InsuranceProviderModel> _packageProvider;
        public ObservableCollection<InsuranceProviderModel> PackageProvider
        {
            get { return _packageProvider; }
            set
            {
                _packageProvider = value;
                OnPropertyChanged(nameof(PackageProvider));
            }
        }
        public int BaoViet_Id;
        public int FWD_Id;

        public InsuranceProviderVM()
        {

            _FetchInsuranceProvider();
            UserModel.Instance.IsHomePage = false;

            ProviderCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                int id = (int)p;
                //int _Id = 
                if (id == BaoViet_Id) MainWindowVM.Instance.CurrentView = new BaoVietVM();// Bao Viet
                //if (id == FWD_Id) MainWindowVM.Instance.CurrentView = new InsuranceFWD();// FWD
            });

            BackCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                MainWindowVM.Instance.BackScreen();
                HomeVM vm =  (HomeVM)MainWindowVM.Instance.CurrentView;
                
                //vm.InitializeTimer();
            });
        }
        public void _FetchInsuranceProvider()
        {
            var a = APIHelper.Instance.FetchInsuranceProvider();
            if (a == null) { MessageBox.Show("Value can not be null"); return; }
            UpdatePackageType(a);
            UpdatePackageId(a);
        }
        public void UpdatePackageType(List<InsuranceProviderModel> packageProvider)
        {
            PackageProvider = new ObservableCollection<InsuranceProviderModel>(packageProvider);
        }

        public void UpdatePackageId(List<InsuranceProviderModel> packageProvider)
        {
            for (int i = 0; i < packageProvider.Count; i++)
            {
                if (i == 0) BaoViet_Id = packageProvider[i].id;
                else if (i == 1) FWD_Id = packageProvider[i].id;
            }
        }


    }
}
