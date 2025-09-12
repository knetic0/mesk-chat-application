using System.Net;

namespace MeskChatApplication.Application.Exceptions;

public sealed class InvalidCredentialException() : ApplicationLogicBaseException("Invalid Email or Password!")
{
    public override HttpStatusCode StatusCode => HttpStatusCode.BadRequest;
}