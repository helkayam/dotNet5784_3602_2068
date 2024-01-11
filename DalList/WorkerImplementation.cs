namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;

internal class WorkerImplementation : IWorker
{
    public bool CheckWorker(int id)
    {
        return DataSource.Workers.Any(worker => worker.Id == id);
    }
    public int Create(DO.Worker item)
    {
        if (CheckWorker(item.Id))
            throw new DalAlreadyExistException($"Worker with id={item.Id} already exist");
        else
            DataSource.Workers.Add(item);
       return item.Id;
    }

    public void Delete(int id)
    {

        DO.Worker? worker = DataSource.Workers.Find(worker => worker.Id == id);
        if(worker==null)
            throw new DalDoesNotExistException($"Worker with id={id} does not exist" );
        if (worker.Eraseable==false)
            throw new DalNotErasableException($"Worker with id={id} is not eraseable");
        if (worker.active == false)
            return;

        DO.Worker w = worker with { active = false };
        DataSource.Workers.RemoveAll(wrkr => wrkr.Id == id);
        DataSource.Workers.Add(w);

    }

    public DO.Worker? Read(int id)
    {
        if (CheckWorker(id)==false)
            return null;    
        
            DO.Worker newWorker = DataSource.Workers.Find(newWorker => newWorker.Id == id);


        return newWorker;

    }

    public IEnumerable<Worker?> ReadAll(Func<Worker, bool>? filter = null)
    {
        if(filter!=null)
        {
            return from item in DataSource.Workers
                   where filter(item)
                   select item; 
        }
        return from worker in DataSource.Workers
               where worker.active==true
               select worker; 
    }

  
    public void Update(DO.Worker item)
    {
        DO.Worker w = DataSource.Workers.Find(w => w.Id == item.Id);

        if (w.active == false)
            throw new DalNotActiveException($"Worker with id={item.Id} is not active");

        else
        if (w != null)
        {
            DataSource.Workers.RemoveAll(wrkr=>wrkr.Id==item.Id);
            DataSource.Workers.Add(item);
        }
        else
            throw new DalDoesNotExistException($"Worker with id={item.Id} does not exist");
    }

    public  Worker? Read(Func<Worker, bool> filter)
    {
        var respondToFilter= from item in DataSource.Workers
               where filter(item)
               select item;

        return respondToFilter.FirstOrDefault();

    }
}
