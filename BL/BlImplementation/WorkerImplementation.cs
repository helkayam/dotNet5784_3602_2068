

namespace BlImplementation;
using BlApi;
using BO;
using System;
using System.Collections.Generic;

internal class WorkerImplementation :IWorker
{
    private DalApi.IDal _dal = DalApi.Factory.Get;
    public void AddWorker(Worker newWorker)
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
        }
        catch(DO.DalAlreadyExistException ex)
        {
            //throw new BO.BlAlreadyExistsException($"Student with ID={boStudent.Id} already exists", ex);
        }

    }

    public IEnumerable<WorkerInList?> ReadAllWorkers(Func<Worker, bool>? filter = null)
    {
     
        var workerList =(from DO.Worker item in _dal.Worker.ReadAll()
                        // select new BO.


        
    }

    public Worker? ReadWorker(int Id)
    {
        throw new NotImplementedException();
    }

    public void RemoveWorker(int Id)
    {
        throw new NotImplementedException();
    }

    public void UpdateWorker(Worker workerToUpdate)
    {
        throw new NotImplementedException();
    }
}
