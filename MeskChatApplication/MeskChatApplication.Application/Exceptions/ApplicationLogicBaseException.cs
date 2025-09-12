using System.Net;

namespace MeskChatApplication.Application.Exceptions;

public abstract class ApplicationLogicBaseException : Exception
{
    protected ApplicationLogicBaseException(string message) : base(message) { }

    public abstract HttpStatusCode StatusCode { get; }
}