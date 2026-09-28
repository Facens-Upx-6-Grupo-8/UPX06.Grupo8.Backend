namespace Domain.Models;

public record SampleSetPointSettings(
    SampleMetricSetPointSettings PHSettings,
    SampleMetricSetPointSettings TurbiditySettings,
    SampleMetricSetPointSettings TemperatureSettings,
    SampleMetricSetPointSettings TDSSettings
)
{
    public const string PRIMARY_KEY_PROPERTY_NAME = "Id";
    private int Id { get; init; } = 0;
}

public record SampleMetricSetPointSettings(
    double LowerBound,
    double UpperBound,
    bool IsEnabled
)
{ }