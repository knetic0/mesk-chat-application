using MeskChatApplication.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MeskChatApplication.Persistance.Configurations;

public sealed class MessageConfiguration : IEntityTypeConfiguration<Message>
{
    public void Configure(EntityTypeBuilder<Message> builder)
    {
        builder.ToTable("Messages");
        builder.HasIndex(m => m.Text);
        builder.HasOne(u => u.Sender).WithMany(u => u.SentMessages).HasForeignKey(x => x.SenderId);
        builder.HasOne(u => u.Receiver).WithMany(u => u.ReceivedMessages).HasForeignKey(x => x.ReceiverId);
    }
}