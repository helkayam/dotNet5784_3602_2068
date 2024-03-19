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


        public bool IsFirstStage
        {
            get { return (bool)GetValue(isFirstStageProperty); }
            set { SetValue(isFirstStageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for isThirdStage  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty isFirstStageProperty =
            DependencyProperty.Register("isFirstStage", typeof(bool), typeof(WorkerMainWindow), new PropertyMetadata(null));


        public bool IsThirdStage
        {
            get { return (bool)GetValue(isThirdStageProperty); }
            set { SetValue(isThirdStageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for isThirdStage  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty isThirdStageProperty =
            DependencyProperty.Register("isThirdStage", typeof(bool), typeof(WorkerMainWindow), new PropertyMetadata(null));

        public bool IsSecondStage
        {
            get { return (bool)GetValue(isSecondStageProperty); }
            set { SetValue(isSecondStageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for isThirdStage  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty isSecondStageProperty =
            DependencyProperty.Register("isSecondStage", typeof(bool), typeof(WorkerMainWindow), new PropertyMetadata(null));



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



        public BO.TaskInWorker MyTask
        {
            get { return (BO.TaskInWorker)GetValue(MyTaskProperty); }
            set { SetValue(MyTaskProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyTask.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MyTaskProperty =
            DependencyProperty.Register("MyTask", typeof(BO.TaskInWorker), typeof(WorkerMainWindow), new PropertyMetadata());


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
            DependencyProperty.Register("Hello", typeof(string), typeof(WorkerMainWindow), new PropertyMetadata());



        public WorkerMainWindow(int IdOfWorker)
        {
            InitializeComponent();

            if (s_bl.Task.GetStatusOfProject() == BO.ProjectStatus.PlanStage)
            {
                IsFirstStage = true;
                IsThirdStage = false;
                IsSecondStage = false;
            }
            else
                if (s_bl.Task.GetStatusOfProject() == BO.ProjectStatus.ScheduleDetermination)
            {
                IsFirstStage = false;
                IsThirdStage = false;
                IsSecondStage = true;
            }
            else
            {
                IsFirstStage = false;
                IsThirdStage = true;
                IsSecondStage = false;
            }
            Hello = "Hello " + s_bl.Worker.ReadWorker(IdOfWorker).Name;
            Id= IdOfWorker;
            MyTask = s_bl.Worker.ReadWorker(Id).Task;
            if (MyTask == null)
                HaveTask = false;
            else
                HaveTask = true;


        }

        private void ButtonChoseTask_Click(object sender, RoutedEventArgs e)
        {
            new TasksForWorkerList(Id).ShowDialog();
            HaveTask = true;
        }


        private void Update_click(object sender, MouseButtonEventArgs e)
        {
            new TaskOfWorker(MyTask.Id).ShowDialog();
            MyTask = s_bl.Worker.ReadWorker(Id).Task;


        }

       
    }
}
