using PL.Admin;
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

        private MediaPlayer mediaPlayer = new MediaPlayer();



        public DateTime?  StartDateProject
        {
            get { return  s_bl.Schedule.getStartDateProject(); }
            set { SetValue(StartDateProjectProperty, value); }
        }

        // Using a DependencyProperty as the backing store for StartDateProject.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StartDateProjectProperty =
            DependencyProperty.Register("StartDateProject", typeof(DateTime?), typeof(MainWindow ), new PropertyMetadata(null));

      

        public DateTime Clock
        {
            get { return s_bl.Clock; }
            set { SetValue(ClockProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Clock.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ClockProperty =
            DependencyProperty.Register("Clock", typeof(DateTime), typeof(MainWindow), new PropertyMetadata(s_bl.Clock));




        

        public MainWindow()
        {
            mediaPlayer.Open(new Uri(@"MediaFile\Israel National Anthem (Instrumental).mp3", UriKind.RelativeOrAbsolute)); InitializeComponent();
            //s_bl.InitClock() ;
            Clock = s_bl.Clock;
            StartDateProject = s_bl.Schedule.getStartDateProject(); 

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
            new SignInWindow().ShowDialog();

        }

        private void AdminEntrance_Click(object sender, RoutedEventArgs e)
        {
            new AdminWindow().ShowDialog();
           
            Clock = s_bl.GetDate();
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            mediaPlayer.Play();
        }

        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            mediaPlayer.Pause();
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            mediaPlayer.Stop();

        }
    }
}
