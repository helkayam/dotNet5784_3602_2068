namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

internal class DependencyImplementation : IDependency
{
    /// <summary>
    /// The method receives an ID which is the identification number of the dependency and 
    /// checks whether such a strawberry exists in the list of dependencies
    /// </summary>
    /// <param name="id">ID number of the requested dependency</param>
    /// <returns></returns>
    public bool CheckDependency(int id)
    {
        return DataSource.Dependencies.Any(dependency => dependency.Id == id);
    }
    /// <summary>
    /// The method accepts a dependency type parameter and makes sure to add it to the list of dependencies in the dataSource
    /// if it does not exist
    /// </summary>
    /// <param name="item">Dependency type variable to add</param>
    /// <returns></returns>
    public int Create(Dependency item)
    {
        int ID;
        ID = DataSource.Config.NextDependencyId;//Since our key is a running number, we will use the config class to get a number and the next running number
        Dependency d = item with { Id = ID };
        DataSource.Dependencies.Add(d);
        return ID;
    }

    /// <summary>
    /// The method receives an ID of the dependency that we want to delete and if it is found by the search function, 
    /// it deletes it from the list of dependencies that appears in the dataSource
    /// </summary>
    /// <param name="id">ID number of the requested dependency </param>
    /// <exception cref="DalDoesNotExistException"></exception>
    public void Delete(int id)
    {

        Dependency? dependency = DataSource.Dependencies.Find(dependency => dependency.Id == id);
        if (dependency == null)
            throw new DalDoesNotExistException($"Dependency with id={id} does not exist");//If we did not find the object to delete, we will throw an exception of the type of object that does not exist

        DataSource.Dependencies.RemoveAll(dep => dep.Id == id);
      


    }

    /// <summary>
    /// The method receives an ID key of the dependency and checks whether it exists in the list of dependencies. 
    /// If so, you will return a dependency type variable with the requested key
    /// , if not, you will return null
    /// </summary>
    /// <param name="id">ID number of the requested dependency </param>
    /// <returns></returns>
    public DO.Dependency? Read(int id)
    {
        //The method uses CheckDependency to verify that the requested dependency exists.
        if (!CheckDependency(id))
            return null;
        //. In addition, Find is used to find the requested dependency
        DO.Dependency newDependency = DataSource.Dependencies.Find(newDependency => newDependency.Id == id);
        return newDependency;

    }

    /// <summary>
    /// The method will receive a pointer to a boolean function, delegate of type Func, which will act on one of the members of the list of dependency type and return the list of all objects in the list for which the function returns True. 
    /// If no pointer is sent, the entire list will be returned
    /// </summary>
    /// <param name="filter">This is a pointer to a boolean function (delegate) of type FUNC that will work on objects of type dependency </param>
    /// <returns></returns>
    public IEnumerable<Dependency?> ReadAll(Func<Dependency, bool>? filter = null)
    {
        if (filter != null)
        {
            return from item in DataSource.Dependencies
                   where filter(item)
                   select item;
        }
        return from dependency in DataSource.Dependencies
               select dependency;
    }

    /// <summary>
    /// The method receives a dependency type parameter with a key of an existing dependency and replaces
    /// it with the existing dependency if there is a dependency with this key
    /// </summary>
    /// <param name="item">Dependency type variable to update </param>
    /// <exception cref="DalDoesNotExistException"></exception>
    public void Update(Dependency item)
    {

        DO.Dependency? dependency = DataSource.Dependencies.Find(dp => dp.Id == item.Id);
        if (dependency == null)
            throw new DalDoesNotExistException($"Dependency with id={item.Id} does not exist");
        else
        {
            DataSource.Dependencies.RemoveAll(dp => dp.Id == item.Id);
            DataSource.Dependencies.Add(item);
        }

    }

    /// <summary>
    /// The method will receive a pointer to a boolean function, a delegate of type Func, which will act on one of the members of the dependency type list and return the first object in the list on which the function returns True.
    /// </summary>
    /// <param name="filter">This is a pointer to a boolean function (delegate) of type FUNC that will work on objects of type dependency </param>
    /// <returns></returns>
   public  Dependency? Read(Func<Dependency, bool> filter)
    {
        var respondToFilter = from item in DataSource.Dependencies
                              where filter(item)
                              select item;

        return respondToFilter.FirstOrDefault();

    }
}
