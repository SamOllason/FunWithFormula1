using F1Sim.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace F1Sim.Infrastructure.Data;

public class F1SimDbContext : DbContext
{
    public F1SimDbContext(DbContextOptions<F1SimDbContext> options) : base(options)
    {
    }

    public DbSet<Driver> Drivers { get; set; }
    public DbSet<Team> Teams { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Driver>(entity =>
        {
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.FirstName)
                .IsRequired()
                .HasMaxLength(50);
            
            entity.Property(e => e.LastName)
                .IsRequired()
                .HasMaxLength(50);
            
            entity.Property(e => e.Nationality)
                .IsRequired()
                .HasMaxLength(50);
            
            entity.Property(e => e.DriverNumber)
                .IsRequired();
            
            entity.HasIndex(e => e.DriverNumber)
                .IsUnique();
            
            entity.HasOne(e => e.Team)
                .WithMany(t => t.Drivers)
                .HasForeignKey(e => e.TeamId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<Team>(entity =>
        {
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);
            
            entity.Property(e => e.Constructor)
                .IsRequired()
                .HasMaxLength(100);
            
            entity.Property(e => e.BaseLocation)
                .IsRequired()
                .HasMaxLength(100);
            
            entity.HasIndex(e => e.Name)
                .IsUnique();
        });
    }

    public override int SaveChanges()
    {
        UpdateTimestamps();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateTimestamps();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void UpdateTimestamps()
    {
        var entries = ChangeTracker.Entries()
            .Where(e => e.Entity is BaseEntity && 
                       (e.State == EntityState.Added || e.State == EntityState.Modified));

        foreach (var entry in entries)
        {
            var entity = (BaseEntity)entry.Entity;
            
            if (entry.State == EntityState.Added)
            {
                entity.CreatedAt = DateTime.UtcNow;
            }
            
            entity.UpdatedAt = DateTime.UtcNow;
        }
    }
}
