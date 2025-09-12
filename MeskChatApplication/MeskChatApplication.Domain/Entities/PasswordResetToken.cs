using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MeskChatApplication.Domain.Abstractions;

namespace MeskChatApplication.Domain.Entities;

public sealed class PasswordResetToken : Entity
{
    [Required]
    public Guid UserId { get; init; }

    public User User { get; init; } = default!;

    public string Token { get; init; } = default!;
    
    public DateTime Expires { get; init; } = DateTime.UtcNow;

    public bool IsUsed { get; set; } = false;
    
    public DateTime? UsedAt { get; set; }

    public PasswordResetToken() { }

    public PasswordResetToken(Guid userId, string token)
    {
        UserId = userId;
        Token = token;
    }

    public void MarkAsUsed()
    {
        IsUsed = true;
        UsedAt = DateTime.UtcNow;
    }
}