using Insurance.Model;
using Insurance.ViewModel;
using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Insurance.View
{
    /// <summary>
    /// Interaction logic for VerifyIDView.xaml
    /// </summary>
    /// 
    public partial class VerifyIDView : UserControl
    {


        private API_service _apiService;
        //private scanner _scan;

        public VerifyIDView()
        {
            InitializeComponent();
            //_apiService = new API_service("");
            DataContext = (VerifyIDVM)MainWindowVM.Instance.CurrentView;
            MainWindowVM.Instance.ActiveCountDown();
            //_apiService.OnProcessComplete += ApiService_OnProcessComplete;            

        }



        private void ApiService_OnProcessComplete()
        {
            // Update UI elements in the dispatcher thread
            Dispatcher.Invoke(() =>
            {
                SuccessNoti.Visibility = Visibility.Visible;
            });
        }
    }
}
