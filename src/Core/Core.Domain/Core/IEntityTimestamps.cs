namespace Core.Domain.Core;

public interface IEntityTimestamps
{
    DateTime CreatedDate { get; }
    DateTime? UpdatedDate { get; }
    DateTime? DeletedDate { get; }

    void SetUpdatedDate(DateTime updatedDate);
    void SetDeletedDate(DateTime deletedDate);
}
