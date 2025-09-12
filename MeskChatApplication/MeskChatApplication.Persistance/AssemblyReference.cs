using System.Reflection;
using MeskChatApplication.Persistance.Context;

namespace MeskChatApplication.Persistance;

public static class AssemblyReference
{
    public static readonly Assembly Assembly = typeof(ApplicationDatabaseContext).Assembly;
}