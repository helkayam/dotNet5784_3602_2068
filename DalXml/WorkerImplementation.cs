
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

    /// <summary>
    /// This is a private read-only field of string type that will be initialized with the name of the xml file that constitutes the database of the Worker entity.
    /// </summary>
    readonly string s_workers_xml = "workers";

    /// <summary>
    /// this method gets an XElement type object, and return a Worker with the same data as the XElement
    /// </summary>
    /// <param name="items">item from XElement type</param>
    /// <returns>return the Element but in Worker type</returns>
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

    /// <summary>
    /// 1.we load the list of objects from an XML file into a collection of the XElement class type
    /// 2.this method receive a Worker object and check. if the id of the worker already exist, we throw an error. 
    /// else, we add this worker to the XElement and then return the id.
    /// 3.we save the XElement collection type to the XML file
    /// </summary>
    /// <param name="item">the Worker we want to add to the XML file</param>
    /// <returns>return the id of the worker we added to the XML</returns>
    /// <exception cref="DalAlreadyExistException">if the worker already exist(if his Id exist) we throw an exception</exception>

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

    /// 1.we load the list of objects from an XML file into a collection of the XElement class type
    /// 2.this method receive an id and check if there is a worker with that id in the XLM File.
    /// if its found, we will check if its erasable. if its not erasable, we will throw error
    /// and we check also if the worker is active or not (if the worker is not active we throw error)
    /// if we found the worker with that id and he is erasable and active, we delete it from the File.
    /// 3.we save the XElement collection type to the XML file
    /// <param name="id">id of the Worker we are searching for</param>
    /// <exception cref="DalDoesNotExistException">Worker doesnt exist</exception>
    /// <exception cref="DalNotErasableException">Worker is not erasable</exception>
    /// <exception cref="DalNotActiveException">Worker is not active</exception>
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

    /// <summary>
    /// 1.we load the list of objects from an XML file into a collection of the XElement class type
    /// 2.this method receive an id and check wether there is a worker in the XElement with
    /// that id. If its found we return the object.
    /// 3.we save the XElement collection type to the XML file
    /// </summary>
    /// <param name="id">the id of the worker we are searching for</param>
    /// <param name="throwAnException">if we wna to throw an exception when we did not found it (and dont want to return null ) this boolean paramaeter will be true</param>
    /// <returns>return the worker with the Id(if we find it)</returns>
    /// <exception cref="DalDoesNotExistException">throw exception if we didnt find the Worker</exception>
    public Worker? Read(int id,bool throwAnException=false)
    {

        XElement workers = XMLTools.LoadListFromXMLElement(s_workers_xml);
        XElement? worker;
        worker=workers!.Elements().FirstOrDefault(w=> (int)(w.Element("Id")!) == id);
     

        XMLTools.SaveListToXMLElement(workers, s_workers_xml);
        if (worker == null)
            if (throwAnException)
                throw new DalDoesNotExistException($"Worker with id={id} does not exist");
            else
                return null;
        return getWorker(worker);    

    }
    /// <summary>
    /// 1.we load the list of objects from an XML file into a collection of the XElement class type
    /// 2.this method gets a delegate from type FUNC.
    /// this delegate funtion is a boolean function.
    /// this method check whats the first worker object wich get true in the func filter.
    /// 3.we save the XElement collection type to the XML file
    /// </summary>
    /// <param name="filter">filter function</param>
    /// <returns>return the first worker in the XML hat respond to the filter true</returns>
    public Worker? Read(Func<Worker, bool> filter)
    {
        XElement workers = XMLTools.LoadListFromXMLElement(s_workers_xml);
        var respondToFilter = from item in workers.Elements()   
                             where filter(getWorker(item))
                              select getWorker(item);
        XMLTools.SaveListToXMLElement(workers, s_workers_xml);
        return respondToFilter.FirstOrDefault();
    }

    /// <summary>
    /// 1.we load the list of objects from an XML file into a collection of the XElement class type
    /// 2.The method will receive a pointer to a boolean function, delegate of type Func, which will act on one of the members of the list of Worker type and return the list of all objects in the list for which the function returns True. 
    /// If no pointer is sent, the entire list will be returned
    /// 3.we save the XElement collection type to the XML file
    /// </summary>
    /// <param name="filter">filter function</param>
    /// <returns>return a collection of all the workers that respond true to the filter</returns>
    public IEnumerable<Worker> ReadAll(Func<Worker, bool>? filter = null)
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
               select getWorker(worker);
        
    }

    /// <summary>
    /// 1.we load the list of objects from an XML file into a collection of the XElement class type
    /// 2.this method receive a Worker type object and check if there is a worker in the list with the same id. 
    /// If we found a worker with that id and if the worker is active, we will 
    /// update it to be what the function received.
    /// 3.we save the XElement collection type to the XML file
    /// </summary>
    /// <param name="item">the updated worker</param>
    /// <exception cref="DalDoesNotExistException">Worker doesnt exist</exception>
    /// <exception cref="DalNotActiveException">Workers is not active</exception>
    public void Update(Worker item)
    {
        XElement workers = XMLTools.LoadListFromXMLElement(s_workers_xml);

        var sameId = (from objectWorker in workers.Elements()
                      where (int)(objectWorker.Element("Id"))! == item.Id
                      select objectWorker).FirstOrDefault();

        if (sameId == null)
            throw new DalDoesNotExistException($"Worker with id={item.Id} does not exist");

       

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

    /// <summary>
    /// 1.we load the list of objects from an XML file into a collection of the XElement class type
    /// 2.this method delete all the workers in the XML file.
    /// 3.we save the XElement collection type to the XML file
    /// </summary>
    public void DeleteAll()
    {
        XElement workers = XMLTools.LoadListFromXMLElement(s_workers_xml);
        foreach(XElement worker in workers.Elements().ToList())
            worker.Remove();

      //  foreach(var worker in workers.Elements()) { Delete(getWorker(worker).Id); };
        XMLTools.SaveListToXMLElement(workers, s_workers_xml);
    }
}
