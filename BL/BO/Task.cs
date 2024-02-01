using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class Task
    {
        public int Id { get; set; } 
        public string Description { get; set; }
        public  string Alias { get; set; }
        public DateTime CreatedAtDate { get; set; }
        public Status Status { get; set; }
        public bool Eraseable { get; set; }
        public   List<TaskInList> Dependencies { get; set; }

        public TimeSpan RequiredEffortTime { get; set; }
        public DateTime? ScheduledDate { get; set; }

        public DateTime? StartDate { get; set; } = null;


        public DateTime? ForecastDate { get; set; }

        public DateTime? Deadline { get; set; }

        public DateTime? CompleteDate { get; set; } = null;

        public string? Deliverables { get; set; } = null;

        public string? Remarks { get; set; }


        public WorkerInTask Worker { get; set; }
        public WorkerExperience Complexity {  get; set; }


        public override string ToString()
        {

            string enumerableString = string.Join(",", Dependencies );

            return $"Id of Task:  { Id}  Alias {Alias}    Description: {Description}   Complexity: {Complexity}  Created at Date: { CreatedAtDate}   Status:{Status}   Dependencies {enumerableString}  Required effort time: {RequiredEffortTime}   Scheduled Date: {ScheduledDate} " +
                $"Start Date: {StartDate} Forecast Date: {ForecastDate}  Dead Line Date: {Deadline}    Complete Date: {CompleteDate}  Deliverables: {Deliverables}  Remarks: {Remarks}  Worker in Task: {Worker} "  ;
        }

    }
}
