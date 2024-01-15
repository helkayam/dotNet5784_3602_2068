using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DalApi;

namespace Dal
{
   sealed  public class DalXml : IDal
    {
        public IDependency Dependency => new DependencyImplementation();

        public IWorker Worker => new WorkerImplementation();

        public ITask Task => new TaskImplementation();
    }
}
