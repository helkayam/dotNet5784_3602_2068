using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace PL;
internal class WorkerExperienceCollection : IEnumerable
{
    static readonly IEnumerable<BO.WorkerExperience> W_experience =
(Enum.GetValues(typeof(BO.WorkerExperience)) as IEnumerable<BO.WorkerExperience>)!;

    public IEnumerator GetEnumerator() => W_experience.GetEnumerator();
}

