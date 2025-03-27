using Microsoft.EntityFrameworkCore;
using AwesomeFacts.Models;

namespace AwesomeFacts.Data;

public class FactsDbContext : DbContext
{
    public DbSet<Fact> Facts { get; set; } = null!;

    public FactsDbContext(DbContextOptions<FactsDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Fact>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Text).IsRequired();
            entity.Property(e => e.Category).IsRequired();
            entity.Property(e => e.CreatedAt).IsRequired();
        });
    }
} 