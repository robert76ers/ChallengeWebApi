using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace N5ChallengeWebApi.Models;

public partial class N5UsersPermissionsContext : DbContext
{
    public N5UsersPermissionsContext(DbContextOptions<N5UsersPermissionsContext> options) : base(options)
    {
    }

    public virtual DbSet<Permission> Permissions { get; set; }

    public virtual DbSet<PermissionType> PermissionTypes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Permission>(entity =>
        {
            entity.ToTable("Permission", "Employeers");

            entity.Property(e => e.EmployeeForename)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.EmployeeSurname)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.PermissionDate).HasColumnType("date");

            entity.HasOne(d => d.PermissionTypeNavigation).WithMany(p => p.Permissions)
                .HasForeignKey(d => d.PermissionType)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Permissio__Permi__286302EC");
        });

        modelBuilder.Entity<PermissionType>(entity =>
        {
            entity.ToTable("PermissionType", "Employeers");

            entity.Property(e => e.Description)
                .HasMaxLength(200)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
