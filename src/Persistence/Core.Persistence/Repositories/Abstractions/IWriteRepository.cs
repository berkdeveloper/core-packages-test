using Core.Domain.Core;

namespace Core.Persistence.Repositories.Abstractions;

public interface IWriteRepository<TAggregate, TKey>
    where TAggregate : AggregateRoot<TKey>
{
    void Add(TAggregate entity);
    void AddRange(ICollection<TAggregate> entities);
    void Update(TAggregate entity);
    void UpdateRange(ICollection<TAggregate> entities);
    void Delete(TAggregate entity, bool permanent = false);
    void DeleteRange(ICollection<TAggregate> entities, bool permanent = false);
}
