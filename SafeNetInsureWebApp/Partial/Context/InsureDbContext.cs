using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SafeNetInsureWebApp.Models;

namespace SafeNetInsureWebApp.Context;

public partial class InsureDbContext
{
    public InsureDbContext(DbContextOptions<InsureDbContext> options)
        : base(options)
    {
        ChangeTracker.AutoDetectChangesEnabled = true;
        ChangeTracker.LazyLoadingEnabled = true;
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserInfo>(entity =>
        {
            entity.Property(e => e.Gender)
                .HasConversion<EnumToStringConverter<UserInfo.Sex>>()
                .HasDefaultValueSql("'Other'")
                .HasComment("Пол пользователя.");
        });
        modelBuilder.Entity<Policy>(entity =>
        {
            entity.Property(e => e.InsuranceType)
                .HasConversion<EnumToStringConverter<Policy.InsType>>()
                .HasDefaultValueSql("'Other'")
                .HasComment("Тип страхования.");
            entity.Property(e => e.PolicyNumber)
                .HasConversion<GuidToStringConverter>()
                .IsFixedLength()
                .HasComment("Уникальный идентификатор полиса.");
            entity.Property(e => e.Status)
                .HasConversion<EnumToStringConverter<Policy.StatusInsure>>()
                .HasDefaultValueSql("'Active'")
                .HasComment(
                    "Статус страховки (выплата была произведена в полном объёме и страховка не продлена, выплата не была произведена в полном объёме, страховка действует, страховка продлена).");
        });
    }
}