
#nullable enable
namespace DO;
/// <summary>
/// This is an entity responsible for the tasks of the workers in the project,
/// from fields such as the description of the task, the employee who is responsible,
/// etc...
/// </summary>
/// <param name="Id">Task ID number</param>
/// <param name="Alias">Mission description</param>
/// <param name="ScheduledDate">Scheduled date for the start of the task</param>
/// <param name="DeadLineDate">Planned date for completion of the task</param>
/// <param name="Complexity">The difficulty level of the task</param>
/// <param name="Description">A brief description of the mission</param>
/// <param name="WorkerId">ID number of the employee on the assignment </param>
public record Task
(
 string Alias,
WorkerExperience Complexity,
string Description,
int Id = -1,

DateTime? ScheduledDate=null,
DateTime? DeadLineDate = null,

int? WorkerId=null
)
{
    /// <summary>
    /// This is a boolean variable, its value will be False in all tasks.
    /// Later a task type entity will be able to represent a milestone and then its value can also be True.
    /// </summary>
    public bool IsMileStone { get; set; } = false;

    /// <summary>
    /// Project start date
    /// </summary>
    public DateTime? StartDate { get; set; } = null;

    /// <summary>
    /// This is a property that tells the programmer whether the entity can be deleted or not
    /// </summary>
    public bool Eraseable { get; set; } = true;

    public TimeSpan RequiredEffortTime {  get; set; } = TimeSpan.Zero;
    /// <summary>
    /// Project completion date
    /// </summary>
    public DateTime? CompleteDate { get; set; } = null;

    /// <summary>
    /// The final product of the task
    /// </summary>
    public string? Deliverables { get; set; } = null;

    /// <summary>
    /// Comments of the employees about the task performed
    /// </summary>
    public string? Remarks { get; set; } = null;

    /// <summary>
    /// Date the task was created
    /// </summary>
    public DateTime CreatedAtDate { get; set; } =DateTime.Now;

    public Task():this(" ", 0," ",-1) { }//empty constructor
}
