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
        Timestamp: sample.Timestamp,
        PHEvaluationResult: PHSettings.Evaluate(sample.PH),
        TurbidityEvaluationResult: TurbiditySettings.Evaluate(sample.Turbidity),
        TemperatureEvaluationResult: TemperatureSettings.Evaluate(sample.Temperature),
        TDSEvaluationResult: TDSSettings.Evaluate(sample.TDS)
    );
}
public record EvaluatedSample(
    SampleSourcingPoint SampleSourcingPoint,
    DateTime Timestamp,
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
    public EvaluatedSampleMetric Evaluate(double value)
    {
        if (IsEnabled)
        {
            if (value < LowerBound)
            {
                return new UnderLowerBoundEvaluatedSampleMetric(
                    Value: value,
                    Evaluator: this
                );
            }

            if (value > UpperBound)
            {
                return new OverUpperBoundEvaluatedSampleMetric(
                    Value: value,
                    Evaluator: this
                );
            }
        }

        return new SuccessfullyEvaluatedSampleMetric(
            Value: value,
            Evaluator: this
        );
    }
};
public abstract record EvaluatedSampleMetric(bool IsSuccessful, double Value, SampleMetricEvaluator Evaluator);
public record SuccessfullyEvaluatedSampleMetric(double Value, SampleMetricEvaluator Evaluator) : EvaluatedSampleMetric(true, Value, Evaluator);
public abstract record UnsuccessfullyEvaluatedSampleMetric(double Value, SampleMetricEvaluator Evaluator) : EvaluatedSampleMetric(false, Value, Evaluator);
public record OverUpperBoundEvaluatedSampleMetric(double Value, SampleMetricEvaluator Evaluator) : UnsuccessfullyEvaluatedSampleMetric(Value, Evaluator);
public record UnderLowerBoundEvaluatedSampleMetric(double Value, SampleMetricEvaluator Evaluator) : UnsuccessfullyEvaluatedSampleMetric(Value, Evaluator);
