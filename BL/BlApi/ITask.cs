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

    public BO.Task? ReadTask(int Id);

    public void AddOrUpdateStartDate(int Id);


    public void UpdateScheduleDate(int Id, DateTime mySceduelDate);

    public ProjectStatus GetStatusOfProject();


    public IEnumerable<BO.TaskInList> ReadAllTasks(BO.Filter enumFilter = BO.Filter.None, Object? filtervalue = null);

    public void deleteAll();

    //public ProjectStatus ProjectStatus();

    //public bool AllTaskWithDate();


    //public IGrouping<BO.Status , BO.TaskInList> GroupByStatus();

}
