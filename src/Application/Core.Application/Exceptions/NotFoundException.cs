namespace Core.Application.Exceptions;

public class NotFoundException : ApplicationException
{
    public NotFoundException()
    {

    }

    public NotFoundException(string message, Exception innerException = null) : base(message, innerException)
    {
    }
}
