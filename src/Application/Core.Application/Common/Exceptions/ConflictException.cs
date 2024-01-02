namespace Core.Application.Common.Exceptions;

public class ConflictException : ApplicationException
{
    public ConflictException(string message, Exception inner = null) : base(message, inner)
    {

    }

    public ConflictException()
    {

    }
}
