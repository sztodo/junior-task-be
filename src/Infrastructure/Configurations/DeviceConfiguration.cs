using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public class DeviceConfiguration : IEntityTypeConfiguration<Device>
{
    public void Configure(EntityTypeBuilder<Device> builder)
    {
        builder.ToTable("Devices");

        builder.HasKey(d => d.Id);

        builder.HasQueryFilter(d => !d.IsDeleted);

        builder.Property(d => d.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(d => d.Manufacturer)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(d => d.Type)
            .IsRequired()
            .HasConversion<int>();  // Enum stored as INT: 1=Phone, 2=Tablet

        builder.Property(d => d.OperatingSystem)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(d => d.OsVersion)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(d => d.Processor)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(d => d.RamAmount)
            .IsRequired();

        builder.Property(d => d.Description)
            .HasMaxLength(1000);

        builder.Property(d => d.CreatedAt)
            .IsRequired();

        builder.Property(d => d.UpdatedAt)
            .IsRequired();

        builder.Property(d => d.DeletedAt);

        builder.Property(d => d.IsDeleted);

        builder.HasOne(d => d.AssignedUser)
            .WithMany(u => u.AssignedDevices)
            .HasForeignKey(d => d.AssignedUserId)
            .OnDelete(DeleteBehavior.SetNull)
            .IsRequired(false);

        builder.HasIndex(d => d.AssignedUserId)
            .HasDatabaseName("IX_Devices_AssignedUserId");
    }

}
