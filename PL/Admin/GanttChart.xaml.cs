using System;
using System.Collections.Generic;
using System.Data;
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
using static System.Net.Mime.MediaTypeNames;

namespace PL.Admin
{
    /// <summary>
    /// Interaction logic for GanttChart.xaml
    /// </summary>
    public partial class GanttChart : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        public GanttChart()
        {
            InitializeComponent();
        }


        private void dataGridSched_Initialized(object sender, EventArgs e)
        {

            DataGrid? dataGrid = sender as DataGrid; //the graphic container

            DataTable dataTable = new DataTable(); //the logic container

            //add COLUMNS to datagrid and datatable
            if (dataGrid != null)
            {
                // dataGrid.Columns.Add(new DataGridTextColumn() { Header = "Task Id", Binding = new Binding("[0]") });
                dataTable.Columns.Add("Task Id", typeof(int));

                //dataGrid.Columns.Add(new DataGridTextColumn() { Header = "Task Alias", Binding = new Binding("[1]") });
                dataTable.Columns.Add("Task Alias", typeof(string));

                //dataGrid.Columns.Add(new DataGridTextColumn() { Header = "Engineer Id", Binding = new Binding("[2]") });
                dataTable.Columns.Add("Engineer Id", typeof(int));

                // dataGrid.Columns.Add(new DataGridTextColumn() { Header = "Engineer Name", Binding = new Binding("[3]") });
                dataTable.Columns.Add("Engineer Name", typeof(string));

                int col = 4;
                for (DateTime day = (DateTime)s_bl.Schedule.getStartDateProject(); day <= (DateTime)s_bl.Schedule.getEndDateProject(); day = day.AddDays(1))
                {
                    string strDay = $"{day.Day}/{day.Month}/{day.Year}"; //"21/2/2024"
                    dataGrid.Columns.Add(new DataGridTextColumn() { Header = strDay, Binding = new Binding($"[{col}]") });
                    dataTable.Columns.Add(strDay, typeof(int));// typeof(System.Windows.Media.Color));
                    col++;
                }
            }

            //add ROWS to logic container (data table)
            IEnumerable<BO.TaskSchedule> orderedlistTasksScheduale = s_bl.Task.ReadAllSchedule();
            foreach (BO.TaskSchedule task in orderedlistTasksScheduale)
            {
                //dataGrid.CellStyle

                DataRow row = dataTable.NewRow();
                row[0] = task.Id;
                row[1] = task.Alias;
                row[2] = task.IdWorker;
                row[3] = task.NameWorker;

                for (DateTime day = (DateTime)s_bl.Schedule.getStartDateProject(); day <= (DateTime)s_bl.Schedule.getEndDateProject(); day = day.AddDays(1))
                {
                    string strDay = $"{day.Day}/{day.Month}/{day.Year}"; //"21/2/2024"

                    if (day < task.ScheduleStartDate || day > task.ScheduleEndDate)
                        row[strDay] = 0;
                    else
                        row[strDay] = 1;

                }
                dataTable.Rows.Add(row);
            }

            if (dataGrid != null)
            {
                dataGrid.ItemsSource = dataTable.DefaultView;
            }
        }
    }
}