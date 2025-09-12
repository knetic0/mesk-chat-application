using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MeskChatApplication.Domain.Abstractions;

namespace MeskChatApplication.Domain.Entities;

public sealed class RefreshToken : Entity
{
    [Required]
    public string Token { get; init; } = default!;
    public bool IsUsed { get; set; } = false;
    public bool IsRevoked { get; set; } = false;
    public DateTime Expires { get; init; } = DateTime.UtcNow.AddDays(1);
    
    [Required]
    public Guid UserId { get; init; }
    
    public User User { get; init; } = default!;
    
    [NotMapped]
    public bool IsActive => !IsUsed && !IsRevoked && Expires > DateTime.UtcNow;

    public RefreshToken() { }

    public RefreshToken(string token, Guid userId)
    {
        UserId = userId;
        Token = token;
    }

    public void MarkUsed()  => IsUsed = true;
    public void MarkRevoked() => IsRevoked = true;
}