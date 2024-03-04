namespace BlImplementation;
using BlApi;
using BO;
using DalApi;
using DO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Threading.Tasks;

internal class TaskImplementation : BlApi.ITask
{
    /// <summary>
    /// This is a field called s_bl of the interface type of the logical layer IBl, 
    /// which is initialized with the help of the Factory class that initializes the corresponding implementation class within the BL project(DALXML/DALLIST).
    /// </summary>
    private DalApi.IDal _dal = DalApi.Factory.Get;

    private readonly Bl _bl;
    internal TaskImplementation(Bl bl) => _bl = bl;

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

    /// <summary>
    /// This is a method that takes care of returning the status of the project.
    /// Return a status of the planning phase if the project start date has not been updated
    ///Return the status of the schedule step if the project start date has been updated but the schedule update of all tasks has not finished
    ///You will return the status of the execution phase if the schedule and also the project start date are available
    /// </summary>
    /// <returns> Return the project status: Plan Stage/Schedule Determination/Execution Stage </returns>
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


            if (withoutDate.Count ==0)
                return BO.ProjectStatus.ExecutionStage;

        }

        return BO.ProjectStatus.ScheduleDetermination;

    }
    /// <summary>
    /// Receives a task and returns its status: task schedule not updated / task schedule updated / task in progress / task completed
    /// </summary>
    /// <param name="task"> A task received and you want to know its status </param>
    /// <returns> Task status: Unscheduled/Scheduled/OnTrack/Done</returns>
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
    /// <summary>
    /// The method receives a BO entity of a Task and takes care of generating a corresponding DO entity and sends it to the DAL layer 
    /// which will take care of generating a new task and adding it to the DATABASE.
    ///The method will allow you to add the task only if the project status is a status of the planning phase
    /// </summary>
    /// <param name="newTask"> A task type entity received from the user, to be added </param>
    /// <exception cref="BO.BlForbiddenActionException"> Exception for an attempt to add in a phase of the project that does not allow it </exception>
    /// <exception cref="BO.BlInvalidGivenValueException"> An exception for a case where one of the mission data is incorrect or does not match expectations </exception>
    /// <exception cref="BO.BlAlreadyExistsException"> Exception for the case of adding an already existing task </exception>
    public void AddTask(BO.Task newTask)
    {
        if (GetStatusOfProject() != BO.ProjectStatus.PlanStage)
            throw new BO.BlForbiddenActionException("The project status cant allow to add Task");

        DO.Task DoTask;
        DoTask = new DO.Task(newTask.Alias, (DO.WorkerExperience)(newTask.Complexity), newTask.Description, newTask.Id);


        DoTask.RequiredEffortTime = newTask.RequiredEffortTime;

        DoTask.Eraseable = newTask.Eraseable;
      

      
            if (DoTask.Alias.Length > 0 )
            {
               int IdOfNewTask= _dal.Task.Create(DoTask);
            if (newTask.Dependencies != null)
            {
                var item = from BoDep in newTask.Dependencies
                           let id = IdOfNewTask
                           select new DO.Dependency { DependentTask = id, DependsOnTask = BoDep.Id };



                bool contradictionBetweenDependencies = false;
                foreach (var dep in newTask.Dependencies)
                {
                    int myId = dep.Id;
                    DO.Task d = _dal.Task.Read(dep.Id);
                    DO.Dependency newDep = new DO.Dependency(IdOfNewTask, dep.Id);

                    if (d != null)
                    {

                        _dal.Dependency.Create(newDep);
                    }
                }
            }
            }
            else
                throw new BO.BlInvalidGivenValueException($"One of the data of Task with ID={DoTask.Id} is incorrect");

        }
     




    /// <summary>
    /// The method receives a task and checks whether all the tasks that the received task depends on have finished.
    /// If so, return true
    /// otherwise return false.
    /// </summary>
    /// <param name="newTask"> A task for which it is checked whether the tasks it depends on have been completed  </param>
    /// <returns> true/false </returns>
    public bool checkDependentTaskDone(DO.Task newTask)
    {
        var previousTask = from myDep in _dal.Dependency.ReadAll()
                           where myDep.DependentTask == newTask.Id
                           select myDep.DependsOnTask;

        bool isDone = true;
        foreach (var prevTask in previousTask)
        {
            if (getStatus(_dal.Task.Read(prevTask)) != BO.Status.Done)
                isDone = false;

        }

        return isDone;


    }

    /// <summary>
    /// The method accepts a task and returns estimated time to completion of task
    /// </summary>
    /// <param name="DoTask"> A BO entity of a task for which the estimated time to completion is returned </param>
    /// <returns> estimated time to completion </returns>
    public DateTime? GetForecastDate(DO.Task DoTask)
    {
        if (DoTask.StartDate < DoTask.ScheduledDate)
            return DoTask.ScheduledDate + DoTask.RequiredEffortTime;

        return DoTask.StartDate + DoTask.RequiredEffortTime;
    }

    public bool FindIdContains(DO.Task t,string id)
    {
        int TaskId = t.Id;
        string IdStr = $"{TaskId}";
        if (IdStr.Contains(id))
            return true;
        return false;


        
    }
    public IEnumerable<BO.TaskInList> ReadAllSearch(string search)
    {
        string searchLower=search.ToLower();
        IEnumerable<DO.Task?> result = _dal.Task.ReadAll(ts => (ts.Alias.ToLower ()).Contains(searchLower) || FindIdContains(ts, search) == true ||(ts.Description.ToLower()).Contains(searchLower));
        return result.Select(dotask => new BO.TaskInList()
        {
            Alias = dotask.Alias,
            Id = dotask.Id,
            Description = dotask.Description,
            Status = getStatus(dotask)
        });

    }


    /// <summary>
    /// The method returns a collection of tasks according to a certain filter:
    /// *All tasks with the same level of complexity
    /// *All tasks with the same task status
    /// * All possible tasks that an employee can start performing
    /// *All the tasks without any filtering
    /// </summary>
    /// <param name="enumFilter"> The filter by which the collection of returned tasks is built </param>
    /// <param name="filtervalue"> In the filters of Tasks By Complexity/Status of task/PossibleTaskForWorker you need to know what level to return and therefore this parameter allows us to know what level of filtering is requested. </param>
    /// <returns> A collection of tasks by filtering </returns>
    public IEnumerable<BO.TaskInList> ReadAllTasks(BO.Filter enumFilter = BO.Filter.None, Object? filtervalue = null)
    {

        IEnumerable<DO.Task?> result =
       enumFilter switch
       {
           BO.Filter.ByComplexity => ((DO.WorkerExperience)filtervalue != null) ? _dal.Task.ReadAll(bc => bc.Complexity == (DO.WorkerExperience)filtervalue) : _dal.Task.ReadAll(),
           BO.Filter.Status => ((BO.Status)filtervalue != null) ? _dal.Task.ReadAll(s => getStatus(s) == (BO.Status)filtervalue) : _dal.Task.ReadAll(),
           BO.Filter.PossibleTaskForWorker => ((DO.WorkerExperience)filtervalue != null) ? _dal.Task.ReadAll(bc => (bc.Complexity<= (DO.WorkerExperience)filtervalue && bc.ScheduledDate!= null && checkDependentTaskDone(bc) == true)) : _dal.Task.ReadAll(),
           BO.Filter.None => _dal.Task.ReadAll(),

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

    /// <summary>
    /// The method receives an ID of a task and returns a BO entity of a task after creating this entity according to certain calculations and requests from the DAL layer
    /// </summary>
    /// <param name="Id"> id of task to read </param>
    /// <returns> BO entity of a task </returns>
    /// <exception cref="BO.BlDoesNotExistException"> An exception that is thrown if the task you want to read does not exist </exception>
    public BO.Task? ReadTask(int Id, bool throwexception = false )
    {

        BO.Task BoTask = new BO.Task();
        try
        {
            DO.Task DoTask = _dal.Task.Read(Id);

            if(DoTask == null);
            { if (throwexception == true)
                    _dal.Task.Read(Id, true);
                else
                    return null;                    }

            BoTask.Id = DoTask.Id;
            BoTask.Description = DoTask.Description;
            BoTask.Alias = DoTask.Alias;
            BoTask.CreatedAtDate = DoTask.CreatedAtDate;
            BoTask.Status = getStatus(DoTask);
            BoTask.Eraseable = DoTask.Eraseable;


            BoTask.RequiredEffortTime = DoTask.RequiredEffortTime;
            BoTask.ScheduledDate = DoTask.ScheduledDate;
            BoTask.StartDate = DoTask.StartDate;

            BoTask.ForecastDate = GetForecastDate(DoTask);

            BoTask.Deadline = DoTask.DeadLineDate;
            BoTask.CompleteDate = DoTask.CompleteDate;
            BoTask.Deliverables = DoTask.Deliverables;
            BoTask.Remarks = DoTask.Remarks;
            BoTask.Complexity = (BO.WorkerExperience)(DoTask.Complexity);

            //get the list of dependencies and check where this task is dependent on another task

            var dep = from item in _dal.Dependency.ReadAll()
                      where item.DependentTask == Id
                      select new TaskInList
                      {
                          Id = item.DependsOnTask,
                          Description = (_dal.Task.Read(item.DependsOnTask)).Description,
                          Alias = (_dal.Task.Read(item.DependsOnTask)).Alias,
                          Status = getStatus(_dal.Task.Read(item.DependsOnTask))
                      };

            

            BoTask.Dependencies = new List<TaskInList>();
            foreach (var item in dep)
                BoTask.Dependencies.Add(item);





            //worker...
            int? idWorker = DoTask.WorkerId;
            if (idWorker != null)
            {
                BoTask.Worker = new WorkerInTask();
                BoTask.Worker.Id = (int)idWorker;
                BoTask.Worker.Name = (_dal.Worker.Read(BoTask.Worker.Id)!).Name;
            }






        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BO.BlDoesNotExistException($"Task with ID={Id} does Not exist",ex);
        }


        return BoTask;


    }

    /// <summary>
    /// The method receives an ID of a task to delete.
    /// It allows deletion of the task if the project status is the planning stage, and if no task is dependent on this task.
    /// If all the following conditions are met then it sends a deletion request to the DAL layer
    /// </summary>
    /// <param name="Id"> ID of task to remove</param>
    /// <exception cref="BO.BlNotErasableException"> An exception that is thrown if an attempt was made to delete a task that is prohibited by deletion </exception>
    /// <exception cref="BO.BlDoesNotExistException"> An exception thrown if an attempt was made to delete a task that does not exist  </exception>
    public void RemoveTask(int Id)
    {
        if (GetStatusOfProject() == BO.ProjectStatus.PlanStage)
        {
            try
            {
                _dal.Task.Read(Id, true);
                var dep = _dal.Dependency.ReadAll().
                    Where(item => item.DependsOnTask == Id).Select(item => item).ToList();


                if (dep.Count == 0)
                {
                    _dal.Task.Delete(Id);

                    var depDel = _dal.Dependency.ReadAll().
                       Where(item => item.DependentTask == Id).Select(item => item);
                    foreach (var item in depDel)
                        _dal.Dependency.Delete(item.Id);
                }
                else
                    throw new BO.BlInvalidGivenValueException("Can't delete the task because there is a task that depends on that task ");
            }
            catch (DO.DalNotErasableException ex)
            {
                throw new BO.BlNotErasableException($"Task with ID={Id} does Not Erasable",ex);

            }

            catch (DO.DalDoesNotExistException ex)
            {
                throw new BO.BlDoesNotExistException($"Task with ID={Id} does Not exist",ex);
            }
        }
    }


    public bool WorkerDoesntHaveTask(int id)
    {
        var Tasks = (from currentTask in _dal.Task.ReadAll()
                     where currentTask.WorkerId == id
                     select currentTask).ToList();
        if (Tasks.Count() != 0)
            return false;
        else
            return true;
    }

    /// <summary>
    /// this method gets a BO task entity and if we are not in schedule determination stage, we can update the task.
    /// we create a do entity according to the bo we got. (according to the stage, we will add just the relevant fields, for example in firsst stage (plan stage)
    /// we dont allow to update all date fields, and also deliverables ) and in stage 3 we allow it ...
    /// and then we send a update request to the DAL Layer.
    /// </summary>
    /// <param name="TaskToUpdate">Task updated with relevant fields</param>
    /// <exception cref="BO.BlInvalidGivenValueException">if the BO task we got has invalid fields (like id<0 or alias of task null) we throw this exception</exception>
    /// <exception cref="BO.BlDoesNotExistException">if the task we are trying to update doesnt exist we throw an error</exception>

    public void UpdateTask(BO.Task TaskToUpdate)
    {

      
       
        if(GetStatusOfProject()!=BO.ProjectStatus.ScheduleDetermination)
            try
            {

                DO.Task updatedTask = new DO.Task();
                if (TaskToUpdate.Id >= 0 && TaskToUpdate.Alias.Length > 0)
                {

                    var TaskToUpd = _dal.Task.Read(TaskToUpdate.Id, true);

                    if (GetStatusOfProject() == BO.ProjectStatus.ExecutionStage)
                    {


                        updatedTask = new DO.Task(TaskToUpdate.Alias, (DO.WorkerExperience)TaskToUpdate.Complexity, TaskToUpdate.Description, TaskToUpd.Id, TaskToUpd.ScheduledDate, TaskToUpdate.Deadline);


                        if(TaskToUpdate.Worker.Id != null)
                        {
                            _dal.Worker.Read(TaskToUpdate.Worker.Id, true);
                            if ( WorkerDoesntHaveTask(TaskToUpdate.Worker.Id) == false)
                                throw new BO.BlInvalidGivenValueException($"One of the data of Task with ID={TaskToUpdate.Id} is incorrect, the worker={TaskToUpdate.Worker.Id} is already on a task");
                            updatedTask=updatedTask with { WorkerId = TaskToUpdate.Worker.Id };

                        }
                      





                        updatedTask.IsMileStone = TaskToUpd.IsMileStone;

                        updatedTask.Remarks = TaskToUpdate.Remarks;//1 3 
                        updatedTask.CreatedAtDate = TaskToUpd.CreatedAtDate;//1 3 


                        updatedTask.CompleteDate = TaskToUpd.CompleteDate;//3
                        updatedTask.Deliverables = TaskToUpdate.Deliverables;//3
                        updatedTask.StartDate = TaskToUpd.StartDate;//3







                    }
                    else//1 
                    {
                        updatedTask = new DO.Task(TaskToUpdate.Alias, (DO.WorkerExperience)TaskToUpdate.Complexity, TaskToUpdate.Description, TaskToUpdate.Id);
                        updatedTask.IsMileStone = TaskToUpd.IsMileStone;
                        updatedTask.Eraseable = TaskToUpdate.Eraseable;
                        updatedTask.Remarks = TaskToUpdate.Remarks;
                        updatedTask.CreatedAtDate = TaskToUpd.CreatedAtDate;
                        updatedTask.RequiredEffortTime = TaskToUpdate.RequiredEffortTime;
                        bool contradictionBetweenDependencies = false;
                        foreach (var item in TaskToUpdate.Dependencies)
                        {
                            int myId = item.Id;
                            bool DepDoesntExist = true;
                            DO.Task d = _dal.Task.Read(item.Id);
                            if (d != null)
                            {
                                
                                DO.Dependency newDep = new DO.Dependency(TaskToUpdate.Id, item.Id);
                                foreach (var CurrentDependency in _dal.Dependency.ReadAll())
                                {
                                    if (CurrentDependency.DependentTask == newDep.DependentTask && CurrentDependency.DependsOnTask == newDep.DependsOnTask)
                                        DepDoesntExist = false;
                                    if (CurrentDependency.DependentTask == newDep.DependsOnTask && CurrentDependency.DependsOnTask == newDep.DependentTask)
                                        contradictionBetweenDependencies = true;


                                }

                                if (DepDoesntExist == true && contradictionBetweenDependencies == false)
                                    _dal.Dependency.Create(newDep);
                            }
                        }

                    }




                }


                else
                    throw new BO.BlInvalidGivenValueException($"One of the data of the Updated Task is incorrect");
                _dal.Task.Update(updatedTask);

            }

            catch (DO.DalDoesNotExistException ex)
            {
                throw new BO.BlDoesNotExistException($"Task with id={TaskToUpdate.Id} does not exist",ex);
            }
        
    }

    public void AutomaticSchedule()
    {
           
    }

    /// <summary>
    /// this method gets an Id of a task, and a date
    /// and update that task with update date 
    /// </summary>
    /// <param name="Id">Id of the task</param>
    /// <param name="mySceduelDate">schedule date we want to add to the task</param>
    /// <exception cref="BO.BlFalseUpdateDate">throw excpetion if there is a problem with the date (schedule date before start project date or before scheduling the previous tasks or before forecast date of previous tasks...)</exception>
    /// <exception cref="BO.BlInvalidGivenValueException">update date before updating schedule date of project</exception>
    /// <exception cref="BO.BlDoesNotExistException">if we schedule a task that doesnt exist we throw an exception</exception>
    public void UpdateScheduleDate(int Id, DateTime mySceduelDate)
    {

        foreach (var item in _dal.Dependency.ReadAll())
        {
            if (item.DependentTask == Id && _dal.Task.Read(item.DependsOnTask).ScheduledDate == null)
                throw new BO.BlFalseUpdateDate($"update of start date of task with id={Id} failed because trial to update before scheduling previous task");
            if (item.DependentTask == Id && mySceduelDate < this.ReadTask(item.DependsOnTask).ForecastDate)
                throw new BO.BlFalseUpdateDate($"update of task with id={Id} failed because trial to update schedule date of task to be before forecast finishing date of  previous task");


        }
        var depends = from task in _dal.Dependency.ReadAll()
                      where task.DependentTask == Id
                      select task;

        try
        {
            if (depends.Count() == 0 && _dal.Schedule.GetStartDateProject() != null && mySceduelDate > _dal.Schedule.GetStartDateProject())
            {
                _dal.Task.Read(Id, true);
                DO.Task updDate = _dal.Task.Read(Id) with { ScheduledDate = mySceduelDate };
                _dal.Task.Update(updDate);


            }
            else
            {
                if (depends.Count() == 0 && _dal.Schedule.GetStartDateProject() == null)
                    throw new BO.BlInvalidGivenValueException($"false Schedule date update of task: Project did not start yet ");
                else
            if (depends.Count() == 0 && mySceduelDate <= _dal.Schedule.GetStartDateProject())
                    throw new BO.BlInvalidGivenValueException($"false Schedule Date date update of task: Schedule Date  before start date project");
                else if (depends.Count() != 0)
                {
                    DO.Task updDate = _dal.Task.Read(Id) with { ScheduledDate = mySceduelDate };
                    _dal.Task.Update(updDate);
                }

            }
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BO.BlDoesNotExistException($"Task with id={Id} does not exists", ex);
        }

    }

    /// <summary>
    /// this method gets an id of task and update the start date
    /// to be now.
    /// </summary>
    /// <param name="Id">Id of the task we want to update the start date</param>
    /// <exception cref="BO.BlInvalidGivenValueException">if we started this task before the scheduled date</exception>
    /// <exception cref="BO.BlDoesNotExistException">if the task we are trying to update does not exist we throw an error</exception>
    public void AddOrUpdateStartDate(int Id)
    {
        try
        {

            if (_bl.Clock < (_dal.Task.Read(Id).ScheduledDate))
                throw new BO.BlInvalidGivenValueException("false start date update of task: date before scheduled date");
            else
            {
                DO.Task updDate = _dal.Task.Read(Id) with { StartDate = _bl.Clock };
                _dal.Task.Update(updDate);
            }
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BO.BlDoesNotExistException($"Task with id={Id} does not exists", ex);

        }


    }
    /// <summary>
    /// this method gets Id of task and update the complete date to be now 
    /// </summary>
    /// <param name="Id">Id of task we want to update</param>
    /// <exception cref="BO.BlDoesNotExistException">the task does not exist</exception>
    public void AddCompleteDate(int Id)
    {

        try
        {
          DO.Task t=_dal.Task.Read(Id);
            if (t != null && t.StartDate == null)
                throw new BO.BlInvalidGivenValueException($"False update of complete date: updating a complete date before starting the task");
            DO.Task updDate = _dal.Task.Read(Id,true) with { CompleteDate = _bl.Clock };
            _dal.Task.Update(updDate);
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BO.BlDoesNotExistException($"Task with id={Id} does not exists", ex);

        }


    }
    /// <summary>
    /// this method delete all the tasks and dependencies
    /// </summary>
    public void deleteAll()
    {
        _dal.Task.DeleteAll();
        _dal.Dependency.DeleteAll();
    }

    

}

