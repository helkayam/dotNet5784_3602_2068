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
    /// Interaction logic for ChoseDependency.xaml
    /// </summary>
    public partial class ChoseDependency : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        public IEnumerable<BO.TaskInList> TaskList
        {
            get { return (IEnumerable<BO.TaskInList>)GetValue(TaskListProperty); }
            set { SetValue(TaskListProperty, value); }
        }

        public static readonly DependencyProperty TaskListProperty =
            DependencyProperty.Register("TaskList", typeof(IEnumerable<BO.TaskInList>), typeof(ChoseDependency ), new PropertyMetadata(null));

        BO.Task myTask=new BO.Task();
        public ChoseDependency( BO.Task task)
        {
            InitializeComponent();
            TaskList = s_bl.Task.ReadAllTasks();
            myTask = task;

        }

        private void ListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (myTask.Dependencies == null)
                myTask.Dependencies = new List<BO.TaskInList>();
            myTask.Dependencies.Add(((BO.TaskInList)((ListView)sender).SelectedItem));
            this.Close();
        }
    }
}
