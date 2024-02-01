

namespace BO;

[Serializable]
public class BlDoesNotExistException : Exception
{
    public BlDoesNotExistException(string? message) : base(message) { }
    public BlDoesNotExistException(string message, Exception innerException)
                : base(message, innerException) { }
}

[Serializable]
public class BlAlreadyExistsException : Exception
{
    public BlAlreadyExistsException(string? message) : base(message) { }
    public BlAlreadyExistsException(string message, Exception innerException)
                : base(message, innerException) { }
}



[Serializable]
public class BlNotErasableException : Exception
{
    public BlNotErasableException(string? message) : base(message) { }
    public BlNotErasableException(string message, Exception innerException)
                : base(message, innerException) { }
}

[Serializable]
public class BlNotActiveException : Exception
{
    public BlNotActiveException(string? message) : base(message) { }
    public BlNotActiveException(string message, Exception innerException)
                : base(message, innerException) { }
}



[Serializable]
public class BlXMLFileLoadCreateException : Exception
{
    public BlXMLFileLoadCreateException(string? message) : base(message) { }
    public BlXMLFileLoadCreateException(string message, Exception innerException)
                : base(message, innerException) { }
}



[Serializable]
public class BlInvalidGivenValueException : Exception
{
    public BlInvalidGivenValueException(string? message) : base(message)
    {
    }
}


[Serializable]
public class BlForbiddenActionException : Exception
{
    public BlForbiddenActionException(string? message) : base(message)
    {
    }
}

[Serializable]
public class BlFalseUpdateDate : Exception
{
    public BlFalseUpdateDate(string? message) : base(message)
    {
    }
}


