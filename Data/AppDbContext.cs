using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using aspgrupo2.Models;

namespace aspgrupo2.Data;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Privilege> Privileges { get; set; }

    public virtual DbSet<RefreshToken> RefreshTokens { get; set; }

    public virtual DbSet<SportsArticle> SportsArticles { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {


        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__RefreshT__3214EC07DD7755A5");

            entity.HasIndex(e => e.Token, "IX_RefreshTokens_Token");

            entity.HasIndex(e => e.UserId, "IX_RefreshTokens_UserId");

            entity.HasIndex(e => e.Token, "UQ__RefreshT__1EB4F817BE997C3E").IsUnique();

            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Expires).HasColumnType("datetime");
            entity.Property(e => e.Token).HasMaxLength(500);

            entity.HasOne(d => d.User).WithMany(p => p.RefreshTokens)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__RefreshTo__UserI__44FF419A");
        });

        modelBuilder.Entity<SportsArticle>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__SportsAr__3214EC070D70D6C6");

            entity.HasIndex(e => e.Categoria, "IX_SportsArticles_Categoria");

            entity.Property(e => e.Categoria).HasMaxLength(50);
            entity.Property(e => e.Descripcion).HasMaxLength(500);
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FechaModificacion).HasColumnType("datetime");
            entity.Property(e => e.Nombre).HasMaxLength(100);
            entity.Property(e => e.Precio).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.CreadoPorNavigation).WithMany(p => p.SportsArticles)
                .HasForeignKey(d => d.CreadoPor)
                .HasConstraintName("FK__SportsArt__Cread__49C3F6B7");
        });

        // Configuración de Privilege
        modelBuilder.Entity<Privilege>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Name).IsUnique();
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        // Configuración de RefreshToken
        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Token).IsUnique();
            entity.HasIndex(e => e.UserId);
            entity.Property(e => e.Token).HasMaxLength(500);

            entity.HasOne(d => d.User)
                .WithMany(p => p.RefreshTokens)
                .HasForeignKey(d => d.UserId);
        });

        // Configuración de SportsArticle
        modelBuilder.Entity<SportsArticle>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Categoria);
            entity.Property(e => e.Precio).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.CreadoPorNavigation)
                .WithMany(p => p.SportsArticles)
                .HasForeignKey(d => d.CreadoPor);
        });

        // Configuración de User
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.UserName).IsUnique();
        });

        // Configuración explícita de la relación muchos-a-muchos
        modelBuilder.Entity<UserPrivilege>()
            .HasKey(up => new { up.UserId, up.PrivilegeId });

        modelBuilder.Entity<UserPrivilege>()
            .HasOne(up => up.User)
            .WithMany(u => u.UserPrivileges)
            .HasForeignKey(up => up.UserId);

        modelBuilder.Entity<UserPrivilege>()
            .HasOne(up => up.Privilege)
            .WithMany(p => p.UserPrivileges)
            .HasForeignKey(up => up.PrivilegeId);
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
