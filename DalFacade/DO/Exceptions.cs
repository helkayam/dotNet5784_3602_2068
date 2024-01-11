

namespace DO;

[Serializable]
public class DalDoesNotExistException : Exception
{
    public DalDoesNotExistException( string? message) : base(message)
    {

    }
}


public class DalAlreadyExistException : Exception
{
    public DalAlreadyExistException( string? message) : base(message)
    {

    }
}


public class DalNotErasableException : Exception
{
    public DalNotErasableException( string? message) : base(message)
    {

    }
}

public class DalNotActiveException : Exception
{
    public DalNotActiveException(string? message) : base(message)
    {
    }
}

