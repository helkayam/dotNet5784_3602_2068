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


       
        public bool IsThirdStageAndWithoutTask
        {
            get { return (bool)GetValue(IsThirdStageAndWithoutTaskProperty); }
            set { SetValue(IsThirdStageAndWithoutTaskProperty, value); }
        }

        // Using a DependencyProperty as the backing store for isThirdStage  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsThirdStageAndWithoutTaskProperty =
            DependencyProperty.Register("IsThirdStageAndWithoutTask", typeof(bool), typeof(WorkerMainWindow), new PropertyMetadata());

       

        public bool HaveTask
        {
            get { return (bool)GetValue(HaveTaskProperty); }
            set { SetValue(HaveTaskProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HaveTask.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HaveTaskProperty =
            DependencyProperty.Register("HaveTask", typeof(bool), typeof(WorkerMainWindow), new PropertyMetadata());



        public int Id
        {
            get { return (int)GetValue(IdProperty); }
            set { SetValue(IdProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Id.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IdProperty =
            DependencyProperty.Register("Id", typeof(int), typeof(WorkerMainWindow), new PropertyMetadata());



        public IEnumerable<BO.TaskInWorker> MyTask
        {
            get { return (IEnumerable<BO.TaskInWorker>)GetValue(MyTaskProperty); }
            set { SetValue(MyTaskProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyTask.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MyTaskProperty =
            DependencyProperty.Register("MyTask", typeof(IEnumerable<BO.TaskInWorker >), typeof(WorkerMainWindow), new PropertyMetadata());


        public BO.Worker MyWorker
        {
            get { return (BO.Worker)GetValue(MyWorkerProperty); }
            set { SetValue(MyWorkerProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyWorker.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MyWorkerProperty =
            DependencyProperty.Register("MyWorker", typeof(BO.Worker), typeof(WorkerMainWindow), new PropertyMetadata());


        public string Hello
        {
            get { return (string)GetValue(MyStringHelloProperty); }
            set { SetValue(MyStringHelloProperty, value); }
        }

        public static readonly DependencyProperty MyStringHelloProperty =
            DependencyProperty.Register("Hello", typeof(string), typeof(WorkerMainWindow), new PropertyMetadata());



        public WorkerMainWindow(int IdOfWorker)
        {
          

           
               
           
            Hello = "Hello " + s_bl.Worker.ReadWorker(IdOfWorker).Name;
            Id= IdOfWorker;
            MyTask = s_bl.Task.ReadAllWorkerTask(Id);
            if (MyTask.Count()==0)
                HaveTask = false;
            else
                HaveTask = true;

            if (s_bl.Task.GetStatusOfProject() == BO.ProjectStatus.ExecutionStage && HaveTask == false)
                IsThirdStageAndWithoutTask = true;
                InitializeComponent();

        }

        private void ButtonChoseTask_Click(object sender, RoutedEventArgs e)
        {

            new TasksForWorkerList(Id).ShowDialog();
        
            MyTask = s_bl.Task.ReadAllWorkerTask(Id);
            if (MyTask.Count() != 0)
                HaveTask = true;
            else HaveTask = false;
            if (s_bl.Task.GetStatusOfProject() == BO.ProjectStatus.ExecutionStage && HaveTask == false)
                IsThirdStageAndWithoutTask = true;
            else
                IsThirdStageAndWithoutTask = false;
            InitializeComponent();
        }


        private void Update_click(object sender, MouseButtonEventArgs e)
        {
            BO.TaskInWorker tiw = MyTask.FirstOrDefault();
            new TaskOfWorker(tiw.Id).ShowDialog();

            MyTask = s_bl.Task.ReadAllWorkerTask(Id);
            if (MyTask.Count() != 0)
                HaveTask = true;
            else HaveTask = false;
            if (s_bl.Task.GetStatusOfProject() == BO.ProjectStatus.ExecutionStage && HaveTask == false)
                IsThirdStageAndWithoutTask = true;
            else
                IsThirdStageAndWithoutTask = false;
            InitializeComponent();


        }


    }
}
