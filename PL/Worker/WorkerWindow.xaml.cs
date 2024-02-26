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


    
        public WorkerWindow( int IdOfWorker = 0)
        {
            try
            {
                InitializeComponent();
                if (IdOfWorker == 0)
                {
                    MyWorker = new BO.Worker { };
                }
                else
                    MyWorker = s_bl.Worker.ReadWorker(IdOfWorker,true);
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

       
    }
}
