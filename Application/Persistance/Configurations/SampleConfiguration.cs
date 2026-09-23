using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Application.Persistance.Configurations;

internal class SampleConfiguration : IEntityTypeConfiguration<Sample>
{
    const string PRIMARY_KEY_PROPERTY_NAME = "Id";
    public void Configure(EntityTypeBuilder<Sample> builder)
    {
        builder.Property(s => s.CreatedAt);
        builder.Property(s => s.Source);
        builder.Property(s => s.Value);

        builder.Property<long>(PRIMARY_KEY_PROPERTY_NAME)
            .ValueGeneratedOnAdd();

        builder.HasKey(PRIMARY_KEY_PROPERTY_NAME);
    }
}
