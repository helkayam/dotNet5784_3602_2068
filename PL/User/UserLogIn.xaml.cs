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
using PL.Admin;
using PL.User;
using PL.Worker;

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

        //private void KeyDownUserSignIn(object sender, TextChangedEventArgs e)
        //{
        //    try
        //    {
        //        BO.User user;
        //        if (s_bl.User.ReadUser(MyUser.UserName) == null)
        //            s_bl.User.ReadUser(MyUser.UserName, true);
        //        else
        //        {
        //            user = s_bl.User.ReadUser(MyUser.UserName);
        //            if (user.Password == MyUser.Password)
        //            {
        //                if (user.IsAdmin == true)
        //                    new AdminWindow().ShowDialog();
        //                else
        //                    new WorkerMainWindow(s_bl.User.ReadUser(MyUser.UserName).Id);
        //            }

        //        }
        //    }
        //    catch(BO.BlDoesNotExistException ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }

        //}

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ChackEmail_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                s_bl.Worker.ReadWorker(MyUser.Id, true);


                if (s_bl.User.ReadUser(MyUser.UserName, true).Id == MyUser.Id) ;
                {
                    int code = s_bl.User.SendEmail(MyUser.Email);
                    new Two_Step_Verification(code, MyUser);
                    // שליחת האימייל

                    new Two_Step_Verification(code, MyUser);
                }
            }

            catch (BO.BlDoesNotExistException ex)
            {
                MessageBox.Show($"No worker was found with the same ID:{MyUser.Id} as the user with this username;{MyUser.UserName}");
            }
            catch (Exception ex)
            {
                MessageBox.Show("There was an error sending the email " + ex.Message);
                new SignIn().ShowDialog();
            }
           

        }
    }


        //private void Button_AddNewUser_Click(object sender, RoutedEventArgs e)
        //{

        //    try
        //    {
        //        if (s_bl.User.ReadUser(MyUser.UserName) == null)
        //        {
        //            s_bl.User.AddUser(MyUser);
        //            MessageBox.Show($"Adding the user with UserName: {MyUser.UserName} card was successful");

        //        }
        //        else
        //        {
        //            s_bl.User.UpdateUser(MyUser );
        //            MessageBox.Show($"Updating the user with UserName: {MyUser.UserName} card was successful");
        //        }
        //    }
        //    catch (BO.BlInvalidGivenValueException ex)
        //    {
        //        MessageBox.Show(ex.Message);


        //    }
        //    catch (BO.BlDoesNotExistException ex)
        //    {
        //        MessageBox.Show(ex.Message);

        //    }






        //    this.Close();
        //}

    }


