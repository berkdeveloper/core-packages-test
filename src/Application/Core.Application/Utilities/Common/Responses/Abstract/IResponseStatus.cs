using Newtonsoft.Json;
using System.Net;

namespace Core.Application.Utilities.Common.Responses.Abstract;

public interface IResponseStatus
{
    [JsonProperty("statusCode")]
    public HttpStatusCode StatusCode { get; }
}
