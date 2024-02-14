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

namespace PL.Worker
{
    /// <summary>
    /// Interaction logic for WorkerWindow.xaml
    /// </summary>
    public partial class WorkerWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        public BO.WorkerExperience LevelOfWorker { get; set; } = BO.WorkerExperience.Beginner;



        public BO.Worker MyWorker
        {
            get { return (BO.Worker)GetValue(MyWorkerProperty); }
            set { SetValue(MyWorkerProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyWorker.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MyWorkerProperty =
            DependencyProperty.Register("MyWorker", typeof(BO.Worker), typeof(WorkerWindow), new PropertyMetadata());



        public WorkerWindow(string Name,int IdOfWorker = 0)
        {
            try
             {
                InitializeComponent();
                if (IdOfWorker == 0)
                {
                    MyWorker = new BO.Worker { Name = Name };
                }
                else
                    MyWorker = s_bl.Worker.ReadWorker(IdOfWorker);
            }
            catch (Dal.DalDoesNotExistException ex)
            {
                throw new PL.BlDoesNotExistException($"Worker with ID={IdOfWorker} does Not exist", ex);
            }


        }

        private void AddOrUpdate_Click(object sender, RoutedEventArgs e)
        {
            if(MyWorker.Id==0)
            {
                s_bl.Worker.AddWorker(MyWorker);
                //if(s_bl.Worker.ReadWorker())
            }
            else
            {
                try
                {
                    s_bl.Worker.UpdateWorker(MyWorker);
                    //cout messege for user abut succes
                    WorkerWindow.Close();
                }
                catch ( BO.BlInvalidGivenValueException ex)
                { 
                    throw ex;
                
                }
                catch (DO.DalDoesNotExistException ex)
                {
                    throw new BO.BlDoesNotExistException($"Worker with ID={MyWorker.Id} does Not exist", ex);

                }
                catch (DO.DalNotActiveException ex)
                {
                    throw new BO.BlNotActiveException($"Worker with ID={MyWorker.Id} is Not active", ex);

                }

            }
    }
}
