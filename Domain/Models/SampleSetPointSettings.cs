namespace Domain.Models;

public record SampleSetPointSettings(
    SampleMetricSetPointSettings PHSettings,
    SampleMetricSetPointSettings TurbiditySettings,
    SampleMetricSetPointSettings TemperatureSettings,
    SampleMetricSetPointSettings TDSSettings
)
{
    public const string PRIMARY_KEY_PROPERTY_NAME = "Id";
    private int Id { get; init; } = 1;

    public SampleSetPointSettings() : this(default!, default!, default!, default!) { }
}

public record SampleMetricSetPointSettings(
    double LowerBound,
    double UpperBound,
    bool IsEnabled
)
{ }