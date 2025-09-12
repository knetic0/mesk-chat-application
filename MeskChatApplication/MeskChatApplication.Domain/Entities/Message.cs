using MeskChatApplication.Domain.Abstractions;

namespace MeskChatApplication.Domain.Entities;

public sealed class Message : Entity
{
    public Guid SenderId { get; set; }
    public User Sender { get; init; } = default!;
    
    public Guid ReceiverId { get; set; }
    public User Receiver { get; init; } = default!;
    
    public string Text { get; init; } = string.Empty;
    
    public DateTime SendAt { get; init; } = DateTime.UtcNow;

    public bool IsRead { get; set; } = false;
    public DateTime? ReadAt { get; set; }

    public Message() { }

    public Message(Guid senderId, Guid receiverId, string text)
    {
        SenderId = senderId;
        ReceiverId = receiverId;
        Text = text;
    }

    public void MarkAsRead()
    {
        IsRead = true;
        ReadAt = DateTime.UtcNow;
    }
}