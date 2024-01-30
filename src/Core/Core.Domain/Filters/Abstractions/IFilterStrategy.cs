namespace Core.Domain.Filters.Abstractions;

public interface IFilterStrategy<TAggregate>
{
    IQueryable<TAggregate> ApplyFilter(IQueryable<TAggregate> queryable);
}
