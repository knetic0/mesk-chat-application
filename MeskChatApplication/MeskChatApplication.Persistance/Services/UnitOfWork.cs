using MeskChatApplication.Application.Services;
using MeskChatApplication.Persistance.Context;
using Microsoft.EntityFrameworkCore.Storage;

namespace MeskChatApplication.Persistance.Services;

public sealed class UnitOfWork(ApplicationDatabaseContext context) : IUnitOfWork
{
    private readonly ApplicationDatabaseContext _context = context;
    private IDbContextTransaction? _transaction;

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        if(_transaction is not null) throw new InvalidOperationException("Transaction has already been started.");
        _transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        if(_transaction is null) throw new InvalidOperationException("Transaction not started.");
        await _transaction.CommitAsync(cancellationToken);
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        if(_transaction is null) throw new InvalidOperationException("Transaction not started.");
        await _transaction.RollbackAsync(cancellationToken);
    }

    public void Dispose()
    {
        _transaction?.Dispose();
    }
}