using System.Net;

namespace MeskChatApplication.Application.Exceptions;

public class UnauthorizedAccessException() : ApplicationLogicBaseException("Unauthorized!")
{
    public override HttpStatusCode StatusCode => HttpStatusCode.Unauthorized;
}