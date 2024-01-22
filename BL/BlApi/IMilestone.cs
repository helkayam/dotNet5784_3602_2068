using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;

namespace BlApi;

public interface IMilestone
{
    public void CreateSchedule();
    public Milestone? ReadMilestone(int Id);

    public Milestone UpdateMilestone(int Id);
}
