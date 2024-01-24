


namespace BlImplementation;
using BlApi;
using BO;
using System;
using System.Collections.Generic;

internal class TaskImplementation : ITask
{
    public void AddTask(System.Threading.Tasks.Task newTask)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<TaskInList?> ReadAllWorkers(Func<System.Threading.Tasks.Task, bool>? filter = null)
    {
        throw new NotImplementedException();
    }

    public System.Threading.Tasks.Task? ReadTask(int Id)
    {
        throw new NotImplementedException();
    }

    public void RemoveTask(int Id)
    {
        throw new NotImplementedException();
    }

    public void UpdateTask(System.Threading.Tasks.Task TaskToUpdate)
    {
        throw new NotImplementedException();
    }
}
