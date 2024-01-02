using Core.Application.Utilities.Common.Responses.Abstract;
using Newtonsoft.Json;
using System.Net;

namespace Core.Application.Utilities.Common.Responses.ComplexTypes;

public class ResponseBase : IResponseStatus, IResponseMessage
{
    [JsonIgnore]
    public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.OK;

    public string Message { get; set; }

    public ResponseBase() { }

    public ResponseBase(HttpStatusCode statusCode, string message = null)
    {
        StatusCode = statusCode;
        Message = message;
    }
}
