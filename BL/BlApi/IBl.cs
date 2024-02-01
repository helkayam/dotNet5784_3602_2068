using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;

namespace BlApi
{
    public interface IBl
    {
        public DateTime StartDateProject { get; set; }

        public DateTime EndDateProject { get; set; }

        public BO.ProjectStatus GetStatusOfProject()
        {
            //if (StartDateProject == null)
            //    return BO.ProjectStatus.PlanStage;

            //if (StartDateProject != null)
            //{
            //    var taskswithoutdate = from x in 
            //}


            //return ProjectStatus.PlanStage;
        }
        

        public IWorker Worker { get; }
        public ITask Task { get; }
      

    }
}
