using Insurance.Command;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Insurance.ViewModel
{
    class InsuranceSuccessVM : ViewModelBase
    {
        public ICommand DoneCommand { get; set; }

        public InsuranceSuccessVM()
        {
            MainWindowVM.Instance.VisibilityHeader = Visibility.Collapsed;

            Timer timer = new Timer(TimerCallback, null, TimeSpan.FromSeconds(10), TimeSpan.FromMilliseconds(-1)); // Set Timer Count down 10 second.

            DoneCommand = new RelayCommand((parameter) =>
            {
                MainWindowVM.Instance.CurrentView = new HomeVM();
                MainWindowVM.Instance.setDefaultLayout();
            });
        }

        private void TimerCallback(object state)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                DoneCommand.Execute(null);
            });
        }
    }
}
