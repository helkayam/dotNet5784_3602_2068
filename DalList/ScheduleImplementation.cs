using DalApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal;

public class ScheduleImplementation : ISchedule
{

    public DateTime? GetStartDateProject()
    {
        return DataSource.Config.startdateProject; 
    }

    public void UpdateStartDateProject(DateTime dt)
    {
        DataSource.Config.startdateProject = dt;
    }

    public DateTime? GetCurrentDate()
    {
        return DataSource.Config.CurrentDate;
    }

    public void UpdateCurrentDate(DateTime dt)
    {
        DataSource.Config.CurrentDate  = dt;
    }


}
