using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class TaskInList
    {
        public int Id {  get; init; }

        public string Description { get; set; }

        public string Alias { get; init; }

        public Status Status { get; set; }

        public override string ToString()
        {
            return $"Id: {Id}  Alias: {Alias}  Description: {Description}  Status: {Status}";
        }
    }
}
