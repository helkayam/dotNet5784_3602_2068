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

namespace PL.Worker
{
    /// <summary>
    /// Interaction logic for SignInWindow.xaml
    /// </summary>
    public partial class SignInWindow : Window
    {


        public int ID
        {
            get { return (int)GetValue(IDProperty); }
            set { SetValue(IDProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ID.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IDProperty =
            DependencyProperty.Register("ID", typeof(int), typeof(SignInWindow), new PropertyMetadata());

        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();


        public SignInWindow()
        {
            InitializeComponent();
        }

        private void WorkerWindow_click(object sender, RoutedEventArgs e)
        {
            try
            {
                s_bl.Worker.ReadWorker(ID, true);
                new WorkerMainWindow(ID).ShowDialog();
                this.Close();
            }
            catch(BO.BlDoesNotExistException ex) 
            {
                    MessageBox.Show(ex.Message);
            }
             
        }
    }
}
