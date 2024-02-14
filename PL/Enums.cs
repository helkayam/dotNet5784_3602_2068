using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PL;

internal class FilterWorker:IEnumerable
{
    static readonly IEnumerable<BO.FilterWorker> w_enums = (Enum.GetValues(typeof(BO.FilterWorker)) as IEnumerable<BO.FilterWorker>)!;

    public IEnumerator GetEnumerator ()=> w_enums.GetEnumerator();
}

internal class WorkerExperience : IEnumerable
{
    static readonly IEnumerable<BO.WorkerExperience> we_enums = (Enum.GetValues(typeof(BO.WorkerExperience)) as IEnumerable<BO.WorkerExperience>)!;

    public IEnumerator GetEnumerator() => we_enums.GetEnumerator();
}
