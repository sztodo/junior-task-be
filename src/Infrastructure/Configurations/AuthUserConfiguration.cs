using Domain.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public class AuthUserConfiguration : IEntityTypeConfiguration<AuthUser>
{
    public void Configure(EntityTypeBuilder<AuthUser> builder)
    {
        builder.ToTable("AuthUsers");
        builder.HasKey(a => a.Id);

        builder.Property(a => a.Id)
            .ValueGeneratedOnAdd()
            .UseIdentityColumn();

        builder.Property(a => a.Email)
            .IsRequired()
            .HasMaxLength(256);

        builder.HasIndex(a => a.Email)
            .IsUnique()
            .HasDatabaseName("IX_AuthUsers_Email");

        builder.Property(a => a.PasswordHash)
            .IsRequired()
            .HasMaxLength(512);

        builder.Property(a => a.Role)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(a => a.CreatedAt)
            .IsRequired();

        builder.Property(d => d.DeletedAt);

        builder.Property(d => d.IsDeleted);

        builder.HasOne(a => a.LinkedUser)
            .WithMany()
            .HasForeignKey(a => a.LinkedUserId)
            .OnDelete(DeleteBehavior.SetNull)
            .IsRequired(false);
    }

}
