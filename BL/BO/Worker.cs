using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class Worker
    {
        public int Id {  get; init; }
        public required string Name { get; set; }
        public WorkerExperience Level { get; set; }    

        public string? PhoneNumber { get; set; }
        public double? Cost { get; set; }
        public TaskInWorker Task { get; init; }
        public bool active { get; set; }    
    }
}
