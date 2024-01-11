
namespace DO;
/// <summary>
/// Workers entityrepresent a worker with all its props
/// </summary>
/// <param name="Id">ID of the worker >
/// <param name="Level">Professional Level of the worker>
/// <param name="Name">First Name and Last name of the worker >
/// <param name="PhoneNumber">Phone number of the workers>
/// <param name="Cost">salary per hour of the worker depending on his level>
public record Worker
(
    int Id,
    WorkerExperience Level,
    string Name,
string? PhoneNumber=null,
double? Cost=null

)
{
    public Worker(): this(0,0," ") { }//empty constructor of Worker
    /// <summary>
    /// This is a property that tells the programmer whether the entity can be deleted or not
    /// </summary>
    public bool Eraseable { get; set; } = false;
    /// <summary>
    /// This is a property that indicates to the programmer whether this employee is considered active or not
    /// </summary>
    public bool active { get; set; } = true;

}
