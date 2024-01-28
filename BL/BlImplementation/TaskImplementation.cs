namespace BlImplementation;
using BlApi;
using DalApi;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

internal class TaskImplementation : BlApi.ITask
{
    private DalApi.IDal _dal = DalApi.Factory.Get;
    public BO.Status getStatus(DO.Task task)
    {
        TimeSpan t = TimeSpan.FromDays(2);
        if (task.ScheduledDate == null)
            return BO.Status.Unscheduled;
        else
            if (task.StartDate == null)
            return BO.Status.Scheduled;
        else
            if (task.CompleteDate != null)
            return BO.Status.Done;
        else
             return BO.Status.OnTrack;
    }
    public void AddTask(BO.Task newTask)
    {
        DO.Task DoTask = new DO.Task(newTask.Alias, (DO.WorkerExperience)(newTask.Complexity), newTask.Description, newTask.Id, newTask.ScheduledDate, newTask.Deadline, newTask.Worker.Id);
        DoTask.RequiredEffortTime = newTask.RequiredEffortTime; 
        DoTask.Eraseable=newTask.Eraseable;

        //add dependencies
        var item = from BoDep in newTask.Dependencies
                   let id = newTask.Id
                   select new DO.Dependency { DependentTask = BoDep.Id, DependsOnTask = id };
        foreach (var dep in item)
        {
            _dal.Dependency.Create(dep);
        }

        try
        {
            if ( DoTask.Alias.Length > 0&& DoTask.Id>=0)
                _dal.Task.Create(DoTask);
            else
                throw new BO.BlInvalidGivenValueException($"One of the data of Task with ID={DoTask.Id} is incorrect");

        }
        catch (DO.DalAlreadyExistException ex)
        {
            throw new BO.BlAlreadyExistsException($"Task with ID={newTask.Id} already exists", ex);
        }
        

    }

    public IEnumerable<BO.TaskInList?> ReadAllTasks(BO.Filter enumFilter=BO.Filter.None,Object? filtervalue=null)
    {

        IEnumerable<DO.Task?> result =
       enumFilter switch
       {
           BO.Filter.ByComplexity => ((DO.WorkerExperience)filtervalue != null) ? _dal.Task.ReadAll(bc => bc.Complexity == (DO.WorkerExperience)filtervalue) : _dal.Task.ReadAll(),
           BO.Filter.None => _dal.Task.ReadAll(),
            BO.Filter.Status=> ((BO.Status)filtervalue != null) ? _dal.Task.ReadAll(s => getStatus(s) == (BO.Status)filtervalue) : _dal.Task.ReadAll()
       };

        return result.Select(dotask => new BO.TaskInList()
        {
            Alias = dotask.Alias,
            Id = dotask.Id,
            Description = dotask.Description,
            Status = getStatus(dotask)
        }
        ).OrderBy(t => t.Alias);
       
    }

    public BO.Task? ReadTask(int Id)
    {
        
        BO.Task BoTask = new BO.Task();
        try
        {
            DO.Task DoTask = _dal.Task.Read(Id);
            BoTask.Id = DoTask.Id;
            BoTask.Description = DoTask.Description;
            BoTask.Alias = DoTask.Alias;
            BoTask.CreatedAtDate = DoTask.CreatedAtDate;
            BoTask.Status = getStatus(DoTask);
            BoTask.Eraseable = DoTask.Eraseable;

      
            BoTask.RequiredEffortTime = DoTask.RequiredEffortTime;
            BoTask.ScheduledDate = DoTask.ScheduledDate;
            BoTask.StartDate = DoTask.StartDate;
            if (DoTask.StartDate < DoTask.ScheduledDate)
                BoTask.ForecastDate = DoTask.ScheduledDate + DoTask.RequiredEffortTime;
            else
                BoTask.ForecastDate = DoTask.StartDate + DoTask.RequiredEffortTime;

            BoTask.Deadline = DoTask.DeadLineDate;
            BoTask.CompleteDate = DoTask.CompleteDate;
            BoTask.Deliverables = DoTask.Deliverables;
            BoTask.Remarks = DoTask.Remarks;
            BoTask.Complexity = (BO.WorkerExperience)(DoTask.Complexity);

            //get the list of dependencied and check where this task is dependent on anothe task
            var dep=_dal.Dependency.ReadAll().Where(item=>item.DependentTask==Id).
                Select(item=>new BO.TaskInList()
                {
                    Id = item.DependsOnTask,
                    Description = (_dal.Task.Read(item.DependsOnTask)).Description,
                    Alias = (_dal.Task.Read(item.DependsOnTask)).Alias,
                    Status = getStatus(_dal.Task.Read(item.DependsOnTask))
                });
                
            
            foreach (var item in dep)
                BoTask.Dependencies.Add(item);

            //worker...
            int? idWorker = DoTask.WorkerId;
            if (idWorker != null)
            {

                BoTask.Worker.Id = (int)idWorker;
                BoTask.Worker.Name = (_dal.Worker.Read(BoTask.Worker.Id)!).Name;

            }
            
          



          
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BO.BlDoesNotExistException($"Task with ID={Id} does Not exist");
        }


        return BoTask;


    }

    public void RemoveTask(int Id)
    {
        try
        {
            var dep = _dal.Dependency.ReadAll().
                Where(item => item.DependsOnTask == Id || item.DependentTask == Id).Select(item => item);
           

            if (dep == null)
                _dal.Task.Delete(Id);
        }
        catch(DO.DalNotErasableException ex)
        {
            throw new BO.BlNotErasableException($"Task with ID={Id} does Not Erasable");

        }

        catch (DO.DalDoesNotExistException ex)
        {
            throw new BO.BlDoesNotExistException($"Task with ID={Id} does Not exist");
        }


    }

    public void UpdateTask(BO.Task TaskToUpdate)
    {
        try
        {
            if (TaskToUpdate.Id >= 0 && TaskToUpdate.Alias.Length > 0)
            {

                var TaskToUpd = _dal.Task.ReadAll().Where(item => item.Id == TaskToUpdate.Id)
                    .Select(item => item).FirstOrDefault();
                


                _dal.Task.Update(TaskToUpd);

            }
            else
                throw new BO.BlInvalidGivenValueException($"One of the data of the Updated Task is incorrect");

        }
        catch (BO.BlDoesNotExistException ex)
        {
            throw new BO.BlDoesNotExistException($"Task with id={TaskToUpdate.Id} does not exist");


        }

    }
}

