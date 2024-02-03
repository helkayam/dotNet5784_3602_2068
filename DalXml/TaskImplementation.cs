

namespace Dal;
using DalApi;
using DO;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;
using System.Xml.Linq;

/// <summary>
/// Create a class that will implement the ICrud methods that can be performed on Task entity by contacting a data collection of the XML file type
/// </summary>
internal class TaskImplementation:ITask
{
    readonly string data_config = "data-config";
    /// <summary>
    /// This is a private read-only field of string type that will be initialized with the name of the xml file that constitutes the database of the task entity.
    /// </summary>
    readonly string s_tasks_xml = "tasks";
    /// <summary>
    /// 1.With the help of the XmlSerializer class, the list of objects is loaded from an XML file into a logical list of the List type.
    /// 2.the method get a Task type parameter and add it to the list
    /// 3.Save the list back to the XML file through the XmlSerializer
    /// </summary>
    /// <param name="item">Task object to add to the list</param>
    /// <returns>The method returns the ID of the object it added</returns>
    public int Create(DO.Task item)
    {
        List<DO.Task> tasks = XMLTools.LoadListFromXMLSerializer<DO.Task>(s_tasks_xml);
        int ID;
        ID =Config.NextTaskId;
        DO.Task t = item with { Id = ID };
        tasks.Add(t);
        XMLTools.SaveListToXMLSerializer<DO.Task>(tasks, s_tasks_xml);
        return ID;
    }

    /// <summary>
    /// 1.With the help of the XmlSerializer class, the list of objects is loaded from an XML file into a logical list of the List type.
    /// 2.the method receive a id, and check whether there is a task in the list with that id. if its exist and its erasable, we will delete it 
    /// 3. Save the list back to the XML file through the XmlSerializer
    /// </summary>
    /// <param name="id">id of the Task we want to delete</param>
    /// <exception cref="DalDoesNotExistException">if the Task does not exist, we will throw an exception</exception>
    /// <exception cref="DalNotErasableException">if the task is found in the list but its not erasable, we will throw an exception</exception>
    public void Delete(int id)
    {
        List<DO.Task> tasks = XMLTools.LoadListFromXMLSerializer<DO.Task>(s_tasks_xml);
        DO.Task? taskToRemove = tasks.Find(x => x.Id == id); 
        if (taskToRemove == null)
        {
            throw new DalDoesNotExistException($"Task with id={id} does not exist");
        }
        if (!taskToRemove.Eraseable)
        {
            throw new DalNotErasableException($"Task with id={id} is not eraseable");
        }
        tasks.RemoveAll(t => t.Id == id);
        XMLTools.SaveListToXMLSerializer<DO.Task>(tasks, s_tasks_xml);

    }

    /// <summary>
    /// 1.With the help of the XmlSerializer class, the list of objects is loaded from an XML file into a logical list of the List type.
    /// 2.this method receive a id of a task, and return the task with that id. if we did not find a task with this id, we will return null.
    /// 3.Save the list back to the XML file through the XmlSerializer
    /// </summary>
    /// <param name="id">id of the Task we want to search</param>
    /// <param name="throwAnException">This is a feature that allows me to choose whenever there is a case that I can't find the object if I want to throw an exception that informs me of this or return NULL</param>
    /// <returns>The method returns the dependency type object if it found signals otherwise it will return NULL</returns>
    /// <exception cref="DalDoesNotExistException">This is an exception that informs that the object being searched for does not exist</exception>
    public DO.Task? Read(int id, bool throwAnException )
    {
        List<DO.Task> tasks = XMLTools.LoadListFromXMLSerializer<DO.Task>(s_tasks_xml);
        XMLTools.SaveListToXMLSerializer<DO.Task>(tasks, s_tasks_xml);
        if (tasks.Any(wker => wker.Id == id)==false)
            if (throwAnException)
                throw new DalDoesNotExistException($"Task with id={id} does not exist");
             else   
                return null;


        DO.Task saveItem =tasks.Find(saveItem => saveItem.Id == id)!;
        return saveItem;

    }
    /// <summary>
    /// 1.With the help of the XmlSerializer class, the list of objects is loaded from an XML file into a logical list of the List type.
    /// 2.the function gets a delegate of FUNC type which is a bool function, and return the first item in Task that is true in that function
    /// 3.Save the list back to the XML file through the XmlSerializer
    /// </summary>
    /// <param name="filter">filter function that return bool</param>
    /// <returns> The function receives a delegate and looks for the first object that meets the condition, if it finds it it returns it and if not it returns NULL</returns>
    public DO.Task? Read(Func<DO.Task, bool> filter)
    {
        List<DO.Task> tasks = XMLTools.LoadListFromXMLSerializer<DO.Task>(s_tasks_xml);
        var respondToFilter = from item in tasks
                              where filter(item)
                              select item;
        XMLTools.SaveListToXMLSerializer<DO.Task>(tasks, s_tasks_xml);
        return respondToFilter.FirstOrDefault();
    }

