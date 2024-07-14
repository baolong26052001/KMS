using Insurance.Model;
using Insurance.ViewModel;
using System.Windows.Controls;

namespace Insurance.View
{
    /// <summary>
    /// Interaction logic for HomeScreenWindow.xaml
    /// </summary>
    public partial class HomeView : UserControl
    {

        public HomeView()
        {
            InitializeComponent();
            // Access the singleton instance
            var homeVMInstance = HomeVM.Instance;
            DataContext = homeVMInstance;
           if(!UserModel.Instance.IdActiveCountDown)
            {
                MainWindowVM.Instance.ActiveCountDown();
            }    

        }

    }
}
