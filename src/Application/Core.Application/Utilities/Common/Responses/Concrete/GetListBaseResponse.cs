using System.Net;

namespace Core.Application.Utilities.Common.Responses.Concrete;

public class GetListBaseResponse<T> : ListBaseResponse
{
    private IList<T> _items;

    public IList<T> Items
    {
        get => _items ??= new List<T>();
        set => _items = value;
    }

    public GetListBaseResponse() { }
    public GetListBaseResponse(List<T> data = null) => Items = data;
    public GetListBaseResponse(List<T> data, HttpStatusCode statusCode) : this(data) => StatusCode = statusCode;
    public GetListBaseResponse(HttpStatusCode statusCode, string errorMessage, List<T> data = null) : this(data, statusCode) => Message = errorMessage;
}
