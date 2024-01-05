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
                throw new Exception( $"Worker with ID={item.Id} already exist");
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
                if (!x.Eraseable)
                    throw new Exception("Can't delete the Worker");
                if (x.active == false)
                {
                    Worker newW = x with { active = false };
                    DataSource.Workers.Remove(x);
                    DataSource.Workers.Add(newW);
                    return;
                }
                else
                    DataSource.Workers.Remove(x);
            }
        }
        throw new Exception($"Worker with ID={id} does not exist");

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
        throw new Exception($"Worker with ID={item.Id} does not exist");

    }
}
