namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;

public class WorkerImplementation : IWorker
{
    public int Create(Worker item)
    {
        foreach(var x in DataSource.Workers)
        {
            if (x.Id == item.Id)
                throw new NotImplementedException( "object of tipe worker with this id already exist");
        }
        DataSource.Workers.Add(item);  
        return item.Id;
    }

    public void Delete(int id)
    {
        foreach (var x in DataSource.Workers)
        {
            if (x.Id == id)
            {
                if (!x.eraseAbale)
                    throw new NotImplementedException("cant delete the object ");
                Worker newW = x with { active = false };
                DataSource.Workers.Remove(x);
                DataSource.Workers.Add(newW);
                return;
            }
        }
        throw new NotImplementedException("object of tipe worker with this id not exist");

    }

    public Worker? Read(int id)
    {
        foreach (var x in DataSource.Workers)
        {
            if (x.Id == id)
                return x;
        }
        return null;

    }

    public List<Worker> ReadAll()
    {
        return new List<Worker>(DataSource.Workers);
    }

    public void Update(Worker item)
    {
        foreach (var x in DataSource.Workers)
        {
            if (x.Id == item.Id)
            {
                DataSource.Workers.Remove(item);
                DataSource.Workers.Add(item);
                return;
            }
        }
        throw new NotImplementedException("object of tipe worker with this id already exist");

    }
}
