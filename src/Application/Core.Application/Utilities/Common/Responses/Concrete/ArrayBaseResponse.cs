using System.Net;
using Core.Application.Utilities.Common.Responses.ComplexTypes;

namespace Core.Application.Utilities.Common.Responses.Concrete;

public class ArrayBaseResponse<T> : ArrayItemsBaseResponse
{
    private IList<T> _items;

    public IList<T> Items
    {
        get => _items ??= new List<T>();
        set => _items = value;
    }

    public ArrayBaseResponse() { }
    public ArrayBaseResponse(List<T> data = null) => Items = data;
    public ArrayBaseResponse(List<T> data, HttpStatusCode statusCode) : this(data) => StatusCode = statusCode;
    public ArrayBaseResponse(HttpStatusCode statusCode, string errorMessage, List<T> data = null) : this(data, statusCode) => Message = errorMessage;
}
