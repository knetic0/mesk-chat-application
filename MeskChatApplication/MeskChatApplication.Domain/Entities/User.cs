using System.ComponentModel.DataAnnotations;
using MeskChatApplication.Domain.Abstractions;
using MeskChatApplication.Domain.Enums;

namespace MeskChatApplication.Domain.Entities;

public sealed class User : Entity
{
    [MaxLength(30)]
    [MinLength(3)]
    [Required]
    public string FirstName { get; init; } = default!;
    
    [MaxLength(30)]
    [MinLength(3)]
    [Required]
    public string LastName { get; init; } = default!;

    [EmailAddress] [Required] public string Email { get; init; } = default!;

    [MaxLength(30)]
    [MinLength(3)]
    [Required]
    public string Username { get; init; } = default!;

    public byte[] PasswordHash { get; set; } = default!;
    public byte[] PasswordSalt { get; set; } = default!;

    public Status? Status { get; set; } = Enums.Status.Offline;
    public DateTime? LastOnlineAt { get; init; }
    
    public ICollection<RefreshToken> RefreshTokens { get; init; } = new List<RefreshToken>();
    public ICollection<PasswordResetToken> PasswordResetTokens { get; init; } = new List<PasswordResetToken>();
    public ICollection<Message> SentMessages { get; init; } = new List<Message>();
    public ICollection<Message> ReceivedMessages { get; init; } = new List<Message>();

    public User() { }

    public User(string firstName, string lastName, string email, string username, byte[] passwordHash, byte[] passwordSalt)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Username = username;
        PasswordHash = passwordHash;
        PasswordSalt = passwordSalt;
    }

    public void ChangePassword(byte[] newPasswordHash, byte[] newPasswordSalt)
    {
        PasswordHash = newPasswordHash;
        PasswordSalt = newPasswordSalt;
    }

    public void SetStatus(Status status)
    {
        Status = status;
    }
}