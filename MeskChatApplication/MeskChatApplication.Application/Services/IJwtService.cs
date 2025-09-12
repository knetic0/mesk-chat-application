using MeskChatApplication.Domain.Entities;

namespace MeskChatApplication.Application.Services;

public interface IJwtService
{
    string Generate(User user);
}