using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Application.Persistance.Configurations;

internal class SampleSetPointSettingsConfiguration : IEntityTypeConfiguration<SampleSetPointSettings>
{
    const string PRIMARY_KEY_PROPERTY_NAME = "Id";
    public void Configure(EntityTypeBuilder<SampleSetPointSettings> builder)
    {
        builder.ComplexProperty(x => x.PHSettings);
        builder.ComplexProperty(x => x.TurbiditySettings);
        builder.ComplexProperty(x => x.TemperatureSettings);
        builder.ComplexProperty(x => x.TDSSettings);

        builder.HasKey(PRIMARY_KEY_PROPERTY_NAME);
    }
}
