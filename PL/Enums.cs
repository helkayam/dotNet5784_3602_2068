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

public class FilterTask : IEnumerable
{
    static readonly IEnumerable<BO.Filter> t_enums = (Enum.GetValues(typeof(BO.Filter)) as IEnumerable<BO.Filter>)!;

    public IEnumerator GetEnumerator() => t_enums.GetEnumerator();
}

public class WorkerExperienceCollection : IEnumerable
{
    static readonly IEnumerable<BO.WorkerExperience> W_experience = 
        (Enum.GetValues(typeof(BO.WorkerExperience)) as IEnumerable<BO.WorkerExperience>)!;

    public IEnumerator GetEnumerator() => W_experience.GetEnumerator();
}


public class StatusTaskCollection : IEnumerable
{
    static readonly IEnumerable<BO.Status> t_status =
        (Enum.GetValues(typeof(BO.Status)) as IEnumerable<BO.Status>)!;

    public IEnumerator GetEnumerator() => t_status.GetEnumerator();
}

