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

namespace PL.Worker
{
    /// <summary>
    /// Interaction logic for TaskOfWorker.xaml
    /// </summary>
    public partial class TaskOfWorker : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();



        public bool isStart;
        public BO.Task MyTask
        {
            get { return (BO.Task)GetValue(MyTaskProperty); }
            set { SetValue(MyTaskProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyWorker.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MyTaskProperty =
            DependencyProperty.Register("MyTask", typeof(BO.Task), typeof(TaskWindow), new PropertyMetadata());


        public TaskOfWorker(int Id)
        {
            InitializeComponent();

            try
            {
                MyTask = s_bl.Task.ReadTask(Id, true);
                if (MyTask.StartDate == null)
                    isStart = false;
                else
                    isStart = true;

            }
            catch (BO.BlDoesNotExistException ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void StartTaskButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                isStart = true;
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
               
                    s_bl.Task.UpdateTask(MyTask);
                    MessageBox.Show($"Updating the Task with ID: {MyTask.Id} card was successful");
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
            catch (BO.BlDoesNotExistException ex)
            {
                MessageBox.Show(ex.Message);
            }


        }

    }
}
