namespace DO;

public record Worker
(
    int Id,
    WorkerExperience Level,
    string Name,
string? PhoneNumber=null,
double? Cost=null

)
{
    public Worker(): this(0,0," ") { }
   

}
