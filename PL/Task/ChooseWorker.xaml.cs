using BO;
using PL.Worker;
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
    /// Interaction logic for ChooseWorker.xaml
    /// </summary>
    /// 
    public partial class ChooseWorker : Window
    {

        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        public IEnumerable<BO.Worker> WorkerList
        {
            get { return (IEnumerable<BO.Worker>)GetValue(WorkerListProperty); }
            set { SetValue(WorkerListProperty, value); }
        }

        // Using a DependencyProperty as the backing store for WorkerList.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty WorkerListProperty =
            DependencyProperty.Register("WorkerList", typeof(IEnumerable<BO.Worker>), typeof(ChooseWorker), new PropertyMetadata());


        public int TaskId;
        public ChooseWorker(int id)
        {
            InitializeComponent();
            WorkerList=s_bl.Worker.ReadAllWorkers(BO.FilterWorker.WithoutTask);    
            TaskId=id;  
        }

        private void ListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                int id = ((BO.Worker)((ListView)sender).SelectedItem).Id;
                BO.Task? taskToUpdate = s_bl.Task.ReadTask(TaskId);
                taskToUpdate.Worker = s_bl.Worker.returnWorkerInList(id);
                s_bl.Task.UpdateTask(taskToUpdate);
                this.Close();
            }
            catch (BO.BlDoesNotExistException ex) 
            
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
