using Core.Domain.Core;
using Core.Domain.Filters;
using Core.Persistence.Dynamic;
using Core.Persistence.Paging;
using Core.Persistence.Paging.Extensions;
using Core.Persistence.Repositories.Abstractions;
using Core.Persistence.Repositories.Abstractions.Asynchronous;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Core.Persistence.Repositories.Concrete;

public class EfReadRepositoryBase<TAggregate, TKey, TContext> : IReadAsyncRepository<TAggregate, TKey>, IReadRepository<TAggregate, TKey>, IEfRepository<TAggregate, TKey>
    where TAggregate : AggregateRoot<TKey>
    where TContext : DbContext
{
    protected readonly TContext _context;

    public EfReadRepositoryBase(TContext context)
    {
        _context = context;
    }

    public DbSet<TAggregate> Table => _context.Set<TAggregate>();

    public bool Any(Expression<Func<TAggregate, bool>> predicate = null, bool withDeleted = false, bool enableTracking = true)
    {
        var queryable = Query();

        queryable = FilterHelper<TAggregate>.ApplyFilters(queryable, enableTracking, withDeleted);

        if (predicate != null)
            queryable = queryable.Where(predicate);

        return queryable.Any();
    }

    public async Task<bool> AnyAsync(Expression<Func<TAggregate, bool>> predicate = null, bool withDeleted = false, bool enableTracking = true, CancellationToken cancellationToken = default)
    {
        var queryable = Query();

        queryable = FilterHelper<TAggregate>.ApplyFilters(queryable, enableTracking, withDeleted);

        if (predicate != null)
            queryable = queryable.Where(predicate);

        return await queryable.AnyAsync(cancellationToken);
    }

    public TAggregate Get(Expression<Func<TAggregate, bool>> predicate, Func<IQueryable<TAggregate>, IIncludableQueryable<TAggregate, object>> include = null, bool withDeleted = false, bool enableTracking = true)
    {
        var queryable = Query();

        queryable = FilterHelper<TAggregate>.ApplyFilters(queryable, enableTracking, withDeleted, include);

        return queryable.FirstOrDefault(predicate);
    }

    public async Task<TAggregate> GetAsync(Expression<Func<TAggregate, bool>> predicate, Func<IQueryable<TAggregate>, IIncludableQueryable<TAggregate, object>> include = null, bool withDeleted = false, bool enableTracking = true, CancellationToken cancellationToken = default)
    {
        var queryable = Query();

        queryable = FilterHelper<TAggregate>.ApplyFilters(queryable, enableTracking, withDeleted, include);

        return await queryable.FirstOrDefaultAsync(predicate, cancellationToken);
    }

    public Paginate<TAggregate> GetList(Expression<Func<TAggregate, bool>> predicate = null, Func<IQueryable<TAggregate>, IOrderedQueryable<TAggregate>> orderBy = null, Func<IQueryable<TAggregate>, IIncludableQueryable<TAggregate, object>> include = null, int index = 0, int size = 10, bool withDeleted = false, bool enableTracking = true)
    {
        var queryable = Query();

        queryable = FilterHelper<TAggregate>.ApplyFilters(queryable, enableTracking, withDeleted, include);

        if (predicate != null)
            queryable = queryable.Where(predicate);

        if (orderBy != null)
            return orderBy(queryable).ToPaginate(index, size);

        return queryable.ToPaginate(index, size);
    }

    public async Task<Paginate<TAggregate>> GetListAsync(Expression<Func<TAggregate, bool>> predicate = null, Func<IQueryable<TAggregate>, IOrderedQueryable<TAggregate>> orderBy = null, Func<IQueryable<TAggregate>, IIncludableQueryable<TAggregate, object>> include = null, int index = 0, int size = 10, bool withDeleted = false, bool enableTracking = true, CancellationToken cancellationToken = default)
    {
        var queryable = Query();

        queryable = FilterHelper<TAggregate>.ApplyFilters(queryable, enableTracking, withDeleted, include);

        if (predicate != null)
            queryable = queryable.Where(predicate);

        if (orderBy != null)
            return await orderBy(queryable).ToPaginateAsync(index, size, cancellationToken);

        return await queryable.ToPaginateAsync(index, size, cancellationToken);
    }

    public Paginate<TAggregate> GetListByDynamic(DynamicQuery dynamic, Expression<Func<TAggregate, bool>> predicate = null, Func<IQueryable<TAggregate>, IIncludableQueryable<TAggregate, object>> include = null, int index = 0, int size = 10, bool withDeleted = false, bool enableTracking = true)
    {
        IQueryable<TAggregate> queryable = Query().ToDynamic(dynamic);

        queryable = FilterHelper<TAggregate>.ApplyFilters(queryable, enableTracking, withDeleted, include);

        if (predicate != null)
            queryable = queryable.Where(predicate);

        return queryable.ToPaginate(index, size);
    }

    public async Task<Paginate<TAggregate>> GetListByDynamicAsync(DynamicQuery dynamic, Expression<Func<TAggregate, bool>> predicate = null, Func<IQueryable<TAggregate>, IIncludableQueryable<TAggregate, object>> include = null, int index = 0, int size = 10, bool withDeleted = false, bool enableTracking = true, CancellationToken cancellationToken = default)
    {
        IQueryable<TAggregate> queryable = Query().ToDynamic(dynamic);

        queryable = FilterHelper<TAggregate>.ApplyFilters(queryable, enableTracking, withDeleted, include);

        if (predicate != null)
            queryable = queryable.Where(predicate);

        return await queryable.ToPaginateAsync(index, size, cancellationToken);
    }

    public IQueryable<TAggregate> Query() => Table.AsQueryable();
}
