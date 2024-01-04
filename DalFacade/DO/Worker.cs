
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
    public bool eraseAbale { get; set; } = false;
    public bool active { get; set; } = true;

}
