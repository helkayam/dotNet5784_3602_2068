using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class Task
    {
        public int Id { get; init; } 
        public string Description { get; set; }
        public  string Alias { get; set; }
        public DateTime CreatedAtDate { get; init; }
        public Status Status { get; set; }
        public List<TaskInList> Dependencies { get; set; }

        public MilestoneInTask Milestone { get; set; } 
        public TimeSpan RequiredEffortTime { get; set; }
        public DateTime? ScheduledDate { get; init; }

        public DateTime? StartDate { get; init; }


        public DateTime ForecastDate { get; init; }

        public DateTime? Deadline { get; set; }

        public DateTime? CompleteDate { get; init; }

        public string? Deliverables { get; init; }

        public string? Remarks { get; set; }


        public WorkerInTask Worker { get; set; }
        public WorkerExperience Complexity {  get; init; }




    }
}
