using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PL;

public class FilterWorker:IEnumerable
{
    static readonly IEnumerable<BO.FilterWorker> w_enums = (Enum.GetValues(typeof(BO.FilterWorker)) as IEnumerable<BO.FilterWorker>)!;

    public IEnumerator GetEnumerator ()=> w_enums.GetEnumerator();
}

public class WorkerExperienceCollection : IEnumerable
{
    static readonly IEnumerable<BO.WorkerExperience> W_experience = 
        (Enum.GetValues(typeof(BO.WorkerExperience)) as IEnumerable<BO.WorkerExperience>)!;

    public IEnumerator GetEnumerator() => W_experience.GetEnumerator();
}

