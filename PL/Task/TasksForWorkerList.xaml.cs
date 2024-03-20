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

namespace PL.Task
{
    
    /// <summary>
    /// Interaction logic for TasksForWorkerList.xaml
    /// </summary>
    public partial class TasksForWorkerList : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        public int chosenIdTask
        {
            get { return (int)GetValue(IdTaskProperty); }
            set { SetValue(IdTaskProperty, value); }
        }
        public static readonly DependencyProperty IdTaskProperty =
           DependencyProperty.Register("chosenIdTask", typeof(int), typeof(TasksForWorkerList), new PropertyMetadata(null));

        public int Id
        {
            get { return (int)GetValue(IdProperty); }
            set { SetValue(IdProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Id.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IdProperty =
            DependencyProperty.Register("Id", typeof(int), typeof(TasksForWorkerList), new PropertyMetadata(null));


        public IEnumerable<BO.TaskInList> TaskList
        {
            get { return (IEnumerable<BO.TaskInList>)GetValue(TaskListProperty); }
            set { SetValue(TaskListProperty, value); }
        }

        public static readonly DependencyProperty TaskListProperty =
            DependencyProperty.Register("TaskList", typeof(IEnumerable<BO.TaskInList>), typeof(TasksForWorkerList), new PropertyMetadata(null));


        public TasksForWorkerList(int id)
        {
         
            InitializeComponent();
            Id = id;
            TaskList = s_bl.Task.ReadAllTasks(BO.Filter.PossibleTaskForWorker, s_bl.Worker.ReadWorker(id, true).Level);


        }

        private void Chose_Click(object sender, RoutedEventArgs e)
        {if (chosenIdTask != null)
            {
                MessageBoxResult messageBoxResult = MessageBox.Show($"Are you sure you want to be in charge of task with Id={chosenIdTask}?", "hello", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (messageBoxResult == MessageBoxResult.Yes)

                {
                    try
                    {
                        BO.Worker choseWorker = s_bl.Worker.ReadWorker(Id, true);
                        BO.WorkerInTask workerTask = new BO.WorkerInTask { Name = choseWorker.Name, Id = choseWorker.Id };

                        BO.Task updTask = s_bl.Task.ReadTask(chosenIdTask);
                        updTask.Worker = workerTask;
                        s_bl.Task.UpdateTask(updTask);
                    }
                    catch (BO.BlDoesNotExistException ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    catch (BO.BlInvalidGivenValueException ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }

                this.Close();
            }


        }

        private void Chose_Task_mouse(object sender, MouseButtonEventArgs e)
        {
            chosenIdTask = ((BO.TaskInList)((ListView)sender).SelectedValue).Id;
        }
    }
}
