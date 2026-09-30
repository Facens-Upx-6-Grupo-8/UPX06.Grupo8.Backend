using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Application.Persistance;

internal class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Sample> Samples => Set<Sample>();
    public SampleEvaluator SampleEvaluator => Set<SampleEvaluator>().Single();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}
