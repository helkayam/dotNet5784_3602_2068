namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;


internal class DependencyImplementation : IDependency
{

    public bool CheckDependency(int id)
    {
        return DataSource.Dependencies.Any(dependency => dependency.Id == id);
    }
    public int Create(Dependency item)
    {
        int ID;
        ID = DataSource.Config.NextDependencyId;
        Dependency d = item with { Id = ID };
        DataSource.Dependencies.Add(d);
        return ID;
    }

    public void Delete(int id)
    {

        Dependency? dependency = DataSource.Dependencies.Find(dependency => dependency.Id == id);
        if (dependency == null)
            throw new Exception($"Dependency with ID={id} does not exist");

        DataSource.Dependencies.RemoveAll(dep => dep.Id == id);


    }

    public DO.Dependency? Read(int id)
    {

        if (!CheckDependency(id))
            return null;

        DO.Dependency newDependency = DataSource.Dependencies.Find(newDependency => newDependency.Id == id);
        return newDependency;

    }

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

    public void Update(Dependency item)
    {

        DO.Dependency? dependency = DataSource.Dependencies.Find(dp => dp.Id == item.Id);
        if (dependency == null)
            throw new Exception($"Dependency with ID={item.Id} does not exist");
        else
        {
            DataSource.Dependencies.RemoveAll(dp => dp.Id == item.Id);
            DataSource.Dependencies.Add(item);
        }

    }

    Dependency? Read(Func<Dependency, bool> filter)
    {
        var respondToFilter = from item in DataSource.Dependencies
                              where filter(item)
                              select item;

        return respondToFilter.FirstOrDefault();

    }
}
