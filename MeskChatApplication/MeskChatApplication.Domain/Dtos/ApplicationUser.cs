using MeskChatApplication.Domain.Enums;

namespace MeskChatApplication.Domain.Dtos;

public record ApplicationUser(Guid Id, string FirstName, string LastName, string Email, string Username, Status Status, DateTime? LastOnlineAt);