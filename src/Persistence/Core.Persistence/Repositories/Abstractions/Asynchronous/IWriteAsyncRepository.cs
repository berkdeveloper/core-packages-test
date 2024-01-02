using Core.Domain.Core;

namespace Core.Persistence.Repositories.Abstractions.Asynchronous;

public interface IWriteAsyncRepository<TAggregate, TKey> where TAggregate : AggregateRoot<TKey>
{
    Task AddAsync(TAggregate entity);

    Task AddRangeAsync(ICollection<TAggregate> entities);

    Task DeleteAsync(TAggregate entity, bool permanent = false);

    Task DeleteRangeAsync(ICollection<TAggregate> entities, bool permanent = false);
}
