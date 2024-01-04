namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;


public class DependencyImplementation : IDependency
{
    public int Create(Dependency item)
    {
        int ID;
        ID = DataSource.Config.NextDependencyId;
        Dependency d = item with { Id=ID };
        DataSource.Dependencies.Add(d);
        return ID;
    }

    public void Delete(int id)
    {
        Dependency d;
        bool flag = false;
        foreach(var X in DataSource.Dependencies)
        {
            if (X.Id == id)
            {
                DataSource.Dependencies.Remove(X);
                flag = true;
            }
        }
        if(flag==false)
        throw new NotImplementedException("Object from type Dependency with such an ID does not exist");
    }

    public Dependency? Read(int id)
    {
        Dependency d;
        foreach(var X in DataSource.Dependencies)
        {
            if (X.Id == id)
                return X;
        }
        return null;
    }

    public List<Dependency> ReadAll()
    {
        return new List<Dependency>(DataSource.Dependencies);
    }

    public void Update(Dependency item)
    {
        Dependency? d=null;
         foreach(var X in DataSource.Dependencies)
        {
            if (X.Id == item.Id)
                d = X;
        }
        if (d == null)
            throw new NotImplementedException("Object of type Dependency with such an ID does not exist");
        else
            DataSource.Dependencies.Remove(d);
        DataSource.Dependencies.Add(item);
    }
}
