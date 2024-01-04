namespace DO;

public record Dependency
(
int Id,
int DependentTask,
int DependsOnTask
)
{
    public Dependency() : this(0, 0, 0) { }
    public Dependency(int id, int dependentTask, int dependsOnTask)
    { Id = id;
        DependsOnTask = dependsOnTask;
        DependentTask = dependentTask; }  

}
