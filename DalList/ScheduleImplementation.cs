using DalApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal;

public class ScheduleImplementation : ISchedule
{
    public DateTime startdatePro;

    public DateTime? GetStartDateProject()
    {
        return startdatePro; 
    }

    public void UpdateStartDateProject(DateTime dt)
    {
        startdatePro = dt;
    }


  
}
