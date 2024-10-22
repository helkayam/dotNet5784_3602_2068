﻿using PL.Task;
using PL.Worker;
using System;
using System.CodeDom;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PL.Admin
{
    /// <summary>
    /// Interaction logic for AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        public DateTime Clock
        {
            get { return s_bl.Clock; }
            set { SetValue(ClockProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Clock.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ClockProperty =
            DependencyProperty.Register("Clock", typeof(DateTime), typeof(AdminWindow), new PropertyMetadata());


        public IEnumerable<BO.TaskInList> TaskList
        {
            get { return (IEnumerable<BO.TaskInList>)GetValue(TaskListProperty); }
            set { SetValue(TaskListProperty, value); }
        }

        public static readonly DependencyProperty TaskListProperty =
            DependencyProperty.Register("TaskList", typeof(IEnumerable<BO.TaskInList>), typeof(AdminWindow), new PropertyMetadata(null));


        public AdminWindow()
        {

            Clock = s_bl.Clock;
            TaskList = s_bl.Task.ReadAllTasks();
            InitializeComponent();
        }

        private void ButtonWorker_Click(object sender, RoutedEventArgs e)
        {
            new WorkerListWindow().ShowDialog();
        }

        private void ButtonTask_Click(object sender, RoutedEventArgs e)
        {
            new TaskListWindow().ShowDialog();
        }
        private void ButtonINIT_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("do you want to initialize data Base?", "hello", MessageBoxButton.YesNo, MessageBoxImage.Question);
            switch (messageBoxResult)
            {
                case MessageBoxResult.Yes: s_bl.InitializeDB(); break;
                case MessageBoxResult.No: break;
            }

        }

        private void ButtonReset_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("do you want to reset data Base?", "hello", MessageBoxButton.YesNo, MessageBoxImage.Question);
            switch (messageBoxResult)
            {
                case MessageBoxResult.Yes:
                    s_bl.Task.deleteAll();
                    s_bl.Worker.deleteAll() ; 
                    break;
                case MessageBoxResult.No: break;
            }
        }

        private void AddOneDay_click(object sender, RoutedEventArgs e)
        {
            s_bl.IncreasInDay();
            Clock = s_bl.Clock;
        }

        private void AddOneHour_click(object sender, RoutedEventArgs e)
        {
            s_bl.IncreasInHour();
            Clock = s_bl.Clock;
        }

        private void AddWeek_click(object sender, RoutedEventArgs e)
        {
            s_bl.IncreasInWeek();
            Clock = s_bl.Clock;
        }

        private void InitClock_click(object sender, RoutedEventArgs e)
        {
            s_bl.InitClock();
            Clock = s_bl.Clock;
        }

        private void CreateSchedule_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                s_bl.Schedule.CreateAutomaticSchedule(TaskList);
                MessageBox.Show("The schedule was succesfully performed");
            }
            catch (BO.BlInvalidGivenValueException ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void Gantt_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                s_bl.Schedule.StartAndEndUpdated();
                new GanttChart().ShowDialog();
            }
            catch (BO.BlInvalidGivenValueException ex)
            {
                MessageBox.Show(ex.Message);

            }
        }

        private void InitStartOrEndProject_Click(object sender, RoutedEventArgs e)
        {
           new StartEndProject().ShowDialog();

        }
    }
}
