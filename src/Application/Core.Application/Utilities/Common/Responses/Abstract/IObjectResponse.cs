namespace Core.Application.Utilities.Common.Responses.Abstract;

public interface IObjectResponse<out T>
{
    public T Data { get; }
}
