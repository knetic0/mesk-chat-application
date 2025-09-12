using System.Transactions;

namespace MeskChatApplication.Application.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class TransactionalAttribute(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted) : Attribute
{
    public IsolationLevel IsolationLevel { get; } = isolationLevel;
}