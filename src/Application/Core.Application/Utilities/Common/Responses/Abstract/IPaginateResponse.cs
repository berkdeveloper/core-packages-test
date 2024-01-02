namespace Core.Application.Utilities.Common.Responses.Abstract;

public interface IPaginateResponse
{
    public int Size { get; }
    public int Index { get; }
    public int Count { get; }
    public int Pages { get; }
    public bool HasPrevious { get; }
    public bool HasNext { get; }
}

