namespace Domain.Models;

public record SampleSetPointSettings(
    SampleMetricSetPointSettings PHSettings,
    SampleMetricSetPointSettings TurbiditySettings,
    SampleMetricSetPointSettings TemperatureSettings,
    SampleMetricSetPointSettings TDSSettings
)
{
    private int Id { get; init; } = 0;
}

public record SampleMetricSetPointSettings(
    double LowerBound,
    double UpperBound,
    bool IsEnabled
)
{ }