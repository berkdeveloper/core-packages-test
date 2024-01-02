using Core.Domain.Core;
using Core.Persistence.Repositories.Abstractions;
using Core.Persistence.Repositories.Abstractions.Asynchronous;
using Core.Persistence.Repositories.Helpers;
using Microsoft.EntityFrameworkCore;

namespace Core.Persistence.Repositories.Concrete;

public class EfWriteRepositoryBase<TAggregate, TKey, TContext> : IWriteAsyncRepository<TAggregate, TKey>, IWriteRepository<TAggregate, TKey>, IEfRepository<TAggregate, TKey>
    where TAggregate : AggregateRoot<TKey>
    where TContext : DbContext
{

    protected readonly TContext _context;

    public EfWriteRepositoryBase(TContext context) => _context = context;

    public DbSet<TAggregate> Table => _context.Set<TAggregate>();

    public void Add(TAggregate entity)
    {
        Table.Add(entity);
    }

    public async Task AddAsync(TAggregate entity) => await Table.AddAsync(entity);

    public void AddRange(ICollection<TAggregate> entities) => Table.AddRange(entities);


    public async Task AddRangeAsync(ICollection<TAggregate> entities) => await Table.AddRangeAsync(entities);


    public void Delete(TAggregate entity, bool permanent = false)
        => EntityHelper<TAggregate, TKey, TContext>.SetEntityAsDeleted(entity, permanent, _context);

    public async Task DeleteAsync(TAggregate entity, bool permanent = false)
        => await EntityHelper<TAggregate, TKey, TContext>.SetEntityAsDeletedAsync(entity, permanent, _context);


    public void DeleteRange(ICollection<TAggregate> entities, bool permanent = false)
        => EntityHelper<TAggregate, TKey, TContext>.SetEntityAsDeleted(entities, permanent, _context);

    public async Task DeleteRangeAsync(ICollection<TAggregate> entities, bool permanent = false)
        => await EntityHelper<TAggregate, TKey, TContext>.SetEntityAsDeletedAsync(entities, permanent, _context);

    public void Update(TAggregate entity)
    {
        entity.SetUpdatedDate(DateTime.UtcNow);
        Table.Update(entity);
    }

    public void UpdateRange(ICollection<TAggregate> entities)
    {
        foreach (var entity in entities)
            entity.SetUpdatedDate(DateTime.UtcNow);

        Table.UpdateRange(entities);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
