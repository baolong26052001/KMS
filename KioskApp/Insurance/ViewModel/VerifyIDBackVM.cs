using ID_Scanner;
using Insurance.Command;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Insurance.ViewModel
{
    public class VerifyIDBackVM : ViewModelBase
    {
        private Scanner _scanner;

        private string img_bk = "..\\Images\\IDCardModel_Back.png";
        private Visibility _visibilityNextbtnBk = Visibility.Collapsed;
        public Visibility VisibilityNextbtnBk
        {
            get { return _visibilityNextbtnBk; }
            set
            {
                if (_visibilityNextbtnBk != value)
                {
                    _visibilityNextbtnBk = value;
                    OnPropertyChanged(nameof(VisibilityNextbtnBk));
                }
            }
        }
        CancellationTokenSource source = new CancellationTokenSource();
        CancellationToken token;
        private Task scanTask;
        public string imageUri = "";
        public ICommand NextButtonClickCommand => new RelayCommand(NextButtonClick);
        public ICommand BackButoonClickCommand => new RelayCommand(BackButtonClick);
        public string _IMG_BK
        {
            get => this.img_bk;
            set { this.img_bk = value; OnPropertyChanged(); }
        }

        public VerifyIDBackVM()
        {
            INIT();
            MainWindowVM.Instance.VisibilityBtnHead = System.Windows.Visibility.Visible;
        }
        public void INIT()
        {
            _scanner = Scanner.Instance;
            _scanner.onCapture += Scanner_onCapture;
            scanTask = Task.Run(() => { _scanner.AutoCapture(); }, source.Token);
        }
        


        private void Scanner_onCapture(object? sender, EventArgs e)
        {
            imageUri = (sender as string[])[0].ToString();
            _IMG_BK = imageUri;
            source.Cancel();
            _scanner.IsFinish = true;
            VisibilityNextbtnBk = Visibility.Visible;
        }

        private void NextButtonClick(object parameter)
        {
         
            //var view = new VerifyFaceIDVM(1);
            MainWindowVM.Instance.CurrentView = new VerifyFaceIDVM(1);
        }
        private void BackButtonClick (object parameter)
        {
            _IMG_BK = "..\\Images\\IDCardModel_Back.png";
            MainWindowVM.Instance.CurrentView = new VerifyIDBackVM();
        }
    }
}
