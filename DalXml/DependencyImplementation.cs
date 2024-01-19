namespace Dal;
using DalApi;
using DO;
using System;
using System.Collections.Generic;


internal class DependencyImplementation:IDependency
{
    /// <summary>
    /// This is a private read-only field of string type that will be initialized with the name of the xml file that constitutes the database of the Dependency entity.
    /// </summary>
    readonly string s_dependencies_xml = "dependencies";

    /// <summary>
    /// 1.With the help of the XmlSerializer class, the list of objects is loaded from an XML file into a logical list of the List type.
    /// 2.the method get a Dependency type parameter and add it to the list
    /// 3.Save the list back to the XML file through the XmlSerializer
    /// </summary>
    /// <param name="item">Dependency object to add to the list</param>
    /// <returns>The method returns the ID of the object it added</returns>
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

    /// <summary>
    /// 1.With the help of the XmlSerializer class, the list of objects is loaded from an XML file into a logical list of the List type.
    /// 2.the method receive a id, and check whether there is a Dependency in the list with that id. if its exist,  we will delete it 
    /// 3. Save the list back to the XML file through the XmlSerializer
    /// </summary>
    /// <param name="id">id of the dependency we are searching for</param>
    /// <exception cref="DalDoesNotExistException">Dependency with that id doesnt exist</exception>
    public void Delete(int id)
    {
        List<Dependency> dependencies = XMLTools.LoadListFromXMLSerializer<Dependency>(s_dependencies_xml);
        Dependency? dependency = dependencies.Find(dependency => dependency.Id == id);
        if (dependency == null)
            throw new DalDoesNotExistException($"Dependency with id={id} does not exist");//If we did not find the object to delete, we will throw an exception of the type of object that does not exist

        dependencies.RemoveAll(dep => dep.Id == id);
         XMLTools.SaveListToXMLSerializer<Dependency>(dependencies, s_dependencies_xml);
    }

    /// <summary>
    /// 1.With the help of the XmlSerializer class, the list of objects is loaded from an XML file into a logical list of the List type.
    /// 2.this method receive a id of a Dependency, and return the Dependency with that id. if we did not find a Dependency with this id, we will return null.
    /// 3.Save the list back to the XML file through the XmlSerializer
    /// </summary>
    /// <param name="id">id of the Dependency we are looking for</param>
    /// <param name="throwAnException"></param>
    /// <returns></returns>
    /// <exception cref="DalDoesNotExistException"></exception>
    public Dependency? Read(int id, bool throwAnException)
    {
        List<Dependency> dependencies = XMLTools.LoadListFromXMLSerializer<Dependency>(s_dependencies_xml);
        XMLTools.SaveListToXMLSerializer<Dependency>(dependencies, s_dependencies_xml);

        // verify that the requested dependency exists.
        if (dependencies.Any(dependency => dependency.Id == id) == false)
            if (throwAnException)
                throw new DalDoesNotExistException($"Dependency with id={id} does not exist");
           else
            return null;
       
        //. we find the requested dependency
        DO.Dependency newDependency = dependencies.Find(newDependency => newDependency.Id == id);
        return newDependency;
    }

    /// <summary>
    /// 1.With the help of the XmlSerializer class, the list of objects is loaded from an XML file into a logical list of the List type.
    /// 2.The method will receive a pointer to a boolean function, delegate of type Func, which will act on one of the members of the list of Dependency type and return the list of all objects in the list for which the function returns True. 
    /// If no pointer is sent, the entire list will be returned
    /// 3.Save the list back to the XML file through the XmlSerializer
    /// </summary>
    /// <param name="filter">filter function</param>
    /// <returns></returns>
    public Dependency? Read(Func<Dependency, bool> filter)
    {
        List<Dependency> dependencies = XMLTools.LoadListFromXMLSerializer<Dependency>(s_dependencies_xml);
        XMLTools.SaveListToXMLSerializer<Dependency>(dependencies, s_dependencies_xml);

        var respondToFilter = from item in dependencies
                              where filter(item)
                              select item;

        return respondToFilter.FirstOrDefault();
    }

    /// <summary>
    /// 1.With the help of the XmlSerializer class, the list of objects is loaded from an XML file into a logical list of the List type.
    /// 2.The method will receive a pointer to a boolean function, delegate of type Func, which will act on one of the members of the list of Depenency type and return the list of all objects in the list for which the function returns True. 
    /// If no pointer is sent, the entire list will be returned
    /// 3.Save the list back to the XML file through the XmlSerializer
    /// </summary>
    /// <param name="filter">filter function</param>
    /// <returns></returns>
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

    /// <summary>
    /// 1.With the help of the XmlSerializer class, the list of objects is loaded from an XML file into a logical list of the List type.
    /// 2.the method receive a Dependency object, and check if there is in the Dependencies list a Dependency with the same id as item id. 
    /// if we find, we remove the old task and add the updated task, else we throw an exception
    /// 3.Save the list back to the XML file through the XmlSerializer
    /// </summary>
    /// <param name="item"></param>
    /// <exception cref="DalDoesNotExistException"></exception>
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

    /// <summary>
    /// 1.With the help of the XmlSerializer class, the list of objects is loaded from an XML file into a logical list of the List type.
    /// 2.This function makes it possible to empty the list of elements of the dependencies in order to enable initialization of the data in the program
    /// 3.Save the list back to the XML file through the XmlSerializer
    /// </summary>
    public void DeleteAll()
    {
        List<Dependency> dependencies = XMLTools.LoadListFromXMLSerializer<DO.Dependency>(s_dependencies_xml);

        dependencies.Clear();
        XMLTools.SaveListToXMLSerializer<Dependency>(dependencies, s_dependencies_xml);


    }
}
