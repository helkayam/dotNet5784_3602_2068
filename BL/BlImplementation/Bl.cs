using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlImplementation;
using BlApi;


internal class Bl : IBl
{



    //public IBl Clock;

    public DateTime Clock { private set; get;}= DateTime.Now;

    public void IncreasInHour()
    {
        Clock = Clock.AddHours(1);
    }

    public void IncreasInDay()
    {
        Clock = Clock.AddDays(1);

    }

    public void IncreasInWeek()
    {
        Clock = Clock.AddDays(7);

    }
    public void InitClock()
    {
        Clock=DateTime.Now;
    }


    public IWorker Worker => new WorkerImplementation(this);

    public ITask Task => new TaskImplementation(this);

    public void InitializeDB() => DalTest.Initialization.Do();

    public void ResetDB() => DalTest.Initialization.reset();


}
