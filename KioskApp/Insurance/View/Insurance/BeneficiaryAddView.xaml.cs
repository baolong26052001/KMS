using Insurance.ViewModel;
using System.Windows.Controls;

namespace Insurance.View
{
    /// <summary>
    /// Interaction logic for BeneficiaryAddView.xaml
    /// </summary>
    public partial class BeneficiaryAddView : UserControl
    {
        public BeneficiaryAddView()
        {
            InitializeComponent();
            DataContext = (BeneficiaryAddVM)MainWindowVM.Instance.CurrentView;
        }
    }
}
