using MeskChatApplication.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MeskChatApplication.Persistance.Configurations;

public sealed class PasswordResetTokenConfiguration : IEntityTypeConfiguration<PasswordResetToken>
{
    public void Configure(EntityTypeBuilder<PasswordResetToken> builder)
    {
        builder.ToTable("PasswordResetTokens");   
        builder.HasOne(u => u.User).WithMany(u => u.PasswordResetTokens).HasForeignKey(x => x.UserId);
    }
}