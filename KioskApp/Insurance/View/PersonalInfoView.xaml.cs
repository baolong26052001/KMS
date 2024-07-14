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
    /// Interaction logic for PersonalInfoView.xaml
    /// </summary>
    public partial class PersonalInfoView : UserControl
    {
        public PersonalInfoView()
        {
            InitializeComponent();
            //var PersonalInforVM = new PersonalInfoVM();
            //DataContext = PersonalInforVM;
            DataContext = (PersonalInfoVM)MainWindowVM.Instance.CurrentView;
        }
        //private void UploadFieldsButtonClick(object sender, RoutedEventArgs e)
        //{
        //    PersonalInfoVM viewModel = (PersonalInfoVM)this.DataContext;
        //    string email = Email.Text; 
        //    string phoneNumber = Phone_num.Text; 

        //    viewModel.UploadOcrFieldsToApi(UserModel.Instance.UserID, email, phoneNumber);
        //}

    }
}
