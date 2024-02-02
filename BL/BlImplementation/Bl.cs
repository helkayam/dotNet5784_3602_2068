using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlImplementation;
using BlApi;


internal class Bl : IBl
{


    public DateTime StartDateProject { get; set; }

    public DateTime EndDateProject { get; set; }

    public void UpdateStarteEndProjectDate(DateTime startDateProject, DateTime endDateProject)
    {
        
        StartDateProject = startDateProject;    
        EndDateProject = endDateProject;
    }

    public IWorker Worker => new WorkerImplementation();

    public ITask Task => new TaskImplementation();


}
