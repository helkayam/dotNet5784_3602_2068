using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using PL.Worker;

namespace PL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ButtonWorker_Click(object sender, RoutedEventArgs e)
        {
            new WorkerListWindow().Show();
        }
        private void ButtonINIT_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("do you want To initialize data Base?","hello", MessageBoxButton.YesNo, MessageBoxImage.Question) ;
            switch(messageBoxResult)
            {
                case MessageBoxResult.Yes: DalTest.Initialization.Do();break;
                case MessageBoxResult.No:break;
            }
            
        }


    }
}