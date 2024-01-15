
namespace Dal;
using DalApi;
using DO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.Reflection.Metadata.Ecma335;
using System.Xml.Linq;

internal class WorkerImplementation:IWorker
{
    readonly string s_workers_xml = "workers";

    static Worker getWorker(XElement items)
    {
        return new Worker()
        {
            Id = Convert.ToInt32(items.Element("Id")!.Value),
            Level = (WorkerExperience)Convert.ToInt32(items.Element("Level")!.Value),
            Name = items.Element("Name")!.Value,
            PhoneNumber = items.Element("PhoneNumber")!.Value,
            Cost = items.ToDoubleNullable(items.Element("Cost")!.Value),
            Eraseable = Convert.ToBoolean(items.Element("Eraseable")!.Value),
            active = Convert.ToBoolean(items.Element("active")!.Value)
        };
    }
    

    public int Create(Worker item)
    {
        XElement workers=XMLTools.LoadListFromXMLElement(s_workers_xml);
       var sameId=(from objectWorker in workers.Elements()
                  where objectWorker.ToIntNullable(objectWorker.Element("Id")!.Value)==item.Id  
                  select objectWorker).FirstOrDefault(); 
        if(sameId!=null)
        {
              throw new DalAlreadyExistException($"Worker with id={item.Id} already exist");
        }
        workers.Add(item);
        XMLTools.SaveListToXMLElement(workers,s_workers_xml);
        return item.Id;
    }

    public void Delete(int id)
    {
        XElement workers = XMLTools.LoadListFromXMLElement(s_workers_xml);
        XElement? elementToRemove =(from item in workers.Elements()
                 where item.ToIntNullable(item.Element("Id")!.Value) == id
                 select item).FirstOrDefault();
        if (elementToRemove == null)
            throw new DalDoesNotExistException($"Worker with id={id} does not exist");
        if(Convert.ToBoolean(elementToRemove.Element("Eraseable")!.Value)==false)
            throw new DalNotErasableException($"Worker with id={id} is not eraseable");
        if (Convert.ToBoolean(elementToRemove.Element("active")!.Value) == false)
            throw new DalNotActiveException($"Worker with id={id} is not active");
        XElement newWorkerElement = elementToRemove;
        newWorkerElement.Element("active")!.Value = (false).ToString();
        elementToRemove.Remove();
        workers.Add(newWorkerElement);
        XMLTools.SaveListToXMLElement(workers, s_workers_xml);
    }

    public Worker? Read(int id)
    {

        XElement workers = XMLTools.LoadListFromXMLElement(s_workers_xml);
        var sameId = (from objectWorker in workers.Elements()
                      where objectWorker.ToIntNullable(objectWorker.Element("Id")!.Value) == id
                      select getWorker(objectWorker)).FirstOrDefault();

        XMLTools.SaveListToXMLElement(workers, s_workers_xml);

        if (sameId == null)
            return null;
        else
            return sameId;
        

    }

    public Worker? Read(Func<Worker, bool> filter)
    {
        XElement workers = XMLTools.LoadListFromXMLElement(s_workers_xml);
        var respondToFilter = from item in workers.Elements()   
                             where filter(getWorker(item))
                              select getWorker(item);
        XMLTools.SaveListToXMLElement(workers, s_workers_xml);
        return respondToFilter.FirstOrDefault();
    }

    public IEnumerable<Worker?> ReadAll(Func<Worker, bool>? filter = null)
    {
        XElement workers = XMLTools.LoadListFromXMLElement(s_workers_xml);

        XMLTools.SaveListToXMLElement(workers, s_workers_xml);
        if (filter != null)
        {
            return from item in workers.Elements()   
                             where filter(getWorker(item))
                              select getWorker(item);
           
        }
        return from worker in workers.Elements()
               where (Convert.ToBoolean(worker.Element("active")!.Value)==true)
               select getWorker(worker);
        
    }

    public void Update(Worker item)
    {
        XElement workers = XMLTools.LoadListFromXMLElement(s_workers_xml);

        var sameId = (from objectWorker in workers.Elements()
                      where objectWorker.ToIntNullable(objectWorker.Element("Id")!.Value) == item.id
                      select getWorker(objectWorker));

        if (sameId.FirstOrDefault() == null)
            throw new DalDoesNotExistException($"Worker with id={item.Id} does not exist");

        if (sameId.FirstOrDefault().active == false)
            throw new DalNotActiveException($"Worker with id={item.Id} is not active");

        else
        {
            workers.Remove(sameId.FirstOrDefault());    
            workers.Add(item);
        }
        XMLTools.SaveListToXMLElement(workers, s_workers_xml);

    }

    public void deleteAllWorkers()
    {
        List<Worker> workers = XMLTools.LoadListFromXMLSerializer<Worker >(s_workers_xml);
        workers.Clear();
        XMLTools.SaveListToXMLSerializer<Worker>(workers, s_workers_xml);


    }
}
