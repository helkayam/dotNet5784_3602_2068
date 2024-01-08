namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;

internal class TaskImplementation : ITask
{
    public bool checkTask(int id)
    {
        return DataSource.Tasks.Any(wker => wker.Id == id);

    }
    public int Create(DO.Task item)
    {
        int ID;
        ID = DataSource.Config.NextTaskId;
        Task t = item with { Id = ID };
        DataSource.Tasks.Add(t);
        return ID;
    }

    public void Delete(int id)
    {
        if(Read(id)==null) 
        {
          throw new Exception($"Task with ID={id} does not exist");
        }
        DO.Task taskToRemove=Read(id);
        if(!taskToRemove.Eraseable)
        {
            throw new Exception($"Can't delete the Task");
        }
        DataSource.Tasks.RemoveAll(t=>t.Id==id);
    }

    public DO.Task? Read(int id)
    {
        if (!checkTask(id))
            return null;
        DO.Task saveItem = DataSource.Tasks.Find(saveItem => saveItem.Id == id);
       return saveItem; 
    }

    public IEnumerable<DO.Task> ReadAll()
    {
        return from task in DataSource.Tasks  
               select task; 
    }

    public void Update(DO.Task item)
    {
        
        if (DataSource.Tasks.Find(saveItem => saveItem.Id == item.Id)!=null)
        {
            DataSource.Tasks.RemoveAll(t => t.Id == item.Id);
            DataSource.Tasks.Add(item);
        }
       throw new Exception($"Task with ID={item.Id} does not exist");

    }
}
