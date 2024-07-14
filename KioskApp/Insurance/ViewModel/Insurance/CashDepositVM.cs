using Insurance.Command;
using Insurance.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO.Ports;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

using System.IO;
using System.Reflection;
using Insurance.Utility;

namespace Insurance.ViewModel
{
    public class CashDepositVM : ViewModelBase
    {

        //Navigating
        public ICommand ConfirmCommand { get; set; }
        public ICommand BackCommand { get; set; }





        //Logic for the Cash Deposit
        private SerialPort serialPort;
        private List<int> billValues;


        private int bankNote1;
        private int bankNote2;
        private int bankNote3;
        private int bankNote4;

        private int timeout_Count;


        // Properties for data binding
        private string _totalDepositedMoney;
        public string _TotalDepositedMoney
        {
            get { return _totalDepositedMoney; }
            set
            {
                if (_totalDepositedMoney != value)
                {
                    _totalDepositedMoney = value;
                    OnPropertyChanged(nameof(_TotalDepositedMoney));
                }
            }
        }

        private int _bankNote1;
        public int BankNote1
        {
            get { return _bankNote1; }
            set
            {
                if (_bankNote1 != value)
                {
                    _bankNote1 = value;
                    OnPropertyChanged(nameof(BankNote1));
                }
            }
        }

        private int _bankNote2;
        public int BankNote2
        {
            get { return _bankNote2; }
            set
            {
                if (_bankNote2 != value)
                {
                    _bankNote2 = value;
                    OnPropertyChanged(nameof(BankNote2));
                }
            }
        }

        private int _bankNote3;
        public int BankNote3
        {
            get { return _bankNote3; }
            set
            {
                if (_bankNote3 != value)
                {
                    _bankNote3 = value;
                    OnPropertyChanged(nameof(BankNote3));
                }
            }
        }

