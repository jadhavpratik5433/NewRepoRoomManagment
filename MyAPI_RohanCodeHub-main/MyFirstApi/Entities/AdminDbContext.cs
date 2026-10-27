using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace MyFirstApi.Entities;

public partial class AdminDbContext : DbContext
{
    public AdminDbContext(DbContextOptions<AdminDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Admin> Admins { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Admin>(entity =>
        {
            entity.HasKey(e => e.AdminId).HasName("PK__Admins__719FE488764642CA");

            entity.HasIndex(e => e.Email, "UQ__Admins__A9D10534D5E9D916").IsUnique();

            entity.Property(e => e.Email).HasMaxLength(150);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.PasswordHash).HasMaxLength(500);
            entity.Property(e => e.Role)
                .HasMaxLength(20)
                .HasDefaultValue("Admin");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
