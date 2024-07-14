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
    /// Interaction logic for AnGiaPackView.xaml
    /// </summary>
    public partial class AnGiaPackView : UserControl
    {
        public AnGiaPackView()
        {
            InitializeComponent();
            DataContext = (AnGiaPackVM)MainWindowVM.Instance.CurrentView;
        }
    }
}
