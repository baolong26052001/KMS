using Insurance.ViewModel;
using System.Windows;
using System.Windows.Input;

namespace Insurance
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = MainWindowVM.Instance;
            this.KeyDown += MainWindow_KeyDown;
        }

        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Q:// New, ko luu data
                    MainWindowVM.Instance.CurrentView = new CashDepositVM();
                    break;
                case Key.T:// New, ko luu data
                    MainWindowVM.Instance.CurrentView = new VirtualNumpadVM();
                    break;
                case Key.V:// New, ko luu data
                    MainWindowVM.Instance.CurrentView = new VirtualKeyboardVM();
                    break;
                case Key.R:// luu lai data
                    MainWindowVM.Instance.GoBeneficiary();
                    break;
                case Key.A:// luu lai data
                    MainWindowVM.Instance.GoCashDeposit();
                    break;
                default: break;
            }
        }
    }
}
