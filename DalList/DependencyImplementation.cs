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
        throw new NotImplementedException();
    }

    public Dependency? Read(int id)
    {
        throw new NotImplementedException();
    }

    public List<Dependency> ReadAll()
    {
        throw new NotImplementedException();
    }

    public void Update(Dependency item)
    {
        throw new NotImplementedException();
    }
}
