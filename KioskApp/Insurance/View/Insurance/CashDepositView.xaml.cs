using Insurance.ViewModel;
using System.Windows.Controls;

namespace Insurance.View
{
    /// <summary>
    /// Interaction logic for CashDepositView.xaml
    /// </summary>
    public partial class CashDepositView : UserControl
    {
        public CashDepositView()
        {
            InitializeComponent();
           // CashDepositVM vm = new CashDepositVM();
            DataContext =(CashDepositVM) MainWindowVM.Instance.CurrentView;
        }
    }
}
