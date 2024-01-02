namespace Core.Domain.Core;

public interface IQuery<T>
{
    IQueryable<T> Query();
}
