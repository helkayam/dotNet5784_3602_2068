namespace BlImplementation;
using BlApi;
using BO;
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


    public void AddWorker(BO.Worker newWorker)
    {
        DO.Worker doWorker = new DO.Worker(newWorker.Id, (DO.WorkerExperience)newWorker.Level, newWorker.Name, newWorker.PhoneNumber, newWorker.Cost);
        if ((int)newWorker.Level == 2)
            doWorker.Eraseable = true;
        string[] phonePrefix = { "050", "051", "052", "053", "054", "055 ", "056", "058" };

        string prefixOfPhoneNumber = doWorker.PhoneNumber[0].ToString() + doWorker.PhoneNumber[1].ToString() + doWorker.PhoneNumber[2].ToString();
        try
        {
            if (doWorker.Id > 0 && (doWorker.Id.ToString().Length == 9) && doWorker.Name.Length > 0 &&
                doWorker.Cost > 0 && doWorker.PhoneNumber.Length == 10 && phonePrefix.Contains(prefixOfPhoneNumber))
            {
                _dal.Worker.Create(doWorker);

            }
            else
                throw new BO.BlInvalidGivenValueException($"One of the data of Worker with ID={doWorker.Id} is incorrect");

        }
        catch (DO.DalAlreadyExistException ex)
        {
            throw new BO.BlAlreadyExistsException($"Worker with ID={doWorker.Id} already exists", ex);
        }


    }

    public bool WorkerDoesntHaveTask(int id)
    {
        var Tasks = (from currentTask in _dal.Task.ReadAll()
                     where currentTask.WorkerId == id
                     select currentTask).ToList();
        if (Tasks != null)
            return false;
        else
            return true;
    }

    public IEnumerable<BO.WorkerInList?> ReadAllWorkers(BO.FilterWorker enumFilter = BO.FilterWorker.None, Object? filtervalue = null)
    {

        IEnumerable<DO.Worker?> result =
       enumFilter switch
       {
           BO.FilterWorker.ByLevel => ((DO.WorkerExperience)filtervalue != null) ? _dal.Worker.ReadAll(bc => bc.Level == (DO.WorkerExperience)filtervalue) : _dal.Worker.ReadAll(),
           BO.FilterWorker.WithoutTask => (_dal.Worker.ReadAll(MyWorker => (WorkerDoesntHaveTask(MyWorker.Id) == true))),
           BO.FilterWorker.ActiveW => _dal.Worker.ReadAll(d => d.active == true),
           BO.FilterWorker.Erasable => _dal.Worker.ReadAll(d => d.Eraseable == true),
           BO.FilterWorker.None => _dal.Worker.ReadAll()


       };


        var workersInList = (from DO.Worker DoWorker in result
                             select new WorkerInList
                             {
                                 Id = DoWorker.Id,
                                 Name = DoWorker.Name,
                                 Task = (from TaskOfWorker in _dal.Task.ReadAll(MyTask => MyTask.WorkerId == DoWorker.Id)
                                         select new TaskInWorker
                                         {
                                             Id = TaskOfWorker.Id,
                                             Alias = TaskOfWorker.Alias
                                         }).FirstOrDefault()!
                             }).ToList();

        return workersInList;
    }

    public BO.Worker? ReadWorker(int Id)
    {
        BO.Worker DoWorker;

        var myWorker = (from worker in _dal.Worker.ReadAll()
                        where worker.Id == Id
                        select new BO.Worker()
                        {
                            Id = worker.Id,
                            Name = worker.Name,
                            Level = (BO.WorkerExperience)worker.Level,
                            PhoneNumber = worker.PhoneNumber,
                            Cost = worker.Cost,
                            active = worker.active,
                            Task = (from TaskOfWorker in _dal.Task.ReadAll(MyTask => MyTask.WorkerId == worker.Id)
                                    select new TaskInWorker
                                    {
                                        Id = TaskOfWorker.Id,
                                        Alias = TaskOfWorker.Alias
                                    }).FirstOrDefault()


                        }).FirstOrDefault();
        if (myWorker == null)
            throw new BO.BlDoesNotExistException($"Worker with ID={Id} does Not exist");
        return myWorker; 

    }



    public void RemoveWorker(int Id)
    {
        DO.Worker? DoWorkerToRemove;
        try
        {
            DoWorkerToRemove = _dal.Worker.Read(Id, true);

            if (DoWorkerToRemove.active == false)
                throw new BO.BlNotActiveException($"Worker with ID={Id} does Not Active");

            DO.Task taskOfWorker = (from task in _dal.Task.ReadAll(MyTask => MyTask.WorkerId == DoWorkerToRemove!.Id)
                                    select task).FirstOrDefault()!;
            if (taskOfWorker.CompleteDate == null || taskOfWorker.StartDate != null)
                _dal.Worker.Delete(Id);

            else if (taskOfWorker.CompleteDate != null)
                throw new BO.BlNotErasableException($"Worker with ID={Id} does Not Erasable because he completed a Task");

            else if (taskOfWorker.StartDate == null)
                throw new BO.BlNotErasableException($"Worker with ID={Id} does Not Erasable because he started a Task");



        }

        catch (DO.DalNotErasableException ex)
        {
            throw new BO.BlNotErasableException($"Worker with ID={Id} does Not Erasable",ex);
        }

        catch (DO.DalDoesNotExistException ex)
        {
            throw new BO.BlDoesNotExistException($"Worker with ID={Id} does Not exist",ex);
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
                DO.Task taskWithUpdateWorker = _dal.Task.Read(TaskToUp) with { WorkerId = workerToUpdate.Id };
                _dal.Task.Update(taskWithUpdateWorker);
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
        _dal.Worker.DeleteAll();
    }
}


