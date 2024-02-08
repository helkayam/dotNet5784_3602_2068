namespace BlImplementation;
using BlApi;
using BO;
using DO;
using System;
using System.Collections.Generic;

using System.Reflection.Emit;


/// <summary>
/// This is a class that implements the methods of the Worker logical entity by implementing the IWorker interface
/// </summary>
internal class WorkerImplementation : IWorker
{
    /// <summary>
    /// This is a private field named dal_ of type IDal, for access from the methods to the DAL layer. 
    /// The object will be initialized to work with the department that supports the Factory design template that initializes the 
    /// appropriate department in the DAL according to the configuration file dal-config.xml
    /// </summary>
    private DalApi.IDal _dal = DalApi.Factory.Get;


    /// <summary>
    /// The method receives an Worker object (logical entity)
    ///and checks the correctness of ID, name, cost and phone number.
    ///If the data is correct - you will make an attempt to request an addition to the data layer.
    ///The method will throw its own appropriate exception if adding an engineer failed(due to duplicate identifiers in a data layer - detection of an exception, or improper data as above)
    /// </summary>
    /// <param name="newWorker">logical entity of Worker object </param>
    /// <exception cref="BO.BlInvalidGivenValueException">This is an exception that is thrown if one of the logical entity's data is incorrect </exception>
    /// <exception cref="BO.BlAlreadyExistsException"> This is an exception that is thrown in the event that the data layer threw an exception following a situation where an attempt is made to add an object that already exists</exception>

    public ProjectStatus GetStatusOfProject()
    {
        if (_dal.Schedule.GetStartDateProject() == null)
            return BO.ProjectStatus.PlanStage;
        else

        if (_dal.Schedule.GetStartDateProject() != null)
        {
            var withoutDate = (from tasks in _dal.Task.ReadAll()
                               where (tasks.ScheduledDate == null)
                               select tasks).ToList();
            if (withoutDate.Count == 0)
                return BO.ProjectStatus.ExecutionStage;


        }

        return BO.ProjectStatus.ScheduleDetermination;

    }
    public void AddWorker(BO.Worker newWorker)
    {
        //in first case: id,level,name,PhoneNumber, Cost, Eraseable, active
        //in seconde case:..............
        //in third case: task, id,level,name,PhoneNumber, Cost, Eraseable, active
        /*     
    Id,
    Level
    Name
    PhoneNumber
    Cost
    Eraseable
    active 
    
     BO-
     Task   

         */
        if (GetStatusOfProject() != BO.ProjectStatus.ScheduleDetermination)
        {
            DO.Worker doWorker = new DO.Worker(newWorker.Id, (DO.WorkerExperience)newWorker.Level, newWorker.Name, newWorker.PhoneNumber, newWorker.Cost);
            if ((int)newWorker.Level == 2)
                doWorker.Eraseable =false;
            else
                doWorker.Eraseable = true;

            string[] phonePrefix = { "050", "051", "052", "053", "054", "055 ", "056", "058" };

            string prefixOfPhoneNumber = doWorker.PhoneNumber[0].ToString() + doWorker.PhoneNumber[1].ToString() + doWorker.PhoneNumber[2].ToString();
            try
            {
                if (doWorker.Id > 0 && (doWorker.Id.ToString().Length == 9) && doWorker.Name.Length > 0 &&
                    doWorker.Cost > 0 && doWorker.PhoneNumber.Length == 10 && phonePrefix.Contains(prefixOfPhoneNumber))
                {

                    _dal.Worker.Create(doWorker);
                    if (GetStatusOfProject() == BO.ProjectStatus.ExecutionStage)
                    {
                        DO.Task taskWithUpdateWorker = _dal.Task.Read(newWorker.Task.Id) with { WorkerId = newWorker.Id };
                        _dal.Task.Update(taskWithUpdateWorker);
                    }

                }
                else
                    throw new BO.BlInvalidGivenValueException($"One of the data of Worker with ID={doWorker.Id} is incorrect");

            }
            catch (DO.DalAlreadyExistException ex)
            {
                throw new BO.BlAlreadyExistsException($"Worker with ID={doWorker.Id} already exists", ex);
            }
        }

    }

