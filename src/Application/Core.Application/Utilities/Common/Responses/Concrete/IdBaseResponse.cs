using Core.Application.Utilities.Common.Responses.Abstract;
using Core.Application.Utilities.Common.Responses.ComplexTypes;

namespace Core.Application.Utilities.Common.Responses.Concrete;

public class IdBaseResponse<T> : ResponseBase, IObjectResponse<T>
{
    public T Data { get; }
}

