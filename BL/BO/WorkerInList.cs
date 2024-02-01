using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class WorkerInList
    {
        public int Id { get; init; }
        public required string Name { get; set; }
        public TaskInWorker Task { get; init; }

        public override string ToString()
        {
            return $"Id: {Id}, Name:{Name}, Task:{Task}";
        }

    }
}
