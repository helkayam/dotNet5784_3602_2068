using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DalApi;

///This is a class that inherits and implements the IDal interface by initializing the sub-interfaces in the access classes that we implemented with XML.
namespace Dal
{
   sealed  public class DalXml : IDal
    {
        public IDependency Dependency => new DependencyImplementation();

        public IWorker Worker => new WorkerImplementation();

        public ITask Task => new TaskImplementation();
    }
}
