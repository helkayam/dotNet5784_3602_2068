namespace BlImplementation;
using BlApi;
using BO;
using DalApi;
//using DO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

internal class TaskImplementation : BlApi.ITask
{
    private DalApi.IDal _dal = DalApi.Factory.Get;



    //public ProjectStatus ProjectStatus()
    //{
        
    //    if (StartDateProject == null)
    //        return BO.ProjectStatus.PlanStage;
    //    else

    //    if (StartDateProject != null)
    //    {
    //        var withoutDate = (from tasks in _dal.Task.ReadAll()


    //    }

    //}
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
        if (GetStatusOfProject() != BO.ProjectStatus.PlanStage)//fix it 
            throw new BO.BlForbiddenActionException("The project status cant allow to add Task");


        //DO.Task DoTask = new DO.Task(newTask.Alias, (DO.WorkerExperience)(newTask.Complexity), newTask.Description, newTask.Id, newTask.ScheduledDate, newTask.Deadline);
        DO.Task DoTask;
            DoTask = new DO.Task(newTask.Alias, (DO.WorkerExperience)(newTask.Complexity), newTask.Description, newTask.Id);


  
            DoTask.RequiredEffortTime = newTask.RequiredEffortTime;
        DoTask.Eraseable = newTask.Eraseable;
        //if (_dal.Worker.Read(newTask.Worker.Id) == null)
        //    throw new BO.BlDoesNotExistException($"The worker assigned to the task doesnt exist");

        //add dependencies
        var item = from BoDep in newTask.Dependencies
                   let id = newTask.Id
                   select new DO.Dependency { DependentTask = BoDep.Id, DependsOnTask = id };
        foreach (var dep in item)
        {
            if(_dal.Task.Read(dep.DependentTask)!=null)
            _dal.Dependency.Create(dep);
        }

