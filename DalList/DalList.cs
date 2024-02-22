using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Dal;
using DalApi;

/// <summary>
/// This is a class that inherits and implements the new IDal interface by initializing the sub-interfaces in the access classes
/// </summary>
sealed internal class DalList : IDal
{
    /// <summary>
    /// this is Thread Safe singleton with Lazy Initialization
    /// we use the generic Lazy<T> class to achieve the lazy implementation. 
    /// the Lazy<T> take care to check if the lazyInstance has already been initialized ,how?
    /// when you access the Value property of LazyInstance, if the instance has been created already,
    /// it will return the existing instance that have been initialized before, else it will initialized a new instance.
    /// The Lazy class ensure that the 
    /// initialization is done just once, and its thread-safe by default
    /// if multiple threads want to access the Value at the same time, only one thread will do the initialization,
    /// and then the others will wait for the initialization complete before getting the resulting instance
    /// after initialization all the calls to Value will return the existing instance.
    /// </summary>
    private static readonly Lazy<DalList>LazyInstance = new Lazy<DalList>(()=>new DalList());

    private DalList() { }
    public static DalList Instance { get { return LazyInstance.Value; } }

    public ISchedule Schedule => new ScheduleImplementation();
    
    //Implementing the appropriate property defined for dependencies in the IDal interface, so that they return objects of the type that implements IDendency
    public IDependency Dependency => new DependencyImplementation();

    //Implementing the appropriate property defined for Worker in the IDal interface, so that they return objects of the type that implements IWorker
    public IWorker Worker => new WorkerImplementation();

    //Implementing the appropriate property defined for Task in the IDal interface, so that they return objects of the type that implements ITask
    public ITask Task => new TaskImplementation();

    public IUser User => new UserImplementation();

   
}