    public bool WorkerDoesntHaveTask(int id)
    {
        var Tasks = (from currentTask in _dal.Task.ReadAll()
                     where currentTask.WorkerId == id
                     select currentTask).ToList();
        if (Tasks.Count()!=0)
            return false;
        else
            return true;
    }

    public IEnumerable<BO.Worker> ReadAllWorkers(BO.FilterWorker enumFilter = BO.FilterWorker.None, Object? filtervalue = null)
    {
        IEnumerable<BO.Worker> workersInList;
        IEnumerable<DO.Worker> readByLevel;
        if (enumFilter == (BO.FilterWorker.ByLevel) && (filtervalue != null))
        {
            var grpByLevel = (from currentWorker in _dal.Worker.ReadAll()
                              group currentWorker.Id by currentWorker.Level into grp
                              select new { level = grp.Key, workerslvl = grp });
            readByLevel = (from k in grpByLevel
                           where k.level == (DO.WorkerExperience)filtervalue
                           from b in k.workerslvl
                           select (_dal.Worker.Read(b))).ToList();

            workersInList = (from DO.Worker DoWorker in readByLevel 
                             select new BO.Worker
                             {
                                 Id = DoWorker.Id,
                                 Name = DoWorker.Name,
                                 Level = (BO.WorkerExperience)DoWorker.Level,
                                 PhoneNumber = DoWorker.PhoneNumber,
                                 Cost = DoWorker.Cost,


                                 Task = (from TaskOfWorker in _dal.Task.ReadAll(MyTask => MyTask.WorkerId == DoWorker.Id)
                                         select new TaskInWorker
                                         {
                                             Id = TaskOfWorker.Id,
                                             Alias = TaskOfWorker.Alias
                                         }).FirstOrDefault()!,
                                 active = DoWorker.active

                             }).ToList();
            return workersInList;

        }
        else
        {
            if (filtervalue == null && enumFilter == (BO.FilterWorker.ByLevel))
                enumFilter = FilterWorker.None;


            IEnumerable<DO.Worker?> result =
       enumFilter switch
       {
           BO.FilterWorker.WithoutTask => (_dal.Worker.ReadAll(MyWorker => (WorkerDoesntHaveTask(MyWorker.Id) == true))),
           BO.FilterWorker.ActiveW => _dal.Worker.ReadAll(d => d.active == true),
           BO.FilterWorker.Erasable => _dal.Worker.ReadAll(d => d.Eraseable == true),
           BO.FilterWorker.None => _dal.Worker.ReadAll()


       };




                   workersInList = (from DO.Worker DoWorker in result
                                  select new BO.Worker
                                  {
                                      Id = DoWorker.Id,
                                      Name = DoWorker.Name,
                                      Level = (BO.WorkerExperience)DoWorker.Level,
                                      PhoneNumber = DoWorker.PhoneNumber,
                                      Cost = DoWorker.Cost,


                                      Task = (from TaskOfWorker in _dal.Task.ReadAll(MyTask => MyTask.WorkerId == DoWorker.Id)
                                              select new TaskInWorker
                                              {
                                                  Id = TaskOfWorker.Id,
                                                  Alias = TaskOfWorker.Alias
                                              }).FirstOrDefault()!,
                                      active = DoWorker.active

                                  }).ToList();
            return workersInList;
        }
        
    }

    public BO.Worker? ReadWorker(int Id)
    {

        BO.Worker boworker;
        try
        {
            DO.Worker doworker = _dal.Worker.Read(Id,true);

            boworker = new BO.Worker { Id = Id, Name = doworker.Name };
            boworker.Level = (BO.WorkerExperience)doworker.Level;
            boworker.PhoneNumber = doworker.PhoneNumber;
            boworker.Cost = doworker.Cost;
            boworker.active= doworker.active;
       
            TaskInWorker Task  = (from TaskOfWorker in _dal.Task.ReadAll(MyTask => MyTask.WorkerId == doworker.Id)
                                      select new TaskInWorker
                                      {
                                          Id = TaskOfWorker.Id,
                                          Alias = TaskOfWorker.Alias
                                      }).FirstOrDefault()!;
          
           
        }
        catch(DO.DalDoesNotExistException ex)
        {
            throw new BO.BlDoesNotExistException($"Worker with ID={Id} does Not exist", ex);
        }
        return boworker;
    }



