   namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;

internal class TaskImplementation : ITask
{
    /// <summary>
    /// this method receive an id, and check wether a task with that id is in the list
    /// </summary>
    /// <param name="id">id number of the requested task</param>
    /// <returns></returns>
    public bool checkTask(int id)
    {
        return DataSource.Tasks.Any(wker => wker.Id == id);

    }

    /// <summary>
    /// this method get a Task type parameter and add it to the list.
    /// </summary>
    /// <param name="item">Task object to add to the list</param>
    /// <returns>The method returns the ID of the object it added</returns>
    public int Create(DO.Task item)
    {
        int ID;
        ID = DataSource.Config.NextTaskId;
        Task t = item with { Id = ID };
        DataSource.Tasks.Add(t);
        return ID;
    }

    /// <summary>
    /// this method receive a id, and check wether there is a task in the list with that id. if its exist and its erasable, we will delete it 
    /// </summary>
    /// <param name="id">id of the Task we want to delete </param>
    /// <exception cref="DalDoesNotExistException">if the Task doesnt exist, we will throw an exception </exception>
    /// <exception cref="DalNotErasableException">if the task is found in the list but its not erasable, we will throw an exception</exception>
    public void Delete(int id)
    {
        if(Read(id)==null) 
        {
          throw new DalDoesNotExistException($"Task with id={id} does not exist");
        }
        DO.Task taskToRemove=Read(id);
        if(!taskToRemove.Eraseable)
        {
            throw new DalNotErasableException($"Task with id={id} is not eraseable");
        }
        DataSource.Tasks.RemoveAll(t=>t.Id==id);
    }

    /// <summary>
    /// this method receive a id of a task, and return the task with that id. if we did not find a task with this id, we will return null.
    /// </summary>
    /// <param name="id">id of the Task we want to search</param>
    /// <returns></returns>
    public DO.Task? Read(int id, bool throwAnException=false)
    {
        if (!checkTask(id))
            if (throwAnException)
                throw new DalDoesNotExistException($"Task with id={id} does not exist");
            else
                return null;

                DO.Task saveItem = DataSource.Tasks.Find(saveItem => saveItem.Id == id);
       return saveItem; 

        
    }

    /// <summary>
    ///The method will receive a pointer to a boolean function, delegate of type Func, which will act on one of the members of the list of Task type and return the list of all objects in the list for which the function returns True. 
    /// If no pointer is sent, the entire list will be returned
    /// </summary>
    /// <param name="filter">delegate of type FUNC that get a Task type</param>
    /// <returns>Returns a list of objects of type tasks that meet the condition of delegate</returns>
    public IEnumerable<Task?> ReadAll(Func<Task, bool>? filter = null)
    {
        if (filter != null)
        {
            return from item in DataSource.Tasks
                   where filter(item)
                   select item;
        }
        return from task in DataSource.Tasks  
               select task; 
    }

    /// <summary>
    /// this method receive a Task object, and check if there is in the task list a Task with the same id as item id. 
    /// if we find, we remove the old task and add the updated task, else we throw an exception
    /// </summary>
    /// <param name="item">Task object to add to the list</param>
    /// <exception cref="DalDoesNotExistException">if there is no task in the list with the same id as item, we throw exception</exception>
    public void Update(DO.Task item)
    {

        DO.Task? task = DataSource.Tasks.Find(t => t.Id == item.Id);
        if (task == null)
            throw new DalDoesNotExistException($"Task with id={item.Id} does not exist");
        else
        {
            DataSource.Tasks.RemoveAll(t => t.Id == item.Id);
            DataSource.Tasks.Add(item);
        }



    }

    /// <summary>
    /// this function gets a delegate of FUNC type which is a bool function, and return the first item in Task that is true in that function
    /// </summary>
    /// <param name="filter">filter function that return bool.</param>
    /// <returns>The function receives a delegate and looks for the first object that meets the condition, if it finds it it returns it and if not it returns NULL</returns>
    public Task? Read(Func<Task, bool> filter)
    {
        var respondToFilter = from item in DataSource.Tasks
                              where filter(item)
                              select item;

        return respondToFilter.FirstOrDefault();

    }
    /// <summary>
    /// We added this function for the benefit of the implementation of ICrud methods of the entity under XML and here it is not consumed
    /// </summary>
    public void DeleteAll()
    {
        return;

    }
}
