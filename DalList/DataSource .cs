namespace Dal
{
    internal static class DataSource
    {
        internal static List<DO.Task?> Tasks { get; } = new();
        internal static List<DO.Dependency?> Dependencies { get; } = new();
        internal static List<DO.Worker?> Workers { get; } = new();

    }
}
