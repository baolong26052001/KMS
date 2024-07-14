using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Insurance.Command;
using Insurance.Model;
using Insurance.Utility;
using Newtonsoft.Json;
using static Insurance.Utility.APIHelper;

namespace Insurance.ViewModel
{
    public class InsuranceTransactionVM : ViewModelBase
    {
        private ObservableCollection<InsuranceTransactionHeader> _insuranceHeader;
        public ObservableCollection<InsuranceTransactionHeader> InsuranceHeader
        {
            get { return _insuranceHeader; }
            set
            {
                _insuranceHeader = value;
                OnPropertyChanged(nameof(InsuranceHeader));
            }
        }

        public ICommand NextButtonClickCommand => new RelayCommand(NextButtonClick);
        public ICommand BackCommand { get; set; }

        public InsuranceTransactionVM()
        {

            LoadData(UserModel.Instance.UserID);
            BackCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                MainWindowVM.Instance.BackScreen();
            });
        }

        private void LoadData(int memberId)
        {
            try
            {
                List<InsuranceTransactionHeader> headers = Instance.InsuranceHeader(memberId);

                if (headers == null)
                {
                    headers = new List<InsuranceTransactionHeader>();
                }
                else
                {
                    foreach (var header in headers)
                    {
                        header.FormattedAnnualPay = header.annualPay.ToString("N0").Replace(",", ".");
                    }
                }

                InsuranceHeader = new ObservableCollection<InsuranceTransactionHeader>(headers);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void NextButtonClick(object parameter)
        {
            if (parameter is InsuranceTransactionHeader selectedHeader)
            {
                MainWindowVM.Instance.CurrentView = new InsuranceHistoryVM(selectedHeader.id);
            }
            else
            {
                MessageBox.Show("Invalid parameter passed.");
            }
        }
    }
}
