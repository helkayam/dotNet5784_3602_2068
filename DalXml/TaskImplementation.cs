

namespace Dal;
using DalApi;
using DO;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;

internal class TaskImplementation:ITask
{
    readonly string s_tasks_xml = "tasks";
   
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

    public DO.Task? Read(int id)
    {
        List<DO.Task> tasks = XMLTools.LoadListFromXMLSerializer<DO.Task>(s_tasks_xml);
        XMLTools.SaveListToXMLSerializer<DO.Task>(tasks, s_tasks_xml);
        if (tasks.Any(wker => wker.Id == id))
            return null;


        DO.Task saveItem =tasks.Find(saveItem => saveItem.Id == id)!;
        return saveItem;

    }

    public DO.Task? Read(Func<DO.Task, bool> filter)
    {
        List<DO.Task> tasks = XMLTools.LoadListFromXMLSerializer<DO.Task>(s_tasks_xml);
        var respondToFilter = from item in tasks
                              where filter(item)
                              select item;
        XMLTools.SaveListToXMLSerializer<DO.Task>(tasks, s_tasks_xml);
        return respondToFilter.FirstOrDefault();
    }

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

    public void deleteAllTasks()
    {
        List<DO.Task> tasks = XMLTools.LoadListFromXMLSerializer<DO.Task>(s_tasks_xml);
      
            tasks.Clear();
        XMLTools.SaveListToXMLSerializer<DO.Task>(tasks, s_tasks_xml);


    }
}
