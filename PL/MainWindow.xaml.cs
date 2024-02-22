using PL.User;
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

namespace PL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();



        public DateTime Clock
        {
            get { return (DateTime)GetValue(ClockProperty); }
            set { SetValue(ClockProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Clock.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ClockProperty =
            DependencyProperty.Register("Clock", typeof(DateTime), typeof(MainWindow), new PropertyMetadata(0));




        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        public MainWindow(int IdOfWorker = 0)
        {
            
                InitializeComponent();

                Clock = DateTime.Now;
            
            //    if (IdOfWorker == 0)
            //    {
            //        MyWorker = new BO.Worker { };
            //    }
            //    else
            //        MyWorker = s_bl.Worker.ReadWorker(IdOfWorker);
            //}
            //catch (BO.BlDoesNotExistException ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}


        }

        private void AddOneDay_click(object sender, RoutedEventArgs e)
        {
             s_bl.IncreasInDay();
            Clock = s_bl.Clock;
        }

        private void AddOneHour_click(object sender, RoutedEventArgs e)
        {
            s_bl.IncreasInHour();
            Clock = s_bl.Clock;
        }

        private void AddWeek_click(object sender, RoutedEventArgs e)
        {
            s_bl.IncreasInWeek();
            Clock = s_bl.Clock;
        }

        private void InitClock_click(object sender, RoutedEventArgs e)
        {
            s_bl.InitClock();
            Clock = s_bl.Clock;
        }

        private void SignIn_Click(object sender, RoutedEventArgs e)
        {
            new SignIn().ShowDialog();

        }

        private void LogIn_Click(object sender, RoutedEventArgs e)
        {
            new UserLogIn().ShowDialog();

        }
    }
}
