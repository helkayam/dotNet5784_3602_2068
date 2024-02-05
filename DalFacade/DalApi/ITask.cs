namespace DalApi;
using DO;

public interface ITask:ICrud<Task>
{

    public DateTime? GetStartDateProject();
    public void UpdateStartDateProject(DateTime startDate);

}
