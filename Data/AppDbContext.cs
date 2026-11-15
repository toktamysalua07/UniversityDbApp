using Microsoft.EntityFrameworkCore;
using UniversityDbApp.Models;

namespace UniversityDbApp.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Department> Departments => Set<Department>();
    public DbSet<Teacher> Teachers => Set<Teacher>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasIndex(x => x.Code).IsUnique();
            entity.Property(x => x.Name).IsRequired().HasMaxLength(150);
            entity.Property(x => x.Code).IsRequired().HasMaxLength(20);
            entity.Property(x => x.Faculty).IsRequired().HasMaxLength(150);
            entity.Property(x => x.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            entity.HasMany(x => x.Teachers).WithOne(x => x.Department)
                .HasForeignKey(x => x.DepartmentId).OnDelete(DeleteBehavior.Restrict);
        });
        modelBuilder.Entity<Teacher>(entity =>
        {
            entity.HasIndex(x => x.Email).IsUnique();
            entity.Property(x => x.FirstName).IsRequired().HasMaxLength(80);
            entity.Property(x => x.LastName).IsRequired().HasMaxLength(80);
            entity.Property(x => x.Email).IsRequired().HasMaxLength(180);
            entity.Property(x => x.Position).IsRequired().HasMaxLength(120);
        });
    }
}
