using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;
using DO;
namespace BlApi;

public interface IBl
{

   
  
    public static Enum StatusProject { get; set; }
   


    public IWorker Worker { get; }
    public ITask Task { get; }

    public void ResetDB();
    public void InitializeDB();

    #region

    public DateTime Clock { get; }

    public void IncreasInHour();

    public void IncreasInDay();

    public void IncreasInWeek();

    public void InitClock();
    #endregion

}
