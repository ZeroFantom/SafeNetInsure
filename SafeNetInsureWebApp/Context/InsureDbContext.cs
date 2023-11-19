using Microsoft.EntityFrameworkCore;
using SafeNetInsureWebApp.Models;

namespace SafeNetInsureWebApp.Context;

public partial class InsureDbContext : DbContext
{
    public virtual DbSet<ConditionPolicy> ConditionPolicies { get; set; }

    public virtual DbSet<ObjectInsurance> ObjectInsurances { get; set; }

    public virtual DbSet<Policy> Policies { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<RoleHasUser> RoleHasUsers { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserInfo> UserInfos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb3_general_ci")
            .HasCharSet("utf8mb3");

        modelBuilder.Entity<ConditionPolicy>(entity =>
        {
            entity.HasKey(e => e.IdConditionPolicy).HasName("PRIMARY");

            entity.Property(e => e.Condition).HasComment("Текст условия страхования (подробно).");
            entity.Property(e => e.Title).HasComment("Условие страхования.");
        });

        modelBuilder.Entity<ObjectInsurance>(entity =>
        {
            entity.HasKey(e => e.IdObjectInsurance).HasName("PRIMARY");

            entity.Property(e => e.DataObjectInsurance).HasComment("Данные объекта страхования.");
            entity.Property(e => e.Description).HasComment("Описание объекта страхования.");
            entity.Property(e => e.Title).HasComment("Наименование объекта страхования.");

            entity.HasMany(d => d.PolicyIdPolicies).WithMany(p => p.ObjectInsuranceIdObjectInsurances)
                .UsingEntity<Dictionary<string, object>>(
                    "ObjectInsuranceHasPolicy",
                    r => r.HasOne<Policy>().WithMany()
                        .HasForeignKey("PolicyIdPolicy")
                        .HasConstraintName("fk_ObjectInsurance_has_Policy_Policy1"),
                    l => l.HasOne<ObjectInsurance>().WithMany()
                        .HasForeignKey("ObjectInsuranceIdObjectInsurance")
                        .HasConstraintName("fk_ObjectInsurance_has_Policy_ObjectInsurance1"),
                    j =>
                    {
                        j.HasKey("ObjectInsuranceIdObjectInsurance", "PolicyIdPolicy")
                            .HasName("PRIMARY")
                            .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });
                        j.ToTable("ObjectInsurance_has_Policy");
                        j.HasIndex(new[] { "ObjectInsuranceIdObjectInsurance" },
                            "fk_ObjectInsurance_has_Policy_ObjectInsurance1_idx");
                        j.HasIndex(new[] { "PolicyIdPolicy" }, "fk_ObjectInsurance_has_Policy_Policy1_idx");
                        j.IndexerProperty<int>("ObjectInsuranceIdObjectInsurance")
                            .HasColumnName("ObjectInsurance_idObjectInsurance");
                        j.IndexerProperty<int>("PolicyIdPolicy").HasColumnName("Policy_idPolicy");
                    });
        });

        modelBuilder.Entity<Policy>(entity =>
        {
            entity.HasKey(e => e.IdPolicy).HasName("PRIMARY");

            entity.Property(e => e.EndDate).HasComment("Конец действия полиса.");
            entity.Property(e => e.InsuranceAmout)
                .HasDefaultValueSql("'5000.00'")
                .HasComment("Сумма страхования.");
            entity.Property(e => e.StartDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasComment("Начало действия полиса.");

            entity.HasOne(d => d.UserIdUserNavigation).WithMany(p => p.Policies)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_Policy_User1");

            entity.HasMany(d => d.ConditionPolicyIdConditionPolicies).WithMany(p => p.PolicyIdPolicies)
                .UsingEntity<Dictionary<string, object>>(
                    "ConditionPolicyHasPolicy",
                    r => r.HasOne<ConditionPolicy>().WithMany()
                        .HasForeignKey("ConditionPolicyIdConditionPolicy")
                        .HasConstraintName("fk_ConditionPolicy_has_Policy_ConditionPolicy1"),
                    l => l.HasOne<Policy>().WithMany()
                        .HasForeignKey("PolicyIdPolicy")
                        .HasConstraintName("fk_ConditionPolicy_has_Policy_Policy1"),
                    j =>
                    {
                        j.HasKey("PolicyIdPolicy", "ConditionPolicyIdConditionPolicy")
                            .HasName("PRIMARY")
                            .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });
                        j.ToTable("ConditionPolicy_has_Policy");
                        j.HasIndex(new[] { "ConditionPolicyIdConditionPolicy" },
                            "fk_ConditionPolicy_has_Policy_ConditionPolicy1_idx");
                        j.HasIndex(new[] { "PolicyIdPolicy" }, "fk_ConditionPolicy_has_Policy_Policy1_idx");
                        j.IndexerProperty<int>("PolicyIdPolicy").HasColumnName("Policy_idPolicy");
                        j.IndexerProperty<int>("ConditionPolicyIdConditionPolicy")
                            .HasColumnName("ConditionPolicy_idConditionPolicy");
                    });
        });

        modelBuilder.Entity<Role>(entity => { entity.HasKey(e => e.IdRole).HasName("PRIMARY"); });

        modelBuilder.Entity<RoleHasUser>(entity =>
        {
            entity.HasKey(e => new { e.UserIdUser, e.RoleIdRole })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.HasOne(d => d.RoleIdRoleNavigation).WithMany(p => p.RoleHasUsers)
                .HasConstraintName("fk_Role_has_User_Role1");

            entity.HasOne(d => d.UserIdUserNavigation).WithOne(p => p.RoleHasUser)
                .HasConstraintName("fk_Role_has_User_User1");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.IdUser).HasName("PRIMARY");

            entity.Property(e => e.Email).HasComment("Почта пользователя.");
            entity.Property(e => e.Password).HasComment("Пароль пользователя.");
        });

        modelBuilder.Entity<UserInfo>(entity =>
        {
            entity.HasKey(e => e.IdUserInfo).HasName("PRIMARY");

            entity.Property(e => e.Adress).HasComment("Адресс пользователя.");
            entity.Property(e => e.BirthDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasComment("Дата рождения пользователя.");
            entity.Property(e => e.FirstName).HasComment("Имя пользователя.");
            entity.Property(e => e.LastName).HasComment("Фамилия пользователя.");
            entity.Property(e => e.PhoneNumber).HasComment("Номер телефона пользователя.");

            entity.HasOne(d => d.UserIdUserNavigation).WithOne(p => p.UserInfo).HasConstraintName("fk_UserInfo_User1");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}