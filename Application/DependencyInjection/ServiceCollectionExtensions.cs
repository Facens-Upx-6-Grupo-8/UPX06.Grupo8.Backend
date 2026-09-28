using Application.Persistance;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Application.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        AddApplicationPersistance(services);
        return services;
    }

    private static IServiceCollection AddApplicationPersistance(this IServiceCollection services)
    {
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseInMemoryDatabase("InMemoryDb");
            options.UseSeeding(SeedSampleSetPointSettings);
        });

        return services;
    }

    private static void SeedSampleSetPointSettings(DbContext context, bool _)
    {
        var sampleSetPointSettingsDbSet = context.Set<SampleSetPointSettings>();
        
        if (sampleSetPointSettingsDbSet.Any())
        {
            return;
        }
        
        sampleSetPointSettingsDbSet.Add(
            new(
                PHSettings: new(6.5, 8.5, true),
                TurbiditySettings: new(0, 5, true),
                TemperatureSettings: new(20, 30, true),
                TDSSettings: new(0, 500, true)
            )
        );
        
        context.SaveChanges();
    }
}
