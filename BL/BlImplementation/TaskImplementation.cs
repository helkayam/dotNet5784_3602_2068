


namespace BlImplementation;
using BlApi;
using BO;
using System;
using System.Collections.Generic;

internal class TaskImplementation : ITask
{
    private DalApi.IDal _dal = DalApi.Factory.Get;
    public Status getStatus(DO.Task task)
    {
        TimeSpan t = TimeSpan.FromDays(2);
        if (task.ScheduledDate == null)
            return Status.Unscheduled;
        else
            if (task.StartDate == null)
            return Status.Scheduled;
        else
            if (task.CompleteDate != null)
            return Status.Done;
        else
            if (task.DeadLineDate - task.StartDate + task.RequiredEffortTime < t)
            return Status.InJeopardy;
        return Status.OnTrack;
    }
    public void AddTask(BO.Task newTask)
    {
        DO.Task DoTask = new DO.Task(newTask.Alias, (DO.WorkerExperience)(newTask.Complexity), newTask.Description, newTask.Id, newTask.ScheduledDate, newTask.Deadline, newTask.Worker.Id);
        DoTask.RequiredEffortTime = newTask.RequiredEffortTime; 
        DoTask.Eraseable=newTask.Eraseable;
        //add dependencies?
        try
        {
            if (DoTask.Id >= 0 && DoTask.Alias.Length > 0)
                _dal.Task.Create(DoTask);
            else
                throw new Exception();
        }
        catch(DO.DalAlreadyExistException ex)
        {
            //throw new BO.BlAlreadyExistsException($"Task with ID={newTask.Id} already exists", ex);
        }
        catch(Exception ex)
        {
         
        }

    }

    public IEnumerable<BO.TaskInList?> ReadAllWorkers(BO.Filter enumFilter=BO.Filter.None,Object? filtervalue=null)
    {

        IEnumerable<DO.Task?> result =
       enumFilter switch
       {
           BO.Filter.ByComplexity => ((DO.WorkerExperience)filtervalue != null) ? _dal.Task.ReadAll(bc => bc.Complexity == (DO.WorkerExperience)filtervalue) : _dal.Task.ReadAll(),
           BO.Filter.None => _dal.Task.ReadAll(),
            BO.Filter.Done=> _dal.Task.ReadAll(d=>d.CompleteDate !=null)
       } ;

        return (from DO.Task dotask in result
                select new TaskInList
                {
                    Alias = dotask.Alias,
                    Id = dotask.Id,
                    Description = dotask.Description,
                    Status = getStatus(dotask)
                }).OrderBy(t => t.Alias); 
 
      
    }

    public System.Threading.Tasks.Task? ReadTask(int Id)
    {
        throw new NotImplementedException();
    }

    public void RemoveTask(int Id)
    {
        throw new NotImplementedException();
    }

    public void UpdateTask(System.Threading.Tasks.Task TaskToUpdate)
    {
        throw new NotImplementedException();
    }
}

