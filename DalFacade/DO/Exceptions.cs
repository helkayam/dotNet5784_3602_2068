

namespace DO;

[Serializable]
public class DalDoesNotExistException : Exception
{
    public DalDoesNotExistException(int id, string? Type) : base($"{Type} with id={id} doesn't exist")
    {

    }
}


public class DalAlreadyExistException : Exception
{
    public DalAlreadyExistException(int id, string? Type) : base($"{Type} with id={id} already exist")
    {

    }
}


public class DalNotErasableException : Exception
{
    public DalNotErasableException(int id, string? Type) : base($"{Type} with id={id} is not eraseable")
    {

    }
}

public class DalNotActiveException : Exception
{
    public DalNotActiveException(int id, string? Type) : base($"this {Type} with id={id} is not active")
    {
    }
}

