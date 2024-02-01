using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace BlApi;

public interface IWorker
{
    public void AddWorker(BO.Worker newWorker);

    public void RemoveWorker(int Id);

    public void UpdateWorker(BO.Worker workerToUpdate);

    public BO.Worker? ReadWorker(int Id);

    public void deleteAll();

    public IEnumerable<BO.WorkerInList?> ReadAllWorkers(BO.FilterWorker enumFilter = BO.FilterWorker.None, Object? filtervalue = null);



}
