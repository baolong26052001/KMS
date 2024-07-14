using Insurance.Command;
using Insurance.Model;
using Insurance.Utility;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;
using System.Windows;
using System.Windows.Input;
using System.Xml.Linq;

namespace Insurance.ViewModel
{
    class BeneficiaryAddVM : ViewModelBase
    {
        public ICommand AddBeneficiaryCommand { get; set; }
        public ICommand EditBeneficiaryCommand { get; set; }
        public ICommand DoneCommand { get; set; }


        private string beneficiary_Name { get; set; }
        public string Beneficiary_Name
        {
            get { return beneficiary_Name; }
            set
            {
                if (beneficiary_Name != value)
                {
                    beneficiary_Name = value;

                    OnPropertyChanged(nameof(Beneficiary_Name));

                }
            }
        }

        private string beneficiary_ID { get; set; }
        public string Beneficiary_ID
        {
            get { return beneficiary_ID; }
            set
            {
                if (beneficiary_ID != value)
                {
                    beneficiary_ID = value;

                    OnPropertyChanged(nameof(Beneficiary_ID));

                }
            }
        }

        private Visibility _visibilityDashButton = Visibility.Visible;
        public Visibility VisibilityDashButton
        {
            get { return _visibilityDashButton; }
            set
            {
                _visibilityDashButton = value;
                OnPropertyChanged(nameof(VisibilityDashButton));
            }
        }

        private Visibility _visibilityEditButton = Visibility.Visible;
        public Visibility VisibilityEditButton
        {
            get { return _visibilityEditButton; }
            set
            {
                _visibilityEditButton = value;
                OnPropertyChanged(nameof(VisibilityEditButton));
            }
        }

        private Visibility _visibilityDoneButton = Visibility.Visible;
        public Visibility VisibilityDoneButton
        {
            get { return _visibilityDoneButton; }
            set
            {
                _visibilityDoneButton = value;
                OnPropertyChanged(nameof(VisibilityDoneButton));
            }
        }

        public BeneficiaryAddVM()
        {

            // BackCommand = new RelayCommand(RR);
            //BeneficiaryInfoVM vm  = (BeneficiaryInfoVM)MainWindowVM.Instance.CurrentView;

            //Beneficiary_Name = vm.TBName;
            //Beneficiary_ID = vm.TBID;

            //Set Visibility
            VisibilityDashButton = Visibility.Visible;
            VisibilityDoneButton = Visibility.Collapsed;
            VisibilityEditButton = Visibility.Collapsed;

            LoadBeneficiaryInformation();

            AddBeneficiaryCommand = new RelayCommand<object>((p) => { return true; },(p) =>
            {
                //MainWindowVM.Instance.CurrentView = new InsuranceHomeVM();
                MainWindowVM.Instance.CurrentView = new BeneficiaryInfoVM();
            });
            EditBeneficiaryCommand = new RelayCommand<object>((p) => { return true; }, (p) => 
            {
                MainWindowVM.Instance.CurrentView = new BeneficiaryInfoVM();
            });

            DoneCommand = new RelayCommand<object>((p) => { return true; },(p) =>
            {
                API_Beneficiary();
                MainWindowVM.Instance.CurrentView = new InsuranceSuccessVM();
            });
        }

        public void LoadBeneficiaryInformation()
        {
            //if (UserModel.Instance.InsuranceBeneficiary != null && UserModel.Instance.InsuranceBeneficiary.Count > 0)
            //{
            //    Beneficiary_Name = UserModel.Instance.InsuranceBeneficiary[0].beneficiaryName;
            //    Beneficiary_ID = UserModel.Instance.InsuranceBeneficiary[0].beneficiaryId;
            //}
            var lastBeneficiary = UserModel.Instance.InsuranceBeneficiary?.LastOrDefault();
            if (lastBeneficiary != null)
            {
                Beneficiary_Name = lastBeneficiary.beneficiaryName;
                Beneficiary_ID = lastBeneficiary.beneficiaryId;
            }

        }

        public void API_Beneficiary()
        {
            int result = APIHelper.Instance.InsuranceBeneficiary();
            if (result != 200)
            {
                return;
            }
        }

    }
}