        private int _bankNote4;
        public int BankNote4
        {
            get { return _bankNote4; }
            set
            {
                if (_bankNote4 != value)
                {
                    _bankNote4 = value;
                    OnPropertyChanged(nameof(BankNote4));
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

        private Visibility visibilityBtnConfirm = Visibility.Collapsed;
        public Visibility VisibilityBtnConfirm
        {
            get { return visibilityBtnConfirm; }
            set
            {
                visibilityBtnConfirm = value;
                OnPropertyChanged(nameof(VisibilityBtnConfirm));
            }
        }
        public int PackageFee;
        public int totalDepositedMoney;


        public CashDepositVM()
        {
            Initialize();

            if (HomeVM.Instance.ButtonCommandID == 2)
            {
                PackageFee = UserModel.Instance.DebtAmount;
                var _LoanPayback = string.Format(CultureInfo.GetCultureInfo("vi-VN"), "{0:#,0.##}", PackageFee);
                packageInfo = "Số tiền cần trả: " + _LoanPayback;
            }

            else if (HomeVM.Instance.ButtonCommandID == 4)
            {
                PackageFee = UserModel.Instance.SavingLoanAmount;
                var _SavingAmount = string.Format(CultureInfo.GetCultureInfo("vi-VN"), "{0:#,0.##}", PackageFee);
                packageInfo = "Số tiền gửi tiết kiệm: " + _SavingAmount;
            }

            else if (HomeVM.Instance.ButtonCommandID == 6)
            {
                PackageFee = UserModel.Instance.InsurancePackageName.fee;
                var _Fee = string.Format(CultureInfo.GetCultureInfo("vi-VN"), "{0:#,0.##}", PackageFee);
                packageInfo = UserModel.Instance.InsurancePackageName.packageName + " : " + _Fee;
            }


            ConfirmCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                //VisibilityBtnConfirm = Visibility.Visible;
                if (HomeVM.Instance.ButtonCommandID == 4)
                {
                    MainWindowVM.Instance.CurrentView = new InsuranceSuccessVM();
                }
                else if (HomeVM.Instance.ButtonCommandID == 6)
                {
                    MainWindowVM.Instance.CurrentView = new BeneficiaryAddVM();
                }
                APIHelper.Instance.Log($"Insurance Payment Success, MemberID: {UserModel.Instance.UserID}, Member Name: {UserModel.Instance.FullName}, Member ID card number: {UserModel.Instance.IDCard}, Package {packageInfo}");
                serialPort.Close();
            });

            BackCommand = new RelayCommand<object>((p) => { if (totalDepositedMoney > 0) return false; else return true; }, (parameter) =>
            {
                serialPort.Close();
                MainWindowVM.Instance.BackScreen();
            });
        }

        private void Initialize()
        {
            // Your initialization logic goes here
            InitializeSerialPort();

            billValues = new List<int>();

            // Start a background task to read data continuously
            Task.Run(() => ReadDataContinuously());
        }


        private void InitializeSerialPort()
        {
            try
            {
                serialPort = new SerialPort(KioskModel.Instance.CashDepositPort, 9600)
                {
                    Parity = Parity.Even,
                    DataBits = 8,
                    StopBits = StopBits.One
                };

                if (!serialPort.IsOpen)
                {
                    serialPort.Open();
                    PowerUP(serialPort);
                    KioskModel.Instance.CashDepositStatus = 1;
                }
            }
            catch (Exception ex)
            {
                // Handle the exception
                KioskModel.Instance.CashDepositStatus = 0;
                //MessageBox.Show($"An error occurred while initializing the serial port: {ex.Message}");
                APIHelper.Instance.Log($"Error occurred while initializing the serial port: {ex.Message}");
            }

            APIHelper.Instance.UpdateCashDepositStatus(KioskModel.Instance.KioskID, KioskModel.Instance.CashDepositStatus);
        }

        private void ReadDataContinuously()
        {
            try
            {
                while (true)
                {
                    int receivedData = serialPort.ReadByte();
                    // Update UI on the main thread using Dispatcher
                    Application.Current.Dispatcher.Invoke(() => UpdateUI(receivedData));
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., port closed)
                //MessageBox.Show("Error: " + ex.Message);
            }
        }


        private void UpdateUI(int receivedData)
        {
            // Your existing logic for updating UI
            SerialPort sp = serialPort;

            int data = receivedData;
            switch (data)
            {
                // Add your case statements for different responses
                case 0x80:
                    Console.WriteLine("Power is on");
                    break;
                //Bill 1000 -> 20000 VND: Reject
                case 0x40:
                    Console.WriteLine("Bill value: 1000 dong");
                    billValues.Add(0);
                    RejectBill(sp);
                    break;
                case 0x41:
                    Console.WriteLine("Bill value: 2000 dong");
                    billValues.Add(0);
                    RejectBill(sp);
                    break;
                case 0x42:
                    Console.WriteLine("Bill value: 5000 Dong");
                    billValues.Add(0);
                    RejectBill(sp);
                    break;
                case 0x43:
                    Console.WriteLine("Bill value: 10000 Dong");
                    billValues.Add(0);
                    RejectBill(sp);
                    break;
                case 0x44:
                    Console.WriteLine("Bill value: 20000 dong");
                    billValues.Add(0);
                    RejectBill(sp);
                    break;
                // Bill 50k -> 500k
                case 0x45:
                    //tbxBill.Text = ("Bill value: 50000 dong");
                    bankNote1++;
                    billValues.Add(50000);
                    HandleBill(sp);
                    break;
                case 0x46:
                    //tbxBill.Text = ("Bill value: 100000 dong");
                    bankNote2++;
                    billValues.Add(100000);
                    HandleBill(sp);
                    break;
                case 0x47:
                    //tbxBill.Text = ("Bill value: 200000 dong");
                    bankNote3++;
                    billValues.Add(200000);
                    HandleBill(sp);
                    break;
                case 0x48:
                    //tbxBill.Text = ("Bill value: 500000 dong");
                    bankNote4++;
                    billValues.Add(500000);
                    HandleBill(sp);
                    break;

                case 0x2f: // Response when Error Status is Exclusion
                    timeout_Count++;
                    HandleConsecutiveTimeouts(timeout_Count, serialPort, billValues);
                    if (timeout_Count >= 3)
                    {
                        timeout_Count = 0;
                    }
                    break;


                case 0x62: // Enable status
                    MessageBox.Show("Enable status");
                    break;

                case 0x94: // Inhibit status
                    //PowerUP(serialPort);
                    MessageBox.Show("Inhibit status");
                    break;

                default:
                    //tbxStatus.Text = "Unknown response";
                    break;
            }
        }

        private void HandleBill(SerialPort sp)
        {
            byte[] bill_Accept = { 0x02 };
            sp.Write(bill_Accept, 0, bill_Accept.Length);
            totalDepositedMoney = billValues.Sum();

            BankNote1 = bankNote1;
            BankNote2 = bankNote2;
            BankNote3 = bankNote3;
            BankNote4 = bankNote4;

            var _Total = string.Format(CultureInfo.GetCultureInfo("vi-VN"), "{0:#,0.##}", totalDepositedMoney);

            _TotalDepositedMoney = _Total;


            if (totalDepositedMoney == PackageFee)
            {
                VisibilityBtnConfirm = Visibility.Visible;
                MainWindowVM.Instance.VisibilityBtnHead = Visibility.Collapsed;// Tat nut huy bo
            }
            else
            {
                VisibilityBtnConfirm = Visibility.Collapsed;
                MainWindowVM.Instance.VisibilityBtnHead = Visibility.Visible;// Mo nut huy bo
            }
        }

        private void RejectBill(SerialPort sp)
        {
            byte[] bill_Reject = { 0x0F };
            sp.Write(bill_Reject, 0, bill_Reject.Length);

            //Thong bao cho user chi nhan tien tu 50000 VND
            //NotifyUser();

        }
        public static void HandleConsecutiveTimeouts(int timeout_Count, SerialPort sp, List<int> billValues)
        {
            //timeout_Count++;
            if (timeout_Count >= 3)
            {
                //Console.WriteLine("-------Three consecutive timeouts-------");
                byte[] bill_Reset = { 0x30 };
                sp.Write(bill_Reset, 0, bill_Reset.Length);
                //Console.WriteLine("---------Bill RESET!---------");

                byte[] bill_ON = { 0x02 };
                sp.Write(bill_ON, 0, bill_ON.Length);
                //Console.WriteLine("\n\n-----------------------PLEASE INSERT MONEY AGAIN-------------------");
            }
            else
            {
                Console.WriteLine("Error");
                //int latest_value = billValues.LastOrDefault();

                //billValues.Remove(latest_value);
                //Console.WriteLine("\n\n-----------------------PLEASE INSERT MONEY-------------------");
            }
        }

        public static void PowerUP(SerialPort sp)
        {
            byte[] bill_Reset = { 0x30 };
            sp.Write(bill_Reset, 0, bill_Reset.Length);

            byte[] bill_ON = { 0x02 };
            sp.Write(bill_ON, 0, bill_ON.Length);
        }

    }
}