using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace Core.Domain.Filters;

public static class FilterHelper<TAggregate> where TAggregate : class
{
    public static IQueryable<TAggregate> ApplyFilters(IQueryable<TAggregate> queryable, bool enableTracking, bool withDeleted)
    {
        if (!enableTracking)
            queryable = queryable.AsNoTracking();

        if (withDeleted)
            queryable = queryable.IgnoreQueryFilters();

        return queryable;
    }

    public static IQueryable<TAggregate> ApplyFilters(IQueryable<TAggregate> queryable, bool enableTracking, bool withDeleted, Func<IQueryable<TAggregate>, IIncludableQueryable<TAggregate, object>> include = null)
    {
        if (!enableTracking)
            queryable = queryable.AsNoTracking();

        if (include != null)
            queryable = include(queryable);

        if (withDeleted)
            queryable = queryable.IgnoreQueryFilters();

        return queryable;
    }
}
