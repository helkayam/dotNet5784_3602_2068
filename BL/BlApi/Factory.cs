namespace BlApi;
public static class Factory
{
    public static IBl Get() => BlImplementation.Bl.Instance;
}
