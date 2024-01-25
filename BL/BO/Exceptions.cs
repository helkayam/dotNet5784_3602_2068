

namespace BO;

[Serializable]
public class BlDoesNotExistException : Exception
{
    public BlDoesNotExistException(string? message) : base(message) { }
    public BlDoesNotExistException(string message, Exception innerException)
                : base(message, innerException) { }
}

[Serializable]
public class BlAlreadyExistException : Exception
{
    public BlAlreadyExistException(string? message) : base(message) { }
    public BlAlreadyExistException(string message, Exception innerException)
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

