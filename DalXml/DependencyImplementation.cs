
namespace Dal;
using DalApi;
using DO;
using System;
using System.Collections.Generic;

internal class DependencyImplementation:IDependency
{
    readonly string s_dependencies_xml = "dependencies";

    public int Create(Dependency item)
    {
        List<Dependency> dependencies = XMLTools.LoadListFromXMLSerializer<Dependency>(s_dependencies_xml);
        int ID;
        ID = Config.NextDependencyId;//Since our key is a running number, we will use the config class to get a number and the next running number
        Dependency d = item with { Id = ID };
        dependencies.Add(d);
        XMLTools.SaveListToXMLSerializer<Dependency>(dependencies, s_dependencies_xml);
        return ID;
    }

    public void Delete(int id)
    {
        List<Dependency> dependencies = XMLTools.LoadListFromXMLSerializer<Dependency>(s_dependencies_xml);
        Dependency? dependency = dependencies.Find(dependency => dependency.Id == id);
        if (dependency == null)
            throw new DalDoesNotExistException($"Dependency with id={id} does not exist");//If we did not find the object to delete, we will throw an exception of the type of object that does not exist

        dependencies.RemoveAll(dep => dep.Id == id);
         XMLTools.SaveListToXMLSerializer<Dependency>(dependencies, s_dependencies_xml);
    }

    public Dependency? Read(int id)
    {
        List<Dependency> dependencies = XMLTools.LoadListFromXMLSerializer<Dependency>(s_dependencies_xml);
        XMLTools.SaveListToXMLSerializer<Dependency>(dependencies, s_dependencies_xml);

        // verify that the requested dependency exists.
        if (dependencies.Any(dependency => dependency.Id == id)==false)
            return null;
       
        //. we find the requested dependency
        DO.Dependency newDependency = dependencies.Find(newDependency => newDependency.Id == id);
        return newDependency;
    }

    public Dependency? Read(Func<Dependency, bool> filter)
    {
        List<Dependency> dependencies = XMLTools.LoadListFromXMLSerializer<Dependency>(s_dependencies_xml);
        XMLTools.SaveListToXMLSerializer<Dependency>(dependencies, s_dependencies_xml);

        var respondToFilter = from item in dependencies
                              where filter(item)
                              select item;

        return respondToFilter.FirstOrDefault();
    }

    public IEnumerable<Dependency?> ReadAll(Func<Dependency, bool>? filter = null)
    {
        List<Dependency> dependencies = XMLTools.LoadListFromXMLSerializer<Dependency>(s_dependencies_xml);
        XMLTools.SaveListToXMLSerializer<Dependency>(dependencies, s_dependencies_xml);
        if (filter != null)
        {
            return from item in dependencies
                   where filter(item)
                   select item;
        }
        return from dependency in dependencies
               select dependency;
    }

    public void Update(Dependency item)
    {
        List<Dependency> dependencies = XMLTools.LoadListFromXMLSerializer<Dependency>(s_dependencies_xml);
        DO.Dependency? dependency = dependencies.Find(dp => dp.Id == item.Id);
        if (dependency == null)
            throw new DalDoesNotExistException($"Dependency with id={item.Id} does not exist");
        else
        {
            dependencies.RemoveAll(dp => dp.Id == item.Id);
            dependencies.Add(item);
        }

        XMLTools.SaveListToXMLSerializer<Dependency>(dependencies, s_dependencies_xml);

    }

    public void deleteAllDependencies()
    {
        List<Dependency> dependencies = XMLTools.LoadListFromXMLSerializer<DO.Dependency>(s_dependencies_xml);

        dependencies.Clear();
        XMLTools.SaveListToXMLSerializer<Dependency>(dependencies, s_dependencies_xml);


    }
}
