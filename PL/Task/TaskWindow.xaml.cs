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
using PL.Worker;

namespace PL.Task
{

    /// <summary>
    /// Interaction logic for TaskWindow.xaml
    /// </summary>
    public partial class TaskWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
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

                InitializeComponent();
                if (IdOfTask == -1)
                    MyTask = new BO.Task();
                else
                    MyTask = s_bl.Task.ReadTask(IdOfTask,true);
            }
            catch (BO.BlDoesNotExistException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
