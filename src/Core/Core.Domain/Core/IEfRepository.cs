using Microsoft.EntityFrameworkCore;

namespace Core.Domain.Core;

public interface IEfRepository<TAggregate, TKey> : IRepository where TAggregate : AggregateRoot<TKey>
{
    DbSet<TAggregate> Table { get; }
}
