using System.Net;

namespace MeskChatApplication.Application.Exceptions;

public sealed class UserExistsException() : ApplicationLogicBaseException("User already exists.")
{
    public override HttpStatusCode StatusCode => HttpStatusCode.Conflict;
}