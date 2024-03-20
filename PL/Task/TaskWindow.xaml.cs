using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
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
using BO;
using PL.Worker;

namespace PL.Task
{

    /// <summary>
    /// Interaction logic for TaskWindow.xaml
    /// </summary>
    public partial class TaskWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        public bool isthirdstage;
        public BO.WorkerExperience LevelOfTask { get; set; } = BO.WorkerExperience.Beginner;
        public BO.Status StatusOfTask { get; set; } = BO.Status.Unscheduled;

        public static BO.ProjectStatus StatusProject { get; set; } = s_bl.Task.GetStatusOfProject();


        public IEnumerable<BO.WorkerInTask?> WorkersPossible;




        public bool WithoutWorker
        {
            get { return (bool)GetValue(WithoutWorkerProperty); }
            set { SetValue(WithoutWorkerProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty WithoutWorkerProperty =
            DependencyProperty.Register("WithoutWorker", typeof(bool ), typeof(TaskWindow ), new PropertyMetadata());


        public bool WithWorkerAndThirdStage
        {
            get { return (bool)GetValue(WithoutWorkerAndThirdStageProperty); }
            set { SetValue(WithoutWorkerAndThirdStageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for WithoutWorkerAndThirdStage.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty WithWorkerAndThirdStageProperty =
            DependencyProperty.Register("WithWorkerAndThirdStage", typeof(bool), typeof(TaskWindow), new PropertyMetadata());

        public bool WithoutWorkerAndThirdStage
        {
            get { return (bool)GetValue(WithoutWorkerAndThirdStageProperty); }
            set { SetValue(WithoutWorkerAndThirdStageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for WithoutWorkerAndThirdStage.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty WithoutWorkerAndThirdStageProperty =
            DependencyProperty.Register("WithoutWorkerAndThirdStage", typeof(bool), typeof(TaskWindow), new PropertyMetadata());



        public bool IsFirstStage
        {
            get { return (bool)GetValue(isFirstStageProperty); }
            set { SetValue(isFirstStageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for isThirdStage  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty isFirstStageProperty =
            DependencyProperty.Register("isFirstStage", typeof(bool), typeof(TaskWindow), new PropertyMetadata(null));


        public bool IsThirdStage
        {
            get { return (bool)GetValue(isThirdStageProperty); }
            set { SetValue(isThirdStageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for isThirdStage  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty isThirdStageProperty =
            DependencyProperty.Register("isThirdStage", typeof(bool), typeof(TaskWindow), new PropertyMetadata(null));

        public bool IsSecondStage
        {
            get { return (bool)GetValue(isSecondStageProperty); }
            set { SetValue(isSecondStageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for isThirdStage  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty isSecondStageProperty =
            DependencyProperty.Register("isSecondStage", typeof(bool), typeof(TaskWindow), new PropertyMetadata(null));



        public BO.Task MyTask
        {
            get { return (BO.Task)GetValue(MyTaskProperty); }
            set { SetValue(MyTaskProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyWorker.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MyTaskProperty =
            DependencyProperty.Register("MyTask", typeof(BO.Task), typeof(TaskWindow), new PropertyMetadata());


        public TaskWindow(int IdOfTask = -1)
        {

            try
            {
                if (s_bl.Task.GetStatusOfProject() == BO.ProjectStatus.PlanStage)
                {
                    IsFirstStage = true;
                    IsThirdStage = false;
                    IsSecondStage = false;
                    WithWorkerAndThirdStage = false;
                    WithoutWorkerAndThirdStage = false;
                }
                else
                 if(s_bl.Task.GetStatusOfProject() == BO.ProjectStatus.ScheduleDetermination)
                {
                    IsFirstStage = false;
                    IsThirdStage = false;
                    IsSecondStage = true;
                    WithWorkerAndThirdStage = false;
                    WithoutWorkerAndThirdStage = false;

                }
                else
                {
                    IsFirstStage = false;
                    IsThirdStage = true;
                    IsSecondStage = false;
                    
                }

                if (IdOfTask == -1)
                    MyTask = new BO.Task();
                else
                {
                    MyTask = s_bl.Task.ReadTask(IdOfTask, true);
                    if (MyTask.Worker != null && IsThirdStage)
                    {
                        WithWorkerAndThirdStage = true;
                        WithoutWorkerAndThirdStage = false;
                    }
                    if (MyTask.Worker == null && IsThirdStage)
                    {
                        WithoutWorkerAndThirdStage = true;
                        WithWorkerAndThirdStage = false;
                    }
                    if (MyTask.Worker == null)
                        WithoutWorker = true;
                    WorkersPossible = s_bl.Worker.ReadAllWorkersSuitabeToTask(MyTask.Id);


                }
            }
            catch (BO.BlDoesNotExistException ex)
            {
                MessageBox.Show(ex.Message);
            }
            InitializeComponent();

        }

        private void ComboBox_LevelChangedTask(object sender, SelectionChangedEventArgs e)
        {
            MyTask.Complexity = (BO.WorkerExperience)((ComboBox)sender).SelectedItem;
        }



        private void StartTaskButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                s_bl.Task.AddOrUpdateStartDate(MyTask.Id);
            }
            catch (BO.BlInvalidGivenValueException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (BO.BlDoesNotExistException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void FinishTaskButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                s_bl.Task.AddCompleteDate(MyTask.Id);
            }
            catch (BO.BlInvalidGivenValueException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (BO.BlDoesNotExistException ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void AddOrUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (MyTask.Id == -1)
                {
                    s_bl.Task.AddTask(MyTask);
                    MessageBox.Show($"Adding the Task with ID: {MyTask.Id} card was successful");

                }
                else
                {
                    s_bl.Task.UpdateTask(MyTask);
                    MessageBox.Show($"Updating the Task with ID: {MyTask.Id} card was successful");
                    
                }
                this.Close();

            }
            catch (BO.BlInvalidGivenValueException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (BO.BlForbiddenActionException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch(BO.BlDoesNotExistException ex)
            {
                MessageBox.Show(ex.Message);
            }


        }

        private void AddDependency_Click(object sender, RoutedEventArgs e)
        {
            new ChoseDependency(MyTask).ShowDialog();
        }

        private void ComboBox_WorkerChanged(object sender, SelectionChangedEventArgs e)
        {
           
            BO.WorkerInTask workerInTask = (BO.WorkerInTask)((ComboBox)sender).SelectedItem;
            MyTask.Worker=workerInTask;

        }
    }
}
