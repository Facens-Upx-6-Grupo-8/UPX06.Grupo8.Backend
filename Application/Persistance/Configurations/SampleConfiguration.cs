using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Application.Persistance.Configurations;

internal class SampleConfiguration : IEntityTypeConfiguration<Sample>
{
    const string PRIMARY_KEY_PROPERTY_NAME = "Id";
    public void Configure(EntityTypeBuilder<Sample> builder)
    {
        builder.Property(s => s.Timestamp);
        builder.Property(s => s.SourcingPoint);
        builder.Property(s => s.PH);
        builder.Property(s => s.Turbidity);
        builder.Property(s => s.Temperature);
        builder.Property(s => s.TDS);

        builder.Property<long>(PRIMARY_KEY_PROPERTY_NAME)
            .ValueGeneratedOnAdd();
        builder.HasKey(PRIMARY_KEY_PROPERTY_NAME);

        builder.HasAlternateKey(x => new { x.Timestamp, x.SourcingPoint });
    }
}
