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
    public partial class ListWithdrawView : UserControl
    {
        public ListWithdrawView()
        {
            InitializeComponent();
            DataContext = (ListWithdrawVM)MainWindowVM.Instance.CurrentView;
        }
        private void ItemsControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender is ItemsControl itemsControl && itemsControl.Items.Count > 0)
            {
                var firstItem = itemsControl.ItemContainerGenerator.ContainerFromIndex(0) as FrameworkElement;
                firstItem?.Focus();

                if (firstItem != null && itemsControl.DataContext is ListWithdrawVM viewModel)
                {
                    if (firstItem.DataContext is SavingTransactionModel firstItemData)
                    {
                        viewModel.FetchSavingDetail(UserModel.Instance.UserID, 0, firstItemData.savingId);
                        viewModel.FetchPersonInfo(UserModel.Instance.UserID);
                    }
                    firstItem.Focusable = true;
                    firstItem.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                }
            }
        }
    }
}
