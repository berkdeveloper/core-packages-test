namespace Core.Application.Exceptions;

public class ConflictException : ApplicationException
{
    public ConflictException()
    {

    }

    public ConflictException(string message, Exception innerException = null) : base(message, innerException)
    {

    }
}
