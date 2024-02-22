using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DalApi;

///This is a class that inherits and implements the IDal interface by initializing the sub-interfaces in the access classes that we implemented with XML.
namespace Dal
{
   sealed  internal class DalXml : IDal
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
        private static readonly Lazy<DalXml> LazyInstance = new Lazy<DalXml>(() => new DalXml());
        private DalXml() { }
        public static DalXml Instance { get { return LazyInstance.Value; } }


        public ISchedule Schedule => new ScheduleImplementation();
        public IDependency Dependency => new DependencyImplementation();

        public IWorker Worker => new WorkerImplementation();

        public ITask Task => new TaskImplementation();

        public IUser User => new UserImplementation();  
    }
}
