using Microsoft.EntityFrameworkCore;
using QDR_Server.Models;
using System;

namespace QDR_Server.Data
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users => Set<User>();
        public DbSet<Organization> Organizations => Set<Organization>();
        public DbSet<Event> Events => Set<Event>();
        public DbSet<Category> Categories => Set<Category>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Organization → Users (one-to-many)
            modelBuilder.Entity<User>()
                .HasOne(u => u.Organization)
                .WithMany(o => o.Users)
                .HasForeignKey(u => u.OrganizationID)
                .OnDelete(DeleteBehavior.Cascade);

            // Organization → Events (one-to-many)
            modelBuilder.Entity<Event>()
                .HasOne(e => e.Organization)
                .WithMany(o => o.Events)
                .HasForeignKey(e => e.OrganizationID)
                .OnDelete(DeleteBehavior.Cascade);

            // Event ↔ Category (many-to-many)
            modelBuilder.Entity<Event>()
                .HasMany(e => e.Categories)
                .WithMany(c => c.Events)
                .UsingEntity(j => j.ToTable("EventCategories"));

            // Server-side default timestamps
            modelBuilder.Entity<Event>()
                .Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()");

            modelBuilder.Entity<User>()
                .Property(u => u.CreatedAt)
                .HasDefaultValueSql("now()");

            modelBuilder.Entity<Category>()
                .Property(c => c.CreatedAt)
                .HasDefaultValueSql("now()");

            // Uniqueness constraints worth having now rather than discovering the need later
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<Organization>()
                .HasIndex(o => o.Email)
                .IsUnique();
        }
    }
}
