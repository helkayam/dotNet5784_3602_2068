using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DalApi
{
    public interface IDal
    {

        /// <summary>
        /// attribute for the type of the dependency interface read only
        /// </summary>
        IDependency Dependency { get; }
        /// <summary>
        /// attribute for a type of the worker interface read only
        /// </summary>
        IWorker Worker { get; }
        /// <summary>
        /// attribute for the type of the task interface read only
        /// </summary>
        ITask Task { get; }
        
        ISchedule Schedule { get; }


    }
}
