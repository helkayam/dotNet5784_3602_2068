﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class TaskInWorker
    {
        public int Id { get; init; }
        public string Alias { get; init; }


        public override string ToString()
        {
            return this.ToStringProperty();
        }

    }
}
