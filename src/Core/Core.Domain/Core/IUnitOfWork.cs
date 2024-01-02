using System.Data;

namespace Core.Domain.Core;

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync();
    Task BeginTransactionAsync(string transactionKey);
    Task TransactionCommitAsync(string transactionKey);
    Task RollbackTransactionAsync();
    bool HasTransaction { get; }
    IDbTransaction Transaction { get; }
}
