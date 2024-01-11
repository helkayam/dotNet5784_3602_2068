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
sealed public class DalList : IDal
{
    //Implementing the appropriate property defined for dependencies in the IDal interface, so that they return objects of the type that implements IDendency
    public IDependency Dependency => new DependencyImplementation();

    //Implementing the appropriate property defined for Worker in the IDal interface, so that they return objects of the type that implements IWorker
    public IWorker Worker => new WorkerImplementation();

    //Implementing the appropriate property defined for Task in the IDal interface, so that they return objects of the type that implements ITask
    public ITask Task => new TaskImplementation();

   
}
