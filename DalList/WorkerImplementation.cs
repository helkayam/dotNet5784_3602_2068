namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;

internal class WorkerImplementation : IWorker
{
    /// <summary>
    /// this method receive an id number, and check if there is a worker with that id in the list.
    /// </summary>
    /// <param name="id">id that we want to check if its in the list </param>
    /// <returns></returns>
    public bool CheckWorker(int id)
    {
        return DataSource.Workers.Any(worker => worker.Id == id);
    }
    //   /// this method receive an id and check if there is a worker with that id in the list.
    /// if its found, we will check if its erasable. if its not erasable, we will throw error
    /// and we check also if the worker is active or not (if the worker is not active we throw error)
    /// if we found the worker with that id and he is erasable and active, we delete it from the list.

    /// <summary>
    /// this method receive a Worker object and check. if the id of the id already exist, we throw an error. 
    /// else, we add this worker to the lise and then return the id.
    /// </summary>
    /// <param name="item">Worker object </param>
    /// <returns></returns>
    /// <exception cref="DalAlreadyExistException">throw exception if this id already exist in the list</exception>
    public int Create(DO.Worker item)
    {
        if (CheckWorker(item.Id))
            throw new DalAlreadyExistException($"Worker with id={item.Id} already exist");
        else
            DataSource.Workers.Add(item);
       return item.Id;
    }
    /// <summary>
    /// this method receive an id and check if there is a worker with that id in the list.
    /// if its found, we will check if its erasable. if its not erasable, we will throw error
    /// and we check also if the worker is active or not (if the worker is not active we throw error)
    /// if we found the worker with that id and he is erasable and active, we delete it from the list.
    /// </summary>
    /// <param name="id">id </param>
    /// <exception cref="DalDoesNotExistException">Worker doesnt exist</exception>
    /// <exception cref="DalNotErasableException">Worker is not erasable</exception>
    /// <exception cref="DalNotActiveException">Worker is not active</exception>
    public void Delete(int id)
    {

        DO.Worker? worker = DataSource.Workers.Find(worker => worker.Id == id);
        if(worker==null)
            throw new DalDoesNotExistException($"Worker with id={id} does not exist" );
        if (worker.Eraseable==false)
            throw new DalNotErasableException($"Worker with id={id} is not eraseable");
        if (worker.active == false)
            throw new DalNotActiveException($"Worker with id={id} is not active");



        DO.Worker w = worker with { active = false };
        DataSource.Workers.RemoveAll(wrkr => wrkr.Id == id);
        DataSource.Workers.Add(w);

    }
    /// <summary>
    /// this method receive an id and check wethere there is a worker in the list with
    /// that id. If its found we return the object.
    /// </summary>
    /// <param name="id">id</param>
    /// <returns></returns>
    public DO.Worker? Read(int id, bool throwAnException=false)
    {
        if (CheckWorker(id)==false)
            if (throwAnException)
                throw new DalDoesNotExistException($"Worker with id={id} does not exist");
            else
                return null;    
        
            DO.Worker newWorker = DataSource.Workers.Find(newWorker => newWorker.Id == id);


        return newWorker;

    }

    /// <summary>
    /// The method will receive a pointer to a boolean function, delegate of type Func, which will act on one of the members of the list of Workers type and return the list of all objects in the list for which the function returns True. 
    /// If no pointer is sent, the entire list will be returned
    /// </summary>
    /// <param name="filter">filter function</param>
    /// <returns></returns>
    public IEnumerable<Worker> ReadAll(Func<Worker, bool>? filter = null)
    {
        if(filter!=null)
        {
            return from item in DataSource.Workers
                   where filter(item)&& item.active == true
                   select item; 
        }
        return from worker in DataSource.Workers
               where worker.active==true
               select worker; 
    }

  
    /// <summary>
    /// this method receive a Worker type object and check if there is a worker in the list with the same id. 
    /// If we found a worker with that id and if the worker is active, we will 
    /// update it to be what the function received.
    /// </summary>
    /// <param name="item">Worker object type</param>
    /// <exception cref="DalNotActiveException">worker is not active</exception>
    /// <exception cref="DalDoesNotExistException">worker doesnt exist</exception>
    public void Update(DO.Worker item)
    {
        DO.Worker w = DataSource.Workers.Find(w => w.Id == item.Id);
        if(w==null)
            throw new DalDoesNotExistException($"Worker with id={item.Id} does not exist");
        if (w.active == false)
            throw new DalNotActiveException($"Worker with id={item.Id} is not active");
        if (w != null)
        {
            DataSource.Workers.RemoveAll(wrkr=>wrkr.Id==item.Id);
            DataSource.Workers.Add(item);
        }
        
    }

    /// <summary>
    /// this method gets a delegate from type FUNC.
    /// this delegate funtion is a boolean function.
    /// this method check whats the first worker object wich get true in the func filter.
    /// </summary>
    /// <param name="filter">name of the function</param>
    /// <returns></returns>
    public  Worker? Read(Func<Worker, bool> filter)
    {
        var respondToFilter= from item in DataSource.Workers
               where filter(item)
               select item;

        return respondToFilter.FirstOrDefault();

    }

    public void DeleteAll()
    {

        foreach( var item in DataSource.Workers)
        {
            DataSource.Workers.Remove(item);
        }
    }
}
