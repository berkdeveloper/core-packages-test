namespace Core.Domain.Core;

public abstract class Entity<TKey>
{
    public TKey Id { get; protected set; }
}
