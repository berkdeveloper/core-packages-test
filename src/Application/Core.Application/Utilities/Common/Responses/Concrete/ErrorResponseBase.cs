using Core.Application.Utilities.Common.Responses.Abstract;
using System.Text.Json;

namespace Core.Application.Utilities.Common.Responses.Concrete;

public class ErrorResponseBase : IResponseException, IResponseStatusCode, IResponseMessage
{
    public int StatusCode { get; set; }
    public object Exception { get; set; }
    public string Message { get; set; }

    public ErrorResponseBase(object exception = null) => Exception = exception;
    public ErrorResponseBase(object exception, string message = null) : this(exception) => Message = message;
    public ErrorResponseBase(int statusCode, string message = null, object exception = null) : this(exception, message) => StatusCode = statusCode;

    public override string ToString() => JsonSerializer.Serialize(this);
}