    /// <summary>
    /// 1.With the help of the XmlSerializer class, the list of objects is loaded from an XML file into a logical list of the List type.
    /// 2.The method will receive a pointer to a boolean function, delegate of type Func, which will act on one of the members of the list of Task type and return the list of all objects in the list for which the function returns True. 
    /// If no pointer is sent, the entire list will be returned
    /// 3.Save the list back to the XML file through the XmlSerializer
    /// </summary>
    /// <param name="filter">delegate of type FUNC that get a Task type</param>
    /// <returns>Returns a list of objects of type tasks that meet the condition of delegate</returns>
    public IEnumerable<DO.Task?> ReadAll(Func<DO.Task, bool>? filter = null)
    {
        List<DO.Task> tasks = XMLTools.LoadListFromXMLSerializer<DO.Task>(s_tasks_xml);
        XMLTools.SaveListToXMLSerializer<DO.Task>(tasks, s_tasks_xml);
        if (filter != null)
        {
            return from item in tasks
                   where filter(item)
                   select item;
        }
        return from task in tasks
               select task;
    }
    /// <summary>
    /// 1.With the help of the XmlSerializer class, the list of objects is loaded from an XML file into a logical list of the List type.
    /// 2.the method receive a Task object, and check if there is in the task list a Task with the same id as item id. 
    /// if we find, we remove the old task and add the updated task, else we throw an exception
    /// 3.Save the list back to the XML file through the XmlSerializer
    /// </summary>
    /// <param name="item">Task object to add to the list</param>
    /// <exception cref="DalDoesNotExistException">if there is no task in the list with the same id as item, we throw exception</exception>
    public void Update(DO.Task item)
    {
        List<DO.Task> tasks = XMLTools.LoadListFromXMLSerializer<DO.Task>(s_tasks_xml);
        DO.Task? task = tasks.Find(t => t.Id == item.Id);
        if (task == null)
            throw new DalDoesNotExistException($"Dependency with id={item.Id} does not exist");
        else
        {
            tasks.RemoveAll(t => t.Id == item.Id);
            tasks.Add(item);
        }
        XMLTools.SaveListToXMLSerializer<DO.Task>(tasks, s_tasks_xml);

    }

    public void UpdateStartDateProject(DateTime startDate)
    {
        XElement root = XMLTools.LoadListFromXMLElement(data_config);
        root.Element("StartDateProject")?.SetValue((startDate));
        XMLTools.SaveListToXMLElement(root, data_config);

    }
    public  DateTime? GetStartDateProject( )
    {
       
        XElement root = XMLTools.LoadListFromXMLElement(data_config);
        string? dt=root.Element("StartDateProject").Value;

        XMLTools.SaveListToXMLElement(root, data_config);
        if (dt != null)
            return DateTime.Parse(dt);
        else
            return null;
    }

    /// <summary>
    /// 1.With the help of the XmlSerializer class, the list of objects is loaded from an XML file into a logical list of the List type.
    /// 2.This function makes it possible to empty the list of elements of the tasks in order to enable initialization of the data in the program
    /// 3.Save the list back to the XML file through the XmlSerializer
    /// </summary>
    public void DeleteAll()
    {
        List<DO.Task> tasks = XMLTools.LoadListFromXMLSerializer<DO.Task>(s_tasks_xml);
      
            tasks.Clear();
        XMLTools.SaveListToXMLSerializer<DO.Task>(tasks, s_tasks_xml);


    }
}
