using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Application.Persistance;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Sample> Samples => Set<Sample>();
    public SampleSetPointSettings SampleSetPointSettings => Set<SampleSetPointSettings>().Single();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}
