namespace Core.Domain.Core;

public abstract class AggregateRoot<TKey> : Entity<TKey>, IEntityTimestamps 
{
    public AggregateRoot()
    {
        Id = default;
        CreatedDate = DateTime.UtcNow;
    }

    public AggregateRoot(TKey id) => Id = id;

    public DateTime CreatedDate { get; private set; }
    public DateTime? UpdatedDate { get; private set; }
    public DateTime? DeletedDate { get; private set; }

    public void SetUpdatedDate(DateTime updatedDate) => UpdatedDate = updatedDate;
    public void SetDeletedDate(DateTime deletedDate) => DeletedDate = deletedDate;
}
