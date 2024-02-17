using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

using System.Windows.Media.Imaging;
using static System.Net.Mime.MediaTypeNames;

using System.Windows.Navigation;
using System.Windows.Shapes;
using PL.Worker;
using PL.Task;
using System.Media;
using Microsoft.Win32;
namespace PL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
     


        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        private MediaPlayer mediaPlayer = new MediaPlayer();

        public MainWindow()
        {
            InitializeComponent();
           
            mediaPlayer.Open(new Uri(@"MediaFile\Israel National Anthem (Instrumental).mp3", UriKind.RelativeOrAbsolute));
        }

        private void ButtonWorker_Click(object sender, RoutedEventArgs e)
        {
            new WorkerListWindow().Show();
        }

        private void ButtonTask_Click(object sender, RoutedEventArgs e)
        {
            new TaskListWindow().Show();
        }
        private void ButtonINIT_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("do you want to initialize data Base?","hello", MessageBoxButton.YesNo, MessageBoxImage.Question) ;
            switch(messageBoxResult)
            {
                case MessageBoxResult.Yes: s_bl.InitializeDB();break;
                case MessageBoxResult.No:break;
            }
            
        }

        private void ButtonReset_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("do you want to reset data Base?", "hello", MessageBoxButton.YesNo, MessageBoxImage.Question);
            switch (messageBoxResult)
            {
                case MessageBoxResult.Yes: s_bl.ResetDB(); break;
                case MessageBoxResult.No: break;
            }
        }

      

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            mediaPlayer.Play();
        }

        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            mediaPlayer.Pause ();
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            mediaPlayer.Stop();

        }
    }
}