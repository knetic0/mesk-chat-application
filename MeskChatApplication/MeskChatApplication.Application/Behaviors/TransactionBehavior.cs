using System.Reflection;
using MESK.MediatR;
using MeskChatApplication.Application.Attributes;
using MeskChatApplication.Application.Services;

namespace MeskChatApplication.Application.Behaviors;

public sealed class TransactionBehavior<TRequest, TResponse>(IUnitOfWork unitOfWork, IServiceProvider serviceProvider) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var handlerType = typeof(IRequestHandler<TRequest, TResponse>);
        var concreteHandlerType = _serviceProvider.GetService(handlerType)?.GetType();
        if(concreteHandlerType?.GetCustomAttribute<TransactionalAttribute>() is null) return await next();
        await _unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            var response = await next();
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await _unitOfWork.CommitTransactionAsync(cancellationToken);
            return response;
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }
}