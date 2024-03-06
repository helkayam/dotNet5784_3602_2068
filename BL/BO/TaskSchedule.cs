using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
   public  class TaskSchedule
    {
        public int Id { get; init; }


        public string Alias { get; init; }
        public string NameWorker { get; set; }
        public int  IdWorker { get; set; }

        public DateTime ScheduleStartDate { get; set; }

        public DateTime ScheduleEndDate { get; set; }

        public override string ToString()
        {
            return this.ToStringProperty();

        }


    }
}
