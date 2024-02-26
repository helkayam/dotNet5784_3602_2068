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
using System;
using System.Net;
using System.Net.Mail;
using PL.Admin;
namespace PL.User
{
    /// <summary>
    /// Interaction logic for Two_Step_Verification.xaml
    /// </summary>
    /// 


  
    public partial class Two_Step_Verification : Window
    {

        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();



        public int CodeFromEmail
        {
            get { return (int)GetValue(CodeFromEmailProperty); }
            set { SetValue(CodeFromEmailProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CodeFromEmail.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CodeFromEmailProperty =
            DependencyProperty.Register("CodeFromEmail", typeof(int), typeof(Two_Step_Verification), new PropertyMetadata(0));



        private int CodeFromSignOrLogIn = 0;

        BO.User myUser=new BO.User();

        public Two_Step_Verification(int Code,BO.User MyUser)
        {
            InitializeComponent();
            myUser = MyUser;
            CodeFromEmail = 0;
            CodeFromSignOrLogIn = Code;
        }

        private void CheckAndOpenUserWindow_click(object sender, RoutedEventArgs e)
        {
            try
            {

                if (CodeFromEmail == CodeFromSignOrLogIn)
                {

                    if (s_bl.User.ReadUser(myUser.UserName) == null)
                    {

                        s_bl.User.AddUser(myUser);
                        MessageBox.Show($"Adding the user with user name: {myUser.UserName} card was successful");

                    }
                    this.Close();

                    if (myUser.IsAdmin)
                    {
                        new AdminWindow().ShowDialog();
                    }
                    else
                        new WorkerMainWindow(myUser.Id).ShowDialog();
                }
                else
                {
                    MessageBox.Show("The code entered is incorrect");

                }
            }
            catch (BO.BlInvalidGivenValueException ex)
            {
                MessageBox.Show(ex.Message);

            }
            catch (BO.BlAlreadyExistsException ex)
            {
                throw new BO.BlAlreadyExistsException($"User with UserName={myUser.UserName} already exists", ex);
            }







        }


        static int SendEmail(string sendTo)
        {
            int randCode = 0;
            Random rand = new Random();
            randCode= rand.Next(1000, 10000);
            // פרטי ההתחברות לשרת הדואר האלקטרוני
            string smtpServer = "smtp.gmail.com";
            int port = 587; // יתכן וצריך לשנות את הערך לפורט המתאים
            string email = "henelkayam99@gmail.com";
            string password = "helkayam214243602";

            // פרטי האימייל שתשלחי
            string recipientEmail =sendTo;
            string subject = @"Your verification code is:
                               *The code is valid for 30 seconds";
            string body = randCode.ToString(); 
            // יצירת האימייל
            MailMessage mail = new MailMessage(email, recipientEmail, subject, body);

            // הגדרת פרטי התחברות לשרת הדואר האלקטרוני
            SmtpClient client = new SmtpClient(smtpServer);
            client.Port = port;
            client.Credentials = new NetworkCredential(email, password);
            client.EnableSsl = true;

            // שליחת האימייל
            try
            {
                client.Send(mail);
            }
            catch (Exception ex)
            {
                MessageBox.Show("There was an error sending the email " + ex.Message);
            }
            return randCode;
        }

        

        private void BackToSignOrLogW_click(object sender, RoutedEventArgs e)
        {
            this.Close();
            if (s_bl.User.ReadUser(myUser.UserName) == null)
               
                new SignIn().ShowDialog();
            else
                new UserLogIn().ShowDialog();

        }

        

        private void SendEmail_click(object sender, RoutedEventArgs e)
        {
            CodeFromSignOrLogIn= SendEmail(myUser.Email);

        }
    }
    

        
    
}
