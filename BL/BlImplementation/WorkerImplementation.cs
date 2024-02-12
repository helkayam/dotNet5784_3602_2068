namespace BlImplementation;
using BlApi;
using BO;
using DalApi;
using DO;
using System;
using System.Buffers.Text;
using System.Collections.Generic;

using System.Reflection.Emit;
using System.Reflection.Metadata;


/// <summary>
/// This is a class that implements the methods of the Worker logical entity by implementing the IWorker interface
/// </summary>
internal class WorkerImplementation : BlApi.IWorker
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
/// <summary>
/// A method for creating a new task, the method will allow you to add a task if the project status is the planning stage or the execution stage, 
/// and if all the received data is correct.
/// If all the following conditions are met, the method will send to the DAL layer a request to add a task with
/// a parameter of a DO entity of a task it created based on a BO entity received as a parameter
/// </summary>
/// <param name="newWorker"> BO entity of task to add</param>
/// <exception cref="BO.BlInvalidGivenValueException"> An exception that is thrown if one of the entity's data is incorrect </exception>
/// <exception cref="BO.BlAlreadyExistsException"> An exception that is thrown if an attempt is made to add a task that already exists</exception>
public void AddWorker(BO.Worker newWorker)
    {

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

    /// <summary>
    /// A method that checks for a certain employee whether he has a task assigned to him
    /// </summary>
    /// <param name="id"> id of worker that we check it for him </param>
    /// <returns>True- if the employee has not been assigned a task
   /// False- if the employee has been assigned a task</returns>
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
    /// <summary>
    /// The method returns a collection of workers according to a certain filter:
    /// *All workers who have not been assigned a task
    /// *All active workers
    /// * All erasable workers
    /// *All the workers without any filtering
    /// </summary>
    /// <param name="enumFilter">The filter by which the collection of returned workers is built</param>
    /// <param name="filtervalue">In the filters of workers By level you need to know what level to return and therefore this parameter allows us to know what level of filtering is requested. </param>
    /// <returns> A collection of Workers by filtering </returns>
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
    /// <summary>
    /// The method receives an ID of a worker and returns a BO entity of a worker after creating this entity according to certain calculations and requests from the DAL layer
    /// </summary>
    /// <param name="Id"> ID of worker to read </param>
    /// <returns> BO entity of a worker </returns>
    /// <exception cref="BO.BlDoesNotExistException"> An exception that is thrown if the worker you want to read does not exist </exception>
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


    /// <summary>
    /// this method gets an id of a worker 
    /// and delete it 
    /// </summary>
    /// <param name="Id">Id of the worker</param>
    /// <exception cref="BO.BlNotActiveException">if the worker is not active, we throw an exception</exception>
    /// <exception cref="BO.BlNotErasableException">if the workers is not erasable, we throw an exception</exception>
    /// <exception cref="BO.BlDoesNotExistException">if the worker does not exist, we throw an excpetion</exception>
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
    /// <summary>
    /// This is a method for updating employee details. 
    /// If we are in the planning phase of the project, only technical details of the list can be updated.
    /// If we are in the schedule phase, then there is no possibility to update, 
    /// if we are in the execution phase, there is a possibility to update technical details, as well as to assign a task to an employee
    /// </summary>
    /// <param name="workerToUpdate"> Updated employee BO entity </param>
    /// <exception cref="BO.BlInvalidGivenValueException"> An exception that is thrown if one of the data to be updated, of the entity of the received employee, is incorrect </exception>
    /// <exception cref="BO.BlDoesNotExistException">An exception that is thrown if the worker you want to update does not exist </exception>
    /// <exception cref="BO.BlNotActiveException"> An exception that is thrown if you try to update an inactive employee </exception>
    public void UpdateWorker(BO.Worker workerToUpdate)
    {

        DO.Worker doWorker = new DO.Worker(workerToUpdate.Id, (DO.WorkerExperience)workerToUpdate.Level, workerToUpdate.Name,
            workerToUpdate.PhoneNumber, workerToUpdate.Cost);
       
        if ((int)workerToUpdate.Level == 2)
            doWorker.Eraseable = false;
        else
            doWorker.Eraseable = true;

        //if(doWorker.active == false)
        //    throw new BlNotActiveException($"Worker with id={workerToUpdate.Id} is not active");

        string[] phonePrefix = { "050", "051", "052", "053", "054", "055 ", "056", "058" };

        string prefixOfPhoneNumber = doWorker.PhoneNumber[0].ToString() + doWorker.PhoneNumber[1].ToString() + doWorker.PhoneNumber[2].ToString();
        try
        {
            
            if (doWorker.Id > 0 && (doWorker.Id.ToString().Length == 9) && doWorker.Name.Length > 0 &&
                doWorker.Cost > 0 && doWorker.PhoneNumber.Length == 10 && phonePrefix.Contains(prefixOfPhoneNumber))
            {

                _dal.Worker.Update(doWorker);
                if (GetStatusOfProject() == BO.ProjectStatus.ExecutionStage)
                {

                    int TaskToUp = workerToUpdate.Task.Id;
                    if (_dal.Task.Read(TaskToUp) == null)
                        throw new BO.BlInvalidGivenValueException($"One of the data of Worker with ID={doWorker.Id} is incorrect, Task with id={TaskToUp} of worker does not exsists");
                    if((BO.WorkerExperience)_dal.Task.Read(TaskToUp).Complexity>workerToUpdate.Level)
                        throw new BO.BlInvalidGivenValueException($"One of the data of Worker with ID={doWorker.Id} is incorrect, Task with id={TaskToUp} is not appropriate for the worker level");
                    if (WorkerDoesntHaveTask(doWorker.Id)==false)
                        throw new BO.BlInvalidGivenValueException($"One of the data of Worker with ID={doWorker.Id} is incorrect, this worker is already on a task");
                    if (_dal.Task.Read(workerToUpdate.Task.Id).WorkerId!=null)
                        throw new BO.BlInvalidGivenValueException($"One of the data of Worker with ID={doWorker.Id} is incorrect, the task have already a worker that is working on it");


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

    /// <summary>
    /// The method deletes all employees in the database, of course only if we are not currently in the planning phase
    /// </summary>
    public void deleteAll()
    {
        if(GetStatusOfProject() != BO.ProjectStatus.ScheduleDetermination)
            _dal.Worker.DeleteAll();
    }
}


