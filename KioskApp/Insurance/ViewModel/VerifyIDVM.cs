using ID_Scanner;
using Insurance.Command;

using Insurance.Service;
using Insurance.Utility;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Insurance.ViewModel
{
    public class VerifyIDVM : ViewModelBase
    {
        private Scanner _scanner;
        private string img_fr = "..\\Images\\IDCardModel.png";
        private Visibility _visibilityNextbtn = Visibility.Collapsed;
        public Visibility VisibilityNextbtn
        {
            get { return _visibilityNextbtn; }
            set
            {
                if (_visibilityNextbtn != value)
                {
                    _visibilityNextbtn = value;
                    OnPropertyChanged(nameof(VisibilityNextbtn));
                }
            }
        }
        //private API_service _apiService;
        CancellationTokenSource source = new CancellationTokenSource();
        CancellationToken token;

        public string _IMG_FR { get => this.img_fr; set { this.img_fr = value; OnPropertyChanged(); } }
        private Task scanTask;
        public ICommand NextCommand { get; set; }
        public string imageUri = "";
        public VerifyIDVM()
        {
            _scanner = Scanner.Instance;
            _scanner.onCapture += Scanner_onCapture;
            
            NextCommand = new RelayCommand(OnNextCommand);
            token = source.Token;
            scanTask = Task.Run(() => {
                if (token.IsCancellationRequested)
                    throw new TaskCanceledException(scanTask);
                _scanner.AutoCapture();
            }, token);
            MainWindowVM.Instance.VisibilityBtnHead = System.Windows.Visibility.Visible;

        }

        private void _scanner_OnFinish(object? sender, EventArgs e)
        {
            source.Cancel();
            var check1 = token.IsCancellationRequested;
            var check = scanTask.Status;
            Scan_Services _scanServices = new Scan_Services(imageUri);

        }

        private void OnNextCommand(object obj)
        {
            _scanner.onCapture -= Scanner_onCapture;
            _scanner.OnFinish -= _scanner_OnFinish;
            _scanner = null;
            MainWindowVM.Instance.CurrentView = new VerifyIDBackVM();

        }
        #region not use
        //private void Scanner_onCapture(object? sender, EventArgs e)
        //{
        //    //new API_service().ProcessScannedImageAsync(@"D:\Loan-Kiosk-full\Loan-Kiosk\KioskApp\Insurance\bin\Debug\net6.0-windows\CCCD\PIC_2986781_00000.jpg").Wait();

        //    imageUri = (sender as string[])[0].ToString();
        //    _IMG_FR = imageUri;

        //    _scanner.IsFinish = true; // bỏ try catch back về welcome ( scanner was null )
        //}
        #endregion
        private void Scanner_onCapture(object? sender, EventArgs e)
        {
            try
            {
                string[] senderArray = sender as string[];
                if (senderArray != null && senderArray.Length > 0)
                {
                    imageUri = senderArray[0];
                    _IMG_FR = imageUri;
                }
                _scanner.OnFinish += _scanner_OnFinish;
                if (_scanner != null)
                {
                    _scanner.IsFinish = true;
                    VisibilityNextbtn = Visibility.Visible;
                }
                else
                {
                    // Handle the exception: back to Welcome Screen
                    Console.WriteLine("Error: _scanner is null.");
                    APIHelper.Instance.Log("Error: _scanner is null");
                    MainWindowVM.Instance.IsBacktoWelcomeVisible = true;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                APIHelper.Instance.Log($"An error occurred: {ex.Message}");
                MainWindowVM.Instance.IsBacktoWelcomeVisible = true;
                // Handle the exception: back to Welcome Screen
            }
        }

    }
}
