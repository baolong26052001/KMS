using Insurance.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;


namespace Insurance.View
{
    /// <summary>
    /// Interaction logic for VirtualNumpadView.xaml
    /// </summary>
    public partial class VirtualNumpadView : UserControl
    {
        public VirtualNumpadView()
        {
            InitializeComponent();
            DataContext = (VirtualNumpadVM)MainWindowVM.Instance.CurrentView;
        }
    }
}
