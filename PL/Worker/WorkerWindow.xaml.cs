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
using BO;
using PL.Task;

namespace PL.Worker
{
    /// <summary>
    /// Interaction logic for WorkerWindow.xaml
    /// </summary>
    public partial class WorkerWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        public BO.WorkerExperience LevelOfWorker { get; set; } = BO.WorkerExperience.Beginner;


        public bool HaveTask
        {
            get { return (bool)GetValue(HaveTaskProperty); }
            set { SetValue(HaveTaskProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HaveTask.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HaveTaskProperty =
            DependencyProperty.Register("HaveTask", typeof(bool), typeof(WorkerWindow), new PropertyMetadata());


        public bool IsThirdStageAndWithoutTask
        {
            get { return (bool)GetValue(IsThirdStageAndWithoutTaskProperty); }
            set { SetValue(IsThirdStageAndWithoutTaskProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsThirdStageAndWithoutTask.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsThirdStageAndWithoutTaskProperty =
            DependencyProperty.Register("IsThirdStageAndWithoutTask", typeof(bool), typeof(WorkerWindow ), new PropertyMetadata());


        public BO.TaskInWorker MyTask
        {
            get { return (BO.TaskInWorker)GetValue(MyTaskProperty); }
            set { SetValue(MyTaskProperty, value); }
        }
        // Using a DependencyProperty as the backing store for MyWorker.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MyTaskProperty =
            DependencyProperty.Register("MyTask", typeof(BO.TaskInWorker), typeof(WorkerWindow), new PropertyMetadata());

        public BO.Worker MyWorker
        {
            get { return (BO.Worker)GetValue(MyWorkerProperty); }
            set { SetValue(MyWorkerProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyWorker.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MyWorkerProperty =
            DependencyProperty.Register("MyWorker", typeof(BO.Worker), typeof(WorkerWindow), new PropertyMetadata());


    
        public WorkerWindow( int IdOfWorker = 0)
        {
            IsThirdStageAndWithoutTask = false;
            try
            {
                if (IdOfWorker == 0)
                {
                    MyWorker = new BO.Worker { };
                }
                else
                {
                    MyWorker = s_bl.Worker.ReadWorker(IdOfWorker, true);
                    if (MyWorker.Task == null)
                        HaveTask = false;
                    else
                        HaveTask = true;
                    MyTask = MyWorker.Task;
                    if (HaveTask == false && s_bl.Task.GetStatusOfProject() == BO.ProjectStatus.ExecutionStage)
                        IsThirdStageAndWithoutTask = true;
                }
            }
            catch (BO.BlDoesNotExistException ex)
            {
                MessageBox.Show(ex.Message);
            }
            InitializeComponent();


        }

        private void AddOrUpdate_Click(object sender, RoutedEventArgs e)
        {
         
           
                try
                {
                    if (s_bl.Worker.ReadWorker(MyWorker.Id) == null)
                    {
                        s_bl.Worker.AddWorker(MyWorker);
                        MessageBox.Show($"Adding the employee with ID: {MyWorker.Id} card was successful");

                    }
                    else
                    {
                        s_bl.Worker.UpdateWorker(MyWorker);
                        MessageBox.Show($"Updating the employee with ID: {MyWorker.Id} card was successful");
                    }
                }
                catch (BO.BlInvalidGivenValueException ex)
                {
                    MessageBox.Show(ex.Message);


                }
                catch (BO.BlDoesNotExistException ex)
                {
                    MessageBox.Show(ex.Message);

                }
                catch (BO.BlNotActiveException ex)
                {
                    MessageBox.Show(ex.Message);

                }

            

            this.Close();
        }

        private void ComboBox_LevelChangedWorker(object sender, SelectionChangedEventArgs e)
        {
            MyWorker.Level = (BO.WorkerExperience)((ComboBox)sender).SelectedItem;
        }

        private void ButtonChoseTask_Click(object sender, RoutedEventArgs e)
        {
            new TasksForWorkerList(MyWorker.Id).ShowDialog();
            MyTask = s_bl.Worker.ReadWorker(MyWorker.Id).Task;
            MyWorker = s_bl.Worker.ReadWorker(MyWorker.Id);
            if (MyTask == null)
                HaveTask = false;
            else
                HaveTask = true;
            if (s_bl.Task.GetStatusOfProject() == BO.ProjectStatus.ExecutionStage && HaveTask == false)
                IsThirdStageAndWithoutTask = true;
        }
    }
}
