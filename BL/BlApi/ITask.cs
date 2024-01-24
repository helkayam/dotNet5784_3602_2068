using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlApi;

public interface ITask
{
    public void AddTask(BO.Task newTask);
    public void RemoveTask(int Id);

    public void UpdateTask(Task TaskToUpdate);

    public Task? ReadTask(int Id);

    public IEnumerable<BO.TaskInList?> ReadAllWorkers();


}
