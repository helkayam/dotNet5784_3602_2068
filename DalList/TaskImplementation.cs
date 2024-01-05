namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;

public class TaskImplementation : ITask
{
    public int Create(Task item)
    {
        int ID;
        ID = DataSource.Config.NextTaskId;
        Task t = item with { Id = ID };
        DataSource.Tasks.Add(t);
        return ID;
    }

    public void Delete(int id)
    {
        
        bool flag = false;
        foreach (var X in DataSource.Tasks)
        {
            if (X.Id == id&& (X.Eraseable==true))
            {
                DataSource.Tasks.Remove(X);
                flag = true;
            }
            if (X.Id == id && (X.Eraseable == false))
                throw new Exception($"Can't delete the Task");

        }
        if (flag == false)
            throw new Exception($"Task with ID={id} does not exist");
    }

    public Task? Read(int id)
    {
       
    
        foreach (var X in DataSource.Tasks)
        {
            if (X.Id == id)
                return X;
        }
        return null;
    }

    public List<Task> ReadAll()
    {
        return new List<Task>(DataSource.Tasks);
    }

    public void Update(Task item)
    {
        Task? t = null;
        foreach (var X in DataSource.Tasks)
        {
            if (X.Id == item.Id)
                t = X;
        }
        if (t == null)
            throw new Exception($"Task with ID={item.Id} does not exist");
        else
        {
            DataSource.Tasks.Remove(t);
            DataSource.Tasks.Add(item);
        }

    }
}
