using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
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
    /// Interaction logic for TaskListWindow.xaml
    /// </summary>
    public partial class TaskListWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        public BO.Filter filter {  get; set; }=BO.Filter.None;

        public string IdWorker { get; set; } = "";

        public int IdSelectedToDelete = -1;



        public bool DoYouWantToDelete
        {
            get { return (bool)GetValue(DoYouWantToDeleteProperty); }
            set { SetValue(DoYouWantToDeleteProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DoYouWantToDelete.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DoYouWantToDeleteProperty =
            DependencyProperty.Register("DoYouWantToDelete", typeof(bool), typeof(TaskListWindow), new PropertyMetadata());


        public bool IsFirstStage
        {
            get { return (bool)GetValue(isFirstStageProperty); }
            set { SetValue(isFirstStageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for isThirdStage  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty isFirstStageProperty =
            DependencyProperty.Register("isFirstStage", typeof(bool), typeof(TaskListWindow), new PropertyMetadata(null));


        public bool ByStatus
        {
            get { return (bool)GetValue(byStatusProperty); }
            set { SetValue(byStatusProperty, value); }
        }

        // Using a DependencyProperty as the backing store for by possible task for worker.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty byStatusProperty =
            DependencyProperty.Register("ByStatus", typeof(bool), typeof(TaskListWindow), new PropertyMetadata(null));

        public bool ByPossibleTaskforWorker
        {
            get { return (bool)GetValue(byPossibleTask); }
            set { SetValue(byPossibleTask, value); }
        }

        // Using a DependencyProperty as the backing store for by possible task for worker.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty byPossibleTask =
            DependencyProperty.Register("ByPossibleTaskforWorker", typeof(bool), typeof(TaskListWindow), new PropertyMetadata(null));
        public bool Bylevel
        {
            get { return (bool)GetValue(bylevelProperty); }
            set { SetValue(bylevelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for bylevel.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty bylevelProperty =
            DependencyProperty.Register("Bylevel", typeof(bool), typeof(TaskListWindow), new PropertyMetadata(null));

        public IEnumerable<BO.TaskInList> TaskList
        {
            get { return (IEnumerable<BO.TaskInList>)GetValue(TaskListProperty); }
            set { SetValue(TaskListProperty, value); }
        }

        public static readonly DependencyProperty TaskListProperty =
            DependencyProperty.Register("TaskList", typeof(IEnumerable<BO.TaskInList>), typeof(TaskListWindow), new PropertyMetadata(null));


        public TaskListWindow()
        {
            Bylevel = false;
            ByStatus = false;
            DoYouWantToDelete=false;
            ByPossibleTaskforWorker = false;
            InitializeComponent();
            TaskList=s_bl.Task.ReadAllTasks(); 
        }

        private void TaskList_FilterChanged(object sender, SelectionChangedEventArgs e)
        {
            if (filter == BO.Filter.ByComplexity)
            {
                Bylevel = true;
                ByPossibleTaskforWorker = false;
                ByStatus = false;
            }
            else
            if (filter == BO.Filter.PossibleTaskForWorker)
            {
                ByPossibleTaskforWorker = true;
                ByStatus = false;
                Bylevel = false;


            }
            else
            if (filter == BO.Filter.Status)
            {
                ByStatus = true;
                Bylevel = false;
                ByPossibleTaskforWorker = false;
            }
            else
                    {
                TaskList = s_bl.Task.ReadAllTasks(filter);
                Bylevel = false;
                ByPossibleTaskforWorker = false;
                ByStatus = false;
            }

        }
       private void  ComboBoxLevelTask_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            TaskList=s_bl.Task.ReadAllTasks(filter, (BO.WorkerExperience)((ComboBox)sender).SelectedItem);

        }

        private void ComboBoxStatusTask_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TaskList = s_bl.Task.ReadAllTasks(filter, (BO.Status)((ComboBox)sender).SelectedItem);

        }

        private void ListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            int id = ((BO.TaskInList)((ListView)sender).SelectedItem).Id;
            new TaskWindow(id).ShowDialog();
            TaskList  = s_bl.Task.ReadAllTasks();

        }
        private void TextBoxIdWorkerFilter_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                try
                {
                BO.Worker w = s_bl.Worker.ReadWorker( int.Parse((((TextBox)sender).Text)),true)!;
               TaskList = s_bl.Task.ReadAllTasks(filter, w.Level);
                
            }
            catch(BO.BlDoesNotExistException ex)
            {
                MessageBox.Show(ex.Message);

            }
        }

        private void ContentSearch_Changed(object sender, TextChangedEventArgs e)
        {
            if (sender.ToString() != null)
                TaskList = s_bl.Task.ReadAllSearch(((TextBox)sender).Text);
            else
                TaskList = s_bl.Task.ReadAllTasks();

        }

        private void ButtonAddNewTask_Click(object sender, RoutedEventArgs e)
        {
            new TaskWindow().ShowDialog();
            TaskList=s_bl.Task.ReadAllTasks();
        }

        private void DeleteTask_Click(object sender, RoutedEventArgs e)
        {
            try
                {
                MessageBoxResult messageBoxResult = MessageBox.Show($"Are you sure you want to delete worker with Id={IdSelectedToDelete}?", "hello", MessageBoxButton.YesNo, MessageBoxImage.Question);
                switch (messageBoxResult)
                {
                    case MessageBoxResult.Yes: s_bl.Task.RemoveTask(IdSelectedToDelete); break;
                    case MessageBoxResult.No: break;
                }
                TaskList = s_bl.Task.ReadAllTasks();
                DoYouWantToDelete = false;

            }
            catch (BO.BlNotErasableException ex)
            {
                MessageBox.Show(ex.Message);

            }

            catch (BO.BlDoesNotExistException ex)
            {
                MessageBox.Show(ex.Message);

            }
            catch(BO.BlInvalidGivenValueException ex) 
            {
                MessageBox.Show(ex.Message);

            }

        }

        private void ChooceTaskToDelete_click(object sender, MouseButtonEventArgs e)
        {
            IdSelectedToDelete = ((BO.Task)((ListView)sender).SelectedValue).Id;
            DoYouWantToDelete = true;

        }
    }
}
