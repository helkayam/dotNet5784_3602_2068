namespace BlImplementation;
using BlApi;
using BO;
using System;
using System.Collections.Generic;

internal class WorkerImplementation :IWorker
{
    private DalApi.IDal _dal = DalApi.Factory.Get;



    public void AddWorker(BO.Worker newWorker)
    {
        DO.Worker doWorker = new DO.Worker(newWorker.Id,(DO.WorkerExperience)newWorker.Level,newWorker.Name,newWorker.PhoneNumber,newWorker.Cost);
        if((int)newWorker.Level==2)
            doWorker.Eraseable = true;  
        string[] phonePrefix = { "050", "051", "052", "053", "054","055 ","056", "058" };
       
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



    public IEnumerable<BO.WorkerInList?> ReadAllWorkers(BO.FilterWorker enumFilter = BO.FilterWorker.None, Object? filtervalue = null)
    {

        IEnumerable<DO.Worker?> result =
       enumFilter switch
       {
           BO.FilterWorker.ByLevel => (filtervalue != null) ? _dal.Worker.ReadAll(bc => bc.Level== (DO.WorkerExperience)filtervalue) : _dal.Worker.ReadAll(),
           BO.FilterWorker.None => _dal.Worker.ReadAll(),
           BO.FilterWorker.ActiveW => _dal.Worker.ReadAll(d => d.active == true),
           BO.FilterWorker.Erasable => _dal.Worker.ReadAll(d => d.Eraseable== true)

       };


        var workersInList= (from DO.Worker DoWorker in result
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
                        active=worker.active,
                        Task = (from TaskOfWorker in _dal.Task.ReadAll(MyTask => MyTask.WorkerId == worker.Id)
                                       select new TaskInWorker
                                       {
                                           Id = TaskOfWorker.Id,
                                           Alias = TaskOfWorker.Alias
                                       }).First()


                    }).FirstOrDefault();
       if(myWorker == null)
            throw new BO.BlDoesNotExistException($"Worker with ID={Id} does Not exist");
        return myWorker; 

    }



    public void RemoveWorker(int Id)
    {
        DO.Worker? DoWorkerToRemove;
        try
        {
             DoWorkerToRemove=_dal.Worker.Read(Id,true);
        }
        catch(DO.DalDoesNotExistException ex)
        {
            throw new BO.BlDoesNotExistException($"Worker with ID={Id} does Not exist");
        }
        if (DoWorkerToRemove.active == false )
            throw new BO.BlNotActiveException($"Worker with ID={Id} does Not Active");

        DO.Task taskOfWorker = (from task in _dal.Task.ReadAll(MyTask => MyTask.WorkerId == DoWorkerToRemove!.Id)
                                select task).FirstOrDefault()!;
        if (taskOfWorker.CompleteDate != null || taskOfWorker.StartDate != null)
        {
            try
            {
                _dal.Worker.Delete(Id);
            }
            catch (DO.DalNotErasableException ex)

            {
                throw new BO.BlNotErasableException($"Worker with ID={Id} does Not Erasable");
            };
        }
    }

    public void UpdateWorker(BO.Worker workerToUpdate)
    {

        DO.Worker doWorker = new DO.Worker(workerToUpdate.Id, (DO.WorkerExperience)workerToUpdate.Level, workerToUpdate.Name, 
            workerToUpdate.PhoneNumber, workerToUpdate.Cost);
        if ((int)workerToUpdate.Level == 2)
            doWorker.Eraseable = true;

        if(doWorker.active == false)
            throw new BlNotActiveException($"Worker with id={workerToUpdate.Id} is not active");

        string[] phonePrefix = { "050", "051", "052", "053", "054", "055 ", "056", "058" };

        string prefixOfPhoneNumber = doWorker.PhoneNumber[0].ToString() + doWorker.PhoneNumber[1].ToString() + doWorker.PhoneNumber[2].ToString();
        try
        {
            if (doWorker.Id > 0 && (doWorker.Id.ToString().Length == 9) && doWorker.Name.Length > 0 &&
                doWorker.Cost > 0 && doWorker.PhoneNumber.Length == 10 && phonePrefix.Contains(prefixOfPhoneNumber))
            {
                _dal.Worker.Update(doWorker);

            }
            else
                throw new BO.BlInvalidGivenValueException($"One of the data of Worker with ID={doWorker.Id} is incorrect");

        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BO.BlDoesNotExistException($"Worker with ID={workerToUpdate.Id} does Not exist");

        }

    }
}


