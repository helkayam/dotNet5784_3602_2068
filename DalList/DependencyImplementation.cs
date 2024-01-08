namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;


internal class DependencyImplementation : IDependency
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
        throw new Exception($"Dependency with ID={id} does not exist");
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
            throw new Exception($"Dependency with ID={item.Id} does not exist");
        else
        {
            DataSource.Dependencies.Remove(d);
            DataSource.Dependencies.Add(item);
        }
    }
}
