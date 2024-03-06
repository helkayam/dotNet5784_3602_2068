using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlApi;

public interface ISchedule
{
    public DateTime? getStartDateProject();
    public DateTime? getEndDateProject();

    public void UpdateCurrentDate(DateTime UpdateCurrentDate);

    public void UpdateStartProjectDate(DateTime startDateProject);

    public void UpdateEndDateProjectDate(DateTime endDateProject);

}
