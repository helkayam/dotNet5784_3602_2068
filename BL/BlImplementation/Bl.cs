using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlImplementation;
using BlApi;
using System.Data.SqlTypes;


sealed internal class Bl : IBl
{
    private static readonly Lazy<Bl> LazyInstance = new Lazy<Bl>(() => new Bl());
    public static Bl Instance { get { return LazyInstance.Value; } }


    private DalApi.IDal _dal = DalApi.Factory.Get;


    //public IBl Clock;

    public DateTime Clock { private set; get; }

    private Bl()
    {
        Clock = (DateTime)_dal.Schedule.GetCurrentDate();
    }
    public void IncreasInHour()
    {
        Clock = Clock.AddHours(1);
        _dal.Schedule.UpdateCurrentDate(Clock);
    }

    public void IncreasInDay()
    {
        Clock = Clock.AddDays(1);
        _dal.Schedule.UpdateCurrentDate(Clock);
    }

    public void IncreasInWeek()
    {
        Clock = Clock.AddDays(7);
        _dal.Schedule.UpdateCurrentDate(Clock);

    }
    public void InitClock()
    {
        Clock=DateTime.Now;
        _dal.Schedule.UpdateCurrentDate(Clock);

    }
    public IUser User => new UserImplementation();

    public IWorker Worker => new WorkerImplementation(this);

    public ITask Task => new TaskImplementation(this);

    public ISchedule Schedule => new ScheduleImplementation(this);

    public void InitializeDB() => DalTest.Initialization.Do();

    public void ResetDB() => DalTest.Initialization.reset();


}
