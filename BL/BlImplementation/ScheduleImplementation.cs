using BlApi;
using BO;
using DalApi;
using DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlImplementation
{
    internal class ScheduleImplementation:BlApi.ISchedule
    {

        private DalApi.IDal _dal = DalApi.Factory.Get;

        private readonly Bl _bl;
        internal ScheduleImplementation(Bl bl) => _bl = bl;

        /// <summary>
        /// This is a method that receives a date as a parameter and sends it to a class in the DAL layer that takes care of initializing the start date of the project 
        /// on the received date
        /// </summary>
        /// <param name="startDateProject"> Date to start the StartDateOfProject field </param>
        public void UpdateStartProjectDate(DateTime startDateProject)
        {
            if (startDateProject >= _bl.Clock)
                _dal.Schedule.UpdateStartDateProject(startDateProject);
            else
                throw new BO.BlInvalidGivenValueException($"error:start date of project is before current date {_dal.Schedule.GetCurrentDate()}");
        }

        public void UpdateCurrentDate(DateTime currentdt)
        {
            _dal.Schedule.UpdateCurrentDate(currentdt);
        }

        public void UpdateEndDateProjectDate(DateTime endDateProject)
        {
            if (endDateProject > _bl.Clock)
                _dal.Schedule.UpdateEndDateProject(endDateProject);
            else
                throw new BO.BlInvalidGivenValueException($"error:End date of project is before current date {_dal.Schedule.GetCurrentDate()}");
        }

        public DateTime? getStartDateProject()
        {
            try
            {
                if (_dal.Schedule.GetStartDateProject() == null)
                    return null;
                else
                return _dal.Schedule.GetStartDateProject();
            }
            catch(DO.DalDoesNotExistException ex) 
            {
                throw new BO.BlDoesNotExistException("The Start date of the project has not yet been updated",ex);
            
            }


        }


        public bool StartAndEndUpdated()
        {
            if (this.getEndDateProject() != null && this.getStartDateProject() != null&&_bl.Task.GetStatusOfProject()==BO.ProjectStatus.ExecutionStage)
                return true;
            else
                throw new BO.BlInvalidGivenValueException("The Start Date or End Date of the project are not updated!");
            return false;
        }
        public DateTime? getEndDateProject()
        {
            try
            {
                if (_dal.Schedule.getEndDateProject() == null)
                    return null;
                else
                return (DateTime)_dal.Schedule.getEndDateProject();
            }
            catch (DO.DalDoesNotExistException ex)
            {
                throw new BO.BlDoesNotExistException("The End date of the project has not yet been updated", ex);

            }


        }

        public DateTime? GetCurrentDate()
        {
            
                return _bl.Clock;

        }

        public void CreateAutomaticSchedule(IEnumerable<BO.TaskInList> TaskList)
        {
            if (_bl.Schedule.getStartDateProject() == null || _bl.Schedule.getEndDateProject() == null)
                throw new BO.BlInvalidGivenValueException("You must enter the start date and end date of project!");
            List<BO.TaskInList> tasks;

            IEnumerable<TaskInList> taskList = _bl.Task.ReadAllTasks();

            tasks=taskList.ToList();
             // Create a dictionary to store tasks by their IDs
             Dictionary<int, BO.TaskInList> taskLookup = tasks.ToDictionary(task => task.Id);

            // Create a graph represented as adjacency list
            Dictionary<BO.TaskInList, List<BO.TaskInList>> graph = new Dictionary<BO.TaskInList, List<BO.TaskInList>>();

            // Populate the graph with tasks and their dependencies
            foreach (var task in tasks)
            {


                graph[task] = _bl.Task.ReadTask(task.Id).Dependencies.Select(dep => taskLookup[dep.Id]).ToList();
            }

            // Perform topological sorting
            Stack<BO.TaskInList> sortedStack = new Stack<BO.TaskInList>();
            HashSet<BO.TaskInList> visited = new HashSet<BO.TaskInList>();

            foreach (var task in tasks)
            {
                if (!visited.Contains(task))
                {
                    TopologicalSortUtil(task, visited, sortedStack, graph);
                }
            }

            // Convert stack to list
            List<BO.TaskInList> sortedTasks = sortedStack.ToList();
            sortedTasks.Reverse();

            // Set scheduled dates based on topological order
           
            BO.Task BOFirstTask = _bl.Task.ReadTask(sortedTasks[0].Id);
            BOFirstTask.ScheduledDate = _bl.Schedule.getStartDateProject();
            BOFirstTask.ForecastDate = BOFirstTask.ScheduledDate + BOFirstTask.RequiredEffortTime; // Assuming ForecastDate is the same as CompleteDate
            BOFirstTask.Deadline = BOFirstTask.ForecastDate; // Assuming Deadline is the same as ForecastDate
            _bl.Task.UpdateTimeInSchedule(BOFirstTask);
            DateTime? previousEndDate = BOFirstTask.ForecastDate;
            foreach (var task in sortedTasks)
            {
                if (task.Id != BOFirstTask.Id)
                {
                    BO.Task BOTask = _bl.Task.ReadTask(task.Id);
                        BOTask.ScheduledDate = previousEndDate;
                    
                    BOTask.ForecastDate = BOTask.ScheduledDate + BOTask.RequiredEffortTime; // Assuming ForecastDate is the same as CompleteDate
                    BOTask.Deadline = BOTask.ForecastDate; // Assuming Deadline is the same as ForecastDate
                    _bl.Task.UpdateTimeInSchedule(BOTask);
                    previousEndDate = BOTask.ForecastDate;
                }

            }
           

        }

        private static void TopologicalSortUtil(BO.TaskInList task, HashSet<BO.TaskInList> visited, Stack<BO.TaskInList> stack, Dictionary<BO.TaskInList, List<BO.TaskInList>> graph)
        {
            visited.Add(task);

            foreach (var dependency in graph[task])
            {
                if (!visited.Contains(dependency))
                {
                    TopologicalSortUtil(dependency, visited, stack, graph);
                }
            }

            stack.Push(task);
        }
    }




}


