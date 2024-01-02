using Core.Domain.Core;
using MediatR;

namespace Core.Application.Pipelines.Behaviors.Transaction;

public class TransactionScopeBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>, ITransactionalCommand
{
    private readonly IUnitOfWork _unitOfWork;

    public TransactionScopeBehavior(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        await _unitOfWork.BeginTransactionAsync(typeof(TRequest).GUID.ToString());

        var handle = await next.Invoke();

        await _unitOfWork.TransactionCommitAsync(typeof(TRequest).GUID.ToString());

        return handle;
    }
}