    public void RemoveWorker(int Id)
    {
        if (GetStatusOfProject() != BO.ProjectStatus.ScheduleDetermination)
        {
            DO.Worker? DoWorkerToRemove;
            try
            {
                DoWorkerToRemove = _dal.Worker.Read(Id, true);

                if (DoWorkerToRemove.active == false)
                    throw new BO.BlNotActiveException($"Worker with ID={Id} does Not Active");

               var taskOfWorker = (from task in _dal.Task.ReadAll(MyTask => MyTask.WorkerId == DoWorkerToRemove!.Id)
                                   where (task.CompleteDate!=null||task.StartDate!=null)
                                        select task).ToList();
                if (taskOfWorker.Count() == 0)//if the worker started a task and finished it or didnt start yet the task
                    _dal.Worker.Delete(Id);

                else 
                    throw new BO.BlNotErasableException($"Worker with ID={Id} does Not Erasable because he completed a Task or he is working on a task");

                



            }

            catch (DO.DalNotErasableException ex)
            {
                throw new BO.BlNotErasableException($"Worker with ID={Id} does Not Erasable", ex);
            }

            catch (DO.DalDoesNotExistException ex)
            {
                throw new BO.BlDoesNotExistException($"Worker with ID={Id} does Not exist", ex);
            }
        }

    }

    public void UpdateWorker(BO.Worker workerToUpdate)
    {

        DO.Worker doWorker = new DO.Worker(workerToUpdate.Id, (DO.WorkerExperience)workerToUpdate.Level, workerToUpdate.Name,
            workerToUpdate.PhoneNumber, workerToUpdate.Cost);
        int TaskToUp = workerToUpdate.Task.Id;
        if ((int)workerToUpdate.Level == 2)
            doWorker.Eraseable = true;

        //if(doWorker.active == false)
        //    throw new BlNotActiveException($"Worker with id={workerToUpdate.Id} is not active");

        string[] phonePrefix = { "050", "051", "052", "053", "054", "055 ", "056", "058" };

        string prefixOfPhoneNumber = doWorker.PhoneNumber[0].ToString() + doWorker.PhoneNumber[1].ToString() + doWorker.PhoneNumber[2].ToString();
        try
        {
            if (_dal.Task.Read(TaskToUp) == null)
                throw new BO.BlInvalidGivenValueException($"One of the data of Worker with ID={doWorker.Id} is incorrect, Task with id={TaskToUp} of worker does not exsists");
            if (doWorker.Id > 0 && (doWorker.Id.ToString().Length == 9) && doWorker.Name.Length > 0 &&
                doWorker.Cost > 0 && doWorker.PhoneNumber.Length == 10 && phonePrefix.Contains(prefixOfPhoneNumber))
            {

                _dal.Worker.Update(doWorker);
                if (GetStatusOfProject() == BO.ProjectStatus.ExecutionStage)
                { 
                    DO.Task taskWithUpdateWorker = _dal.Task.Read(TaskToUp) with { WorkerId = workerToUpdate.Id };
                    _dal.Task.Update(taskWithUpdateWorker);
                }
            }
            else
                throw new BO.BlInvalidGivenValueException($"One of the data of Worker with ID={doWorker.Id} is incorrect");

        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BO.BlDoesNotExistException($"Worker with ID={workerToUpdate.Id} does Not exist", ex);

        }
        catch (DO.DalNotActiveException ex)
        {
            throw new BO.BlNotActiveException($"Worker with ID={workerToUpdate.Id} is Not active", ex);

        }

    }

    public void deleteAll()
    {
        if(GetStatusOfProject() != BO.ProjectStatus.ScheduleDetermination)
            _dal.Worker.DeleteAll();
    }
}


