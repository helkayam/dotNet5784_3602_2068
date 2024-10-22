﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;



namespace BlApi;

public interface IWorker
{
    public void AddWorker(BO.Worker newWorker);

    public void RemoveWorker(int Id);

    public void UpdateWorker(BO.Worker workerToUpdate);

    public BO.Worker? ReadWorker(int Id, bool throwexception = false);

    public void deleteAll();

    public IEnumerable<WorkerInTask?> ReadAllWorkersSuitabeToTask(int TaskId);
    public IEnumerable<BO.Worker> ReadAllSearch(string search);


    public IEnumerable<BO.Worker> ReadAllWorkers(BO.FilterWorker enumFilter = BO.FilterWorker.None, Object? filtervalue = null);

    public WorkerInTask returnWorkerInList(int id);



}
