using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;

namespace BlApi;

public interface ITask
{
    public void AddTask(BO.Task newTask);
    public void RemoveTask(int Id);

    public void UpdateTask(BO.Task TaskToUpdate);

    public void UpdateTimeInSchedule(BO.Task myTask);
    public BO.Task? ReadTask(int Id,bool throwexception=false);

    public IEnumerable<BO.TaskInWorker> ReadAllWorkerTask(int Id);

    public void AddOrUpdateStartDate(int Id);
    public void AddCompleteDate(int Id);


    public void UpdateScheduleDate(int Id, DateTime mySceduelDate);

    public ProjectStatus GetStatusOfProject();

    public IEnumerable<BO.TaskSchedule> ReadAllSchedule();

    public IEnumerable<BO.TaskInList> ReadAllSearch(string search);

    public IEnumerable<BO.TaskInList> ReadAllTasks(BO.Filter enumFilter = BO.Filter.None, Object? filtervalue = null);

    public void deleteAll();

    public bool CanStartTheTask(int id);

    public int getNextId();

    public void AddDependency(BO.Task t, int IdDependsOn);


    //public ProjectStatus ProjectStatus();

    //public bool AllTaskWithDate();


    //public IGrouping<BO.Status , BO.TaskInList> GroupByStatus();

}
