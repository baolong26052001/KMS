using Insurance.Model;
using Insurance.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Insurance.View
{
    /// <summary>
    /// Interaction logic for WelcomeView.xaml
    /// </summary>
    public partial class WelcomeView : UserControl
    {
        public WelcomeView()
        {
            InitializeComponent();
            DataContext = (WelcomeVM)MainWindowVM.Instance.CurrentView;
            MainWindowVM.Instance.IsBacktoWelcomeVisible = false;
            MainWindowVM.Instance.IsAutoLogoutPopupVisible = false;
            UserModel.Instance.IsWelcomeView = true;
            MainWindowVM.Instance.DeactiveCountDown();
        }
    }
}
