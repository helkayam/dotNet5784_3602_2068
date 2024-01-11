

namespace DO;


/// <summary>
/// Exception Throws an exception for an entity with an ID number that does not exist in the list
/// </summary>
[Serializable]
public class DalDoesNotExistException : Exception
{
    public DalDoesNotExistException( string? message) : base(message)//constructor
    {

    }
}


/// <summary>
/// Throws an exception for an entity with an ID number that already exists in the list
/// </summary>
 [Serializable]
public class DalAlreadyExistException : Exception
{
    public DalAlreadyExistException( string? message) : base(message)
    {

    }
}


/// <summary>
/// Throws an exception for deleting an entity that is not allowed to be deleted
/// </summary>
[Serializable]
public class DalNotErasableException : Exception
{
    public DalNotErasableException( string? message) : base(message)
    {

    }
}

/// <summary>
/// Throws an exception for accessing an entity of type Employee that is not active
/// </summary>
[Serializable]
public class DalNotActiveException : Exception
{
    public DalNotActiveException(string? message) : base(message)
    {
    }
}

