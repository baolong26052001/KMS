using Insurance.ViewModel;
using System.Windows.Controls;

namespace Insurance.View
{
    /// <summary>
    /// Interaction logic for PaymentMethodView.xaml
    /// </summary>
    public partial class PaymentMethodView : UserControl
    {
        public PaymentMethodView()
        {
            InitializeComponent();
            DataContext = (PaymentMethodVM)MainWindowVM.Instance.CurrentView;
        }
    }
}
