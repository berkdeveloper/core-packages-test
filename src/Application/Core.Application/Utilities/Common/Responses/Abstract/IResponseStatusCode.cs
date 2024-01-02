using Newtonsoft.Json;

namespace Core.Application.Utilities.Common.Responses.Abstract
{
    public interface IResponseStatusCode
    {
        [JsonProperty("statusCode")]
        public int StatusCode { get; }
    }
}
