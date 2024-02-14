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

namespace PL.Worker
{
    /// <summary>
    /// Interaction logic for WorkerListWindow.xaml
    /// </summary>
    public partial class WorkerListWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        public WorkerListWindow()
        {
            InitializeComponent();
            WorkerList = s_bl?.Worker.ReadAllWorkers();

        }

        public IEnumerable<BO.Worker> WorkerList
        {
            get { return (IEnumerable<BO.Worker>)GetValue(WorkerListProperty); }
            set { SetValue(WorkerListProperty, value); }
        }

        public static readonly DependencyProperty WorkerListProperty = DependencyProperty.Register("WorkerList", typeof(IEnumerable<BO.Worker>),typeof(WorkerListWindow), new PropertyMetadata(null));

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }



        //private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{

        //}
    }
}
