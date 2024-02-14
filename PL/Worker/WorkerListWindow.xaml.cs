using System;
using System.Collections.Generic;
using System.Globalization;
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
        public BO.FilterWorker filterWorkers { get; set; } = BO.FilterWorker.None;



        public bool Bylevel
        {
            get { return (bool)GetValue(bylevelProperty); }
            set { SetValue(bylevelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for bylevel.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty bylevelProperty =
            DependencyProperty.Register("Bylevel", typeof(bool), typeof(WorkerListWindow), new PropertyMetadata(null));



        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        public WorkerListWindow()
        {
            Bylevel = false;
            InitializeComponent();
            WorkerList = s_bl?.Worker.ReadAllWorkers();
            
        }

        public IEnumerable<BO.Worker> WorkerList
        {
            get { return (IEnumerable<BO.Worker>)GetValue(WorkerListProperty); }
            set { SetValue(WorkerListProperty, value); }
        }

        public static readonly DependencyProperty WorkerListProperty = DependencyProperty.Register("WorkerList", typeof(IEnumerable<BO.Worker>), typeof(WorkerListWindow), new PropertyMetadata(null));



        private void ComboBox_FilterWorkerChanged(object sender, SelectionChangedEventArgs e)
        {
            if (filterWorkers == BO.FilterWorker.ByLevel)
            {
                Bylevel = true;
            }
            else
            {
                WorkerList = s_bl.Worker.ReadAllWorkers(filterWorkers);
                Bylevel = false;
            }


        }

       

        private void ListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            int id = ((BO.Worker)((ListView)sender).SelectedItem).Id;
            new WorkerWindow(id).ShowDialog();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            WorkerList = s_bl.Worker.ReadAllWorkers(filterWorkers,(BO.WorkerExperience)((ComboBox)sender).SelectedItem);
        }

        private void Button_AddClick(object sender, RoutedEventArgs e)
        {
            new WorkerWindow().ShowDialog();

        }
    }
}
