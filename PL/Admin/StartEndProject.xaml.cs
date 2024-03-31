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

namespace PL.Admin
{
    /// <summary>
    /// Interaction logic for StartEndProject.xaml
    /// </summary>
    public partial class StartEndProject : Window
    {





        public DateTime? StartDate
        {
            get { return (DateTime?)GetValue(StartDateProperty); }
            set { SetValue(StartDateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for StartDate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StartDateProperty =
            DependencyProperty.Register("StartDate", typeof(DateTime?), typeof(StartEndProject), new PropertyMetadata(null));



        public DateTime? EndDate
        {
            get { return (DateTime?)GetValue(EndDateProperty); }
            set { SetValue(EndDateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for EndDate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EndDateProperty =
            DependencyProperty.Register("EndDate", typeof(DateTime?), typeof(StartEndProject), new PropertyMetadata(null));



        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        public StartEndProject()
        {
            InitializeComponent();
            StartDate = null;
            EndDate=null;
        }

        private void InitStartEndProject_click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (StartDate != null)
                    s_bl.Schedule.UpdateStartProjectDate((DateTime)StartDate);
                if (EndDate != null)
                    s_bl.Schedule.UpdateEndDateProjectDate((DateTime)EndDate);
                this.Close();

            }
            catch (BO.BlInvalidGivenValueException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch(Exception ex) 
            {
                MessageBox.Show(ex.Message);

            }
        }
    }
}
