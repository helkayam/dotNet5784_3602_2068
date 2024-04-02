using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DalApi;

public interface ISchedule
{
    public DateTime? GetStartDateProject();

    public DateTime? getEndDateProject();

    public void UpdateStartDateProject(DateTime startDate);

    public void UpdateEndDateProject(DateTime endDateProject);

    public DateTime? GetCurrentDate();

    public void UpdateCurrentDate(DateTime dt);

    public void ResetEndStartDateProject();




}
