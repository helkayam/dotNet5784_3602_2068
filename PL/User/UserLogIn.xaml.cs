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
using System.Windows.Shapes;

namespace PL
{
    /// <summary>
    /// Interaction logic for UserLogIn.xaml
    /// </summary>
    public partial class UserLogIn : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        public UserLogIn()
        {
            InitializeComponent();
        }

        public BO.User MyUser
        {
            get { return (BO.User)GetValue(MyUserProperty); }
            set { SetValue(MyUserProperty, value); }
        }
        public static readonly DependencyProperty MyUserProperty =
         DependencyProperty.Register("MyUser", typeof(BO.User), typeof(MainWindow), new PropertyMetadata());
        private void Button_AddNewUser_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                if (s_bl.User.ReadUser(MyUser.userName) == null)
                {
                    s_bl.User.AddUser(MyUser);
                    MessageBox.Show($"Adding the user with UserName: {MyUser.userName} card was successful");

                }
                else
                {
                    s_bl.Worker.UpdateWorker(MyUser);
                    MessageBox.Show($"Updating the user with UserName: {MyUser.userName} card was successful");
                }
            }
            catch (BO.BlInvalidGivenValueException ex)
            {
                MessageBox.Show(ex.Message);


            }
            catch (BO.BlDoesNotExistException ex)
            {
                MessageBox.Show(ex.Message);

            }






            this.Close();
        }

    }
}

