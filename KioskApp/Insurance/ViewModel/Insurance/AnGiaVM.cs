using Insurance.Command;
using Insurance.Model;
using Insurance.Utility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using Insurance.ViewModel;
using Insurance.View;

namespace Insurance.ViewModel
{
    class AnGiaVM : ViewModelBase
    {
        public ICommand TermCommand { get; set; }
        public ICommand TermBCommand { get; set; }
        public ICommand BackCommand { get; set; }

        private ObservableCollection<InsuranceTermModel> _packageTerm;
        public ObservableCollection<InsuranceTermModel> PackageTerm
        {
            get { return _packageTerm; }
            set
            {
                _packageTerm = value;
                OnPropertyChanged(nameof(PackageTerm));
            }
        }

        public int TermA_ID;
        public int TermB_ID;
        public int Term_ID;
  


        public AnGiaVM()
        {
            _FetchInsuranceTerm();
           
            TermCommand = new RelayCommand<object>((p) => { return true; },(p) =>
            {
                Term_ID = (int)p;
                if (Term_ID == TermA_ID) MainWindowVM.Instance.CurrentView = new AnGiaPackVM();
                if (Term_ID == TermB_ID) MainWindowVM.Instance.CurrentView = new AnGiaPackVM();
            });

            BackCommand = new RelayCommand<object>((p) => { return true; },(p) =>
            {
                MainWindowVM.Instance.BackScreen();
            });

        }

        public void _FetchInsuranceTerm()
        {
            var a = APIHelper.Instance.FetchInsuranceTerm();
            if (a == null) { MessageBox.Show("Value can not be null"); return; }
            UpdatePackageType(a);
            UpdateTermID(a);
        }
        public void UpdatePackageType(List<InsuranceTermModel> packageTerm)
        {
            PackageTerm = new ObservableCollection<InsuranceTermModel>(packageTerm);
        }
       
        public void UpdateTermID(List<InsuranceTermModel> packageTerm)
        {
            for (int i = 0; i < packageTerm.Count; i++)
            {
                if (i == 0) TermA_ID = packageTerm[i].id;
                else if (i == 1) TermB_ID = packageTerm[i].id;
            }
        }
    }
}
