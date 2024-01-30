using Core.Application.Utilities.Common.Responses.Abstract;

namespace Core.Application.Utilities.Common.Responses.ComplexTypes;

public class ArrayItemsBaseResponse : ResponseBase, IPaginateResponse
{
    public int Size { get; set; }

    public int Index { get; set; }

    public int Count { get; set; }

    public int Pages { get; set; }

    public bool HasPrevious => Index > 0;

    public bool HasNext => Index + 1 < Pages;
}

