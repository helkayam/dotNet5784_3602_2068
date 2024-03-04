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
using PL.Task;
using PL.Worker;

namespace PL
{
    /// <summary>
    /// Interaction logic for WorkerMainWindow.xaml
    /// </summary>
    public partial class WorkerMainWindow : Window
    {

        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();



        public int Id
        {
            get { return (int)GetValue(IdProperty); }
            set { SetValue(IdProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Id.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IdProperty =
            DependencyProperty.Register("Id", typeof(int), typeof(int), new PropertyMetadata(0));


        public BO.Worker MyWorker
        {
            get { return (BO.Worker)GetValue(MyWorkerProperty); }
            set { SetValue(MyWorkerProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyWorker.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MyWorkerProperty =
            DependencyProperty.Register("MyWorker", typeof(BO.Worker), typeof(WorkerWindow), new PropertyMetadata());


        public string Hello
        {
            get { return (string)GetValue(MyStringHelloProperty); }
            set { SetValue(MyStringHelloProperty, value); }
        }

        public static readonly DependencyProperty MyStringHelloProperty =
            DependencyProperty.Register("MyStringHelloProperty", typeof(BO.Worker), typeof(string), new PropertyMetadata());



        public WorkerMainWindow(int IdOfWorker)
        {

            InitializeComponent();
            Hello = "Hello" + s_bl.Worker.ReadWorker(IdOfWorker).Name;
            Id= IdOfWorker;

        }

        private void ButtonChoseTask_Click(object sender, RoutedEventArgs e)
        {
            new TasksForWorkerList(Id);
        }

        private void ButtonYourTask_Click(object sender, RoutedEventArgs e)
        {
            new TaskOfWorker(s_bl.Worker.ReadWorker(Id).Task.Id).ShowDialog();  
        }
    }
}
