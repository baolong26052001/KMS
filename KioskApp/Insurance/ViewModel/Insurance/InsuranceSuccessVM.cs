using Insurance.Command;
using Insurance.Model;
using Insurance.Utility;
using Insurance.View;
using System;
using System.Net.Mail;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Insurance.Service;

namespace Insurance.ViewModel
{
    class InsuranceSuccessVM : ViewModelBase
    {
        public ICommand DoneCommand { get; set; }

        private Visibility insuranceVisibility = Visibility.Collapsed;
        public Visibility InsuranceVisibility
        {
            get { return insuranceVisibility; }
            set
            {
                insuranceVisibility = value;
                OnPropertyChanged(nameof(InsuranceVisibility));
            }
        }

        private Visibility savingVisibility = Visibility.Collapsed;
        public Visibility SavingVisibility
        {
            get { return savingVisibility; }
            set
            {
                savingVisibility = value;
                OnPropertyChanged(nameof(SavingVisibility));
            }
        }

        public InsuranceSuccessVM()
        {
            //MainWindowVM.Instance.VisibilityHeader = Visibility.Collapsed;
            Print_Services printer = new Print_Services();
            //Loan Payback
            if (HomeVM.Instance.ButtonCommandID == 2)
            {
                //SaveSavingTransaction();
                UserModel.Instance.SavingTransaction.status = 1;
                int result = APIHelper.Instance.SavingTransaction();

                if (result != 200)
                {
                    return;
                }
            }

            //Saving
            else if (HomeVM.Instance.ButtonCommandID == 4)
            {
                SavingVisibility = Visibility.Visible;
                //SaveSavingTransaction();
                UserModel.Instance.SavingTransaction.status = 1;
                int result = APIHelper.Instance.SavingTransaction();

                if (result != 200)
                {
                    return;
                }
            }

            //Insurance
            else if (HomeVM.Instance.ButtonCommandID == 6)
            {
                InsuranceVisibility = Visibility.Visible;
                SaveInsuranceTransaction();
                SendEmail();

                int result = APIHelper.Instance.InsuranceTransaction();

                if (result != 200)
                {
                    return;
                }
            }


            Timer timer = new Timer(TimerCallback, null, TimeSpan.FromSeconds(10), TimeSpan.FromMilliseconds(-1)); // Set Timer Count down 10 second.

            DoneCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                if (UserModel.Instance.PrintCheck == 0 && HomeVM.Instance.ButtonCommandID == 6)
                {
                    printer.Print();
                    UserModel.Instance.PrintCheck++;
                }
                MainWindowVM.Instance.setDefaultLayout();

                MainWindowVM.Instance.CurrentView = HomeVM.Instance;
                //HomeVM.Instance.InitializeTimer();
            });
        }

        private void TimerCallback(object state)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                DoneCommand.Execute(null);
            });
        }

        public void SaveInsuranceTransaction()
        {
            InsuranceTransactionModel ins = new InsuranceTransactionModel();
            ins.memberId = UserModel.Instance.UserID;
            ins.packageDetailId = UserModel.Instance.InsurancePackageName.id;
            ins.registrationDate = DateTime.Now;
            ins.expireDate = DateTime.Now.AddYears(1);
            ins.transactionDate = DateTime.Now;
            ins.annualPay = UserModel.Instance.InsurancePackageName.fee;
            ins.status = 1;
            ins.paymentMethod = UserModel.Instance.PaymentMethod;
            UserModel.Instance.InsuranceTransaction = ins;

            //UserModel.Instance.InsuranceTransaction = new InsuranceTransactionModel();
        }

        public static String to;
        private async Task SendEmail()
        {
            await Task.Run(() =>
            {
                String from, pass, messageBody;
                MailMessage message = new MailMessage();
                to = UserModel.Instance.Email;
                from = "davidle2804@gmail.com";
                pass = "hvsj ywws lpwd acmz";
                messageBody =
                    "Cảm ơn bạn đã sử dụng dịch vụ mua bảo hiểm. Dưới đây là thông tin về giao dịch của bạn." +
                    "\nNhà cung cấp: " + UserModel.Instance.InsurancePackageName.provider +
                    "\nTên gói: " + UserModel.Instance.InsurancePackageName.packageName +
                    "\nGiá: " + UserModel.Instance.InsurancePackageName.fee.ToString() + "VND" +
                    "\nNgày mua: " + UserModel.Instance.InsuranceTransaction.transactionDate +
                    "\n Ngày hết hạn: " + UserModel.Instance.InsuranceTransaction.expireDate;
                message.To.Add(to);
                message.From = new MailAddress(from);
                message.Body = messageBody;
                message.Subject = "Xác nhận mua bảo hiểm";
                SmtpClient smtp = new SmtpClient("smtp.gmail.com");
                smtp.EnableSsl = true;
                smtp.Port = 587;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Credentials = new NetworkCredential(from, pass);

                try
                {
                    smtp.Send(message);
                    //MessageBox.Show("Check your Email");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            });
        }


        public void SaveSavingTransaction()
        {
            SavingTransactionModel saving = new SavingTransactionModel();
            saving.status = 1;
            UserModel.Instance.SavingTransaction = saving;
        }
        //private void SendMailToProvider() 
        //{

        //}
    }
}
