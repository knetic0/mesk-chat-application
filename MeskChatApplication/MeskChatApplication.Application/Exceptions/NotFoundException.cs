using System.Net;

namespace MeskChatApplication.Application.Exceptions;

public sealed class NotFoundException(string entity, object value) : ApplicationLogicBaseException($"{entity} {value} was not found!")
{
    public override HttpStatusCode StatusCode => HttpStatusCode.NotFound;
}