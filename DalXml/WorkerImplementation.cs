
namespace Dal;
using DalApi;
using DO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using System.Xml.Linq;

internal class WorkerImplementation:IWorker
{
    readonly string s_workers_xml = "workers";

    static Worker getWorker(XElement items)
    {
        return new Worker
        {
            Id =(int)(items.Element("Id"))!,
            Level =(WorkerExperience)Enum.Parse(typeof(WorkerExperience),items.Element("Level")!.Value),
            Name = items.Element("Name")!.Value,
            PhoneNumber = items.Element("PhoneNumber")!.Value,
            Cost = items.ToDoubleNullable("Cost"),
            Eraseable = (bool)(items.Element("Eraseable")!),
            active = (bool)(items.Element("active")!)
        };
    }
    

    public int Create(Worker item)
    {
        bool flag = false;
        XElement workers=XMLTools.LoadListFromXMLElement(s_workers_xml);
        
        foreach (var worker in workers.Elements())
        {
            if (getWorker(worker).Id == item.Id)
                flag = true;
        }

        if(flag==true)
            throw new DalAlreadyExistException($"Worker with id={item.Id} already exist");
  
        else
        {
            XElement ID = new XElement("Id", item.Id);
            XElement Name = new XElement("Name", item.Name);
            XElement Level = new XElement("Level",item.Level);
            XElement PhoneNumber = new XElement("PhoneNumber", item.PhoneNumber);
            XElement Cost = new XElement("Cost", item.Cost);
            XElement Eraseable = new XElement("Eraseable", item.Eraseable);
            XElement Active = new XElement("active", item.active);

            workers.Add(new XElement("Worker", ID,Level,Name,PhoneNumber,Cost,Eraseable,Active));
        }
        XMLTools.SaveListToXMLElement(workers,s_workers_xml);
        return item.Id;
    }

    public void Delete(int id)
    {
        XElement workers = XMLTools.LoadListFromXMLElement(s_workers_xml);
        XElement? elementToRemove =(from item in workers.Elements()
                 where (int)(item.Element("Id"))! == id
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
        XElement? worker;
        worker=workers!.Elements().FirstOrDefault(w=> (int)(w.Element("Id")!) == id);
        //var sameId = (from objectWorker in workers.Elements()
        //              where objectWorker.ToIntNullable(objectWorker.Element("Id")!.Value) == id
        //              select getWorker(objectWorker)).FirstOrDefault();

        XMLTools.SaveListToXMLElement(workers, s_workers_xml);
        if (worker == null)
            return null;
        return getWorker(worker);    

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
               where ((bool)(worker.Element("active")!)==true)
               select getWorker(worker);
        
    }

    public void Update(Worker item)
    {
        XElement workers = XMLTools.LoadListFromXMLElement(s_workers_xml);

        var sameId = (from objectWorker in workers.Elements()
                      where (int)(objectWorker.Element("Id"))! == item.Id
                      select objectWorker).FirstOrDefault();

        if (sameId == null)
            throw new DalDoesNotExistException($"Worker with id={item.Id} does not exist");

        if (getWorker(sameId).active == false)
            throw new DalNotActiveException($"Worker with id={item.Id} is not active");

        else
        {
            sameId.Element("Level").Value = item.Level.ToString();
            sameId.Element("Name").Value = item.Name;
            sameId.Element("PhoneNumber").Value = item.PhoneNumber;
            sameId.Element("Cost").Value = item.Cost.ToString();
            sameId.Element("Eraseable").Value = item.Eraseable.ToString();
            sameId.Element("active").Value = item.active.ToString();
        }
        XMLTools.SaveListToXMLElement(workers, s_workers_xml);

    }

    public void DeleteAll()
    {
        XElement workers = XMLTools.LoadListFromXMLElement(s_workers_xml);
        foreach(var worker in workers.Elements()) { Delete(getWorker(worker).Id); };
        XMLTools.SaveListToXMLElement(workers, s_workers_xml);
    }
}