        try
        {
            if (DoTask.Alias.Length > 0 && DoTask.Id >= 0)
                _dal.Task.Create(DoTask);
            else
                throw new BO.BlInvalidGivenValueException($"One of the data of Task with ID={DoTask.Id} is incorrect");

        }
        catch (DO.DalAlreadyExistException ex)
        {
            throw new BO.BlAlreadyExistsException($"Task with ID={newTask.Id} already exists", ex);
        }
    


    }

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

    public DateTime? GetForecastDate(DO.Task DoTask)
    {
        if (DoTask.StartDate < DoTask.ScheduledDate)
            return DoTask.ScheduledDate + DoTask.RequiredEffortTime;

        return DoTask.StartDate + DoTask.RequiredEffortTime;
    }
    public IEnumerable<BO.TaskInList?> ReadAllTasks(BO.Filter enumFilter = BO.Filter.None, Object? filtervalue = null)
    {

        IEnumerable<DO.Task?> result =
       enumFilter switch
       {
           BO.Filter.ByComplexity => ((DO.WorkerExperience)filtervalue != null) ? _dal.Task.ReadAll(bc => bc.Complexity == (DO.WorkerExperience)filtervalue) : _dal.Task.ReadAll(),
            BO.Filter.Status=> ((BO.Status)filtervalue != null) ? _dal.Task.ReadAll(s => getStatus(s) == (BO.Status)filtervalue) : _dal.Task.ReadAll(),
            BO.Filter.PossibleTaskForWorker => ((DO.WorkerExperience)filtervalue != null) ? _dal.Task.ReadAll(bc => (bc.Complexity <= (DO.WorkerExperience)filtervalue && bc.StartDate!=null&&checkDependentTaskDone(bc)==true)) : _dal.Task.ReadAll(),
         
           BO.Filter.None => _dal.Task.ReadAll(),

       };

      return  result.Select(dotask => new BO.TaskInList()
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
            DO.Task DoTask = _dal.Task.Read(Id,true);
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

            //IEnumerable < TaskInList > dep = _dal.Dependency.ReadAll().Where(item => item.DependentTask == Id).
            //    Select(item => new BO.TaskInList()
            //    {
            //        Id = item.DependsOnTask,
            //        Description = (_dal.Task.Read(item.DependsOnTask)).Description,
            //        Alias = (_dal.Task.Read(item.DependsOnTask)).Alias,
            //        Status = getStatus(_dal.Task.Read(item.DependsOnTask))
            //    });
           
          BoTask.Dependencies=new List<TaskInList>();
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
            throw new BO.BlDoesNotExistException($"Task with ID={Id} does Not exist");
        }


        return BoTask;


    }

    public void RemoveTask(int Id)
    {
        try
        {
            _dal.Task.Read(Id,true);
            var dep = _dal.Dependency.ReadAll().
                Where(item => item.DependsOnTask == Id).Select(item => item).ToList();


            if (dep.Count==0)
            {
                _dal.Task.Delete(Id);

                var depDel = _dal.Dependency.ReadAll().
                   Where(item => item.DependentTask == Id).Select(item => item);
                foreach (var item in depDel)
                    _dal.Dependency.Delete(item.Id);
            }
        }
        catch (DO.DalNotErasableException ex)
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

       // if((GetStatusOfProject == BO.ProjectStatus.ScheduleDetermination)

        try
        {

            DO.Task updatedTask=new DO.Task();
            if (TaskToUpdate.Id >= 0 && TaskToUpdate.Alias.Length > 0)
            {

                var TaskToUpd = _dal.Task.ReadAll().Where(item => item.Id == TaskToUpdate.Id)
                    .Select(item => item).FirstOrDefault();

                if (GetStatusOfProject == BO.ProjectStatus.ExecutionStage)
                {

                   
                    updatedTask = new DO.Task(TaskToUpdate.Alias, (DO.WorkerExperience)TaskToUpdate.Complexity, TaskToUpdate.Description, TaskToUpd.Id, TaskToUpd.ScheduledDate, TaskToUpdate.Deadline,TaskToUpdate.Worker.Id );

                   

                 

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

        catch (BO.BlDoesNotExistException ex)
        {
            throw new BO.BlDoesNotExistException($"Task with id={TaskToUpdate.Id} does not exist");
        }
    }


//    public IEnumerable <TaskInList>  GroupByStatus()
//{
      
//    var groupByStatus = (from item in _dal.Task.ReadAll()
//                         group new TaskInList { Alias = item.Alias, Description = item.Description, Id = item.Id, Status = getStatus(item) } by getStatus(item) into groupStatus
//                         select groupStatus);

//        foreach(var group in groupByStatus)
//        {
//            foreach (var item in group)
//                yield return new TaskInList { Alias = item.Alias, Description = item.Description, Id = item.Id, Status = item.Status };
//        }

    
//}

public void AddOrUpdateStartDate(int Id, DateTime? startDate)
{
     
    foreach (var item in _dal.Dependency.ReadAll())
    {
        if (item.DependentTask == Id && _dal.Task.Read(item.DependsOnTask).ScheduledDate == null)
            throw new BO.BlFalseUpdateDate($"update of start date of task with id={Id} failed because trial to update before scheduling previous task");
        if (item.DependentTask == Id&& startDate < this.ReadTask(item.DependsOnTask).ForecastDate)
            throw new BO.BlFalseUpdateDate($"update of task with id={Id} failed because trial to update the start date of task to be before forecast finishing date of  previous task");

        
    }
   var depends= from task in _dal.Dependency.ReadAll()
                where task.DependentTask == Id
                select task;

        try {
            if (depends.Count() == 0 && StartDateProject != null && startDate > StartDateProject)
            {

                DO.Task updDate = _dal.Task.Read(Id) with { StartDate = startDate };
                _dal.Task.Update(updDate);


            }
            if (depends.Count() == 0 && StartDateProject == null)
                throw new BO.BlInvalidGivenValueException($"false start date update of task: Project didnt start yet ");
            else
            if (depends.Count() == 0 && startDate < = StartDateProject)
                throw new BO.BlInvalidGivenValueException($"false start date update of task: start of task before start date project");
            else if (depends.Count() != 0)
            {
                DO.Task updDate = _dal.Task.Read(Id) with { StartDate = startDate };
                _dal.Task.Update(updDate);
            }

        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BO.BlDoesNotExistException($"Task with id={Id} does not exists", ex);
        }
       

}

    public void deleteAll()
    {
        _dal.Task.DeleteAll();
        _dal.Dependency.DeleteAll();
    }

   
}

