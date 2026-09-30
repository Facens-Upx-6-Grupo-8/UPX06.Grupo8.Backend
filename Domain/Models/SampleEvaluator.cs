using Domain.Enums;

namespace Domain.Models;

public record SampleEvaluator(
    SampleMetricEvaluator PHSettings,
    SampleMetricEvaluator TurbiditySettings,
    SampleMetricEvaluator TemperatureSettings,
    SampleMetricEvaluator TDSSettings
)
{
    public const string PRIMARY_KEY_PROPERTY_NAME = "Id";
    private int Id { get; init; } = 1;

    public SampleEvaluator() : this(default!, default!, default!, default!) { }

    public EvaluatedSample Evaluate(Sample sample) => new(
        SampleSourcingPoint: sample.SourcingPoint,
        PHEvaluationResult: PHSettings.Evaluate(sample.PH),
        TurbidityEvaluationResult: TurbiditySettings.Evaluate(sample.Turbidity),
        TemperatureEvaluationResult: TemperatureSettings.Evaluate(sample.Temperature),
        TDSEvaluationResult: TDSSettings.Evaluate(sample.TDS)
    );
}
public record EvaluatedSample(
    SampleSourcingPoint SampleSourcingPoint,
    EvaluatedSampleMetric PHEvaluationResult,
    EvaluatedSampleMetric TurbidityEvaluationResult,
    EvaluatedSampleMetric TemperatureEvaluationResult,
    EvaluatedSampleMetric TDSEvaluationResult
)
{
    public SampleQuality Quality => PHEvaluationResult.IsSuccessful &&
                                    TurbidityEvaluationResult.IsSuccessful &&
                                    TemperatureEvaluationResult.IsSuccessful &&
                                    TDSEvaluationResult.IsSuccessful
        ? SampleQuality.ProperForConsumption
        : SampleQuality.ImproperForConsumption;
};

public record SampleMetricEvaluator(double LowerBound, double UpperBound, bool IsEnabled)
{
    public EvaluatedSampleMetric Evaluate(double value) => new(
        IsSuccessful: !IsEnabled || (value >= LowerBound && value <= UpperBound),
        Value: value,
        Evaluator: this
    );
};
public record EvaluatedSampleMetric(bool IsSuccessful, double Value, SampleMetricEvaluator Evaluator);
