using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DalApi
{
    public interface IDal
    {
        IDependency Dependency { get; }
        IWorker Worker { get; }
        ITask task { get; }


    }
}
