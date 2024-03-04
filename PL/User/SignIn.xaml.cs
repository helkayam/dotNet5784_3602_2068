using PL.Worker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
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
using static System.Runtime.InteropServices.JavaScript.JSType;

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
            DependencyProperty.Register("MyUser", typeof(BO.User), typeof(SignIn), new PropertyMetadata());


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
               
                if (s_bl.User.checkExistId(MyUser.Id) == true)
                    MessageBox.Show($"user with Id:{MyUser.Id} already exist");
                else
                {
                  
                    try
                    {
                        int code=s_bl.User.SendEmail(MyUser.Email);
                        new Two_Step_Verification(code,MyUser);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("There was an error sending the email " + ex.Message);
                        new SignIn().ShowDialog();
                    }
                    
                }

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

        
    }
    
}

