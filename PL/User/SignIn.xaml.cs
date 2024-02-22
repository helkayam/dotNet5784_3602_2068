using PL.Worker;
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

namespace PL.User
{
    /// <summary>
    /// Interaction logic for SignIn.xaml
    /// </summary>
    public partial class SignIn : Window
    {


        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        public BO.User MyUser
        {
            get { return (BO.User)GetValue(MyUserProperty); }
            set { SetValue(MyUserProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyUser.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MyUserProperty =
            DependencyProperty.Register("MyUser", typeof(BO.User), typeof(SignIn), new PropertyMetadata(0));


        public SignIn()
        {
            InitializeComponent();
            MyUser= new BO.User();
        }

        

        private void AddNewUser_click(object sender, RoutedEventArgs e)
        {
            try
            {

                s_bl.Worker.ReadWorker(MyUser.Id, true);
                s_bl.User.AddUser(MyUser);
                MessageBox.Show($"Adding the user with user name: {MyUser.Id} card was successful");

            }
            catch (BO.BlAlreadyExistsException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (BO.BlDoesNotExistException ex)
            {
                MessageBox.Show($"No worker was found with the same ID:{MyUser.Id} as the user with this username;{MyUser.UserName}" );
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
