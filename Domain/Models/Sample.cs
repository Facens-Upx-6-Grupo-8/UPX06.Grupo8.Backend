using Domain.Enums;

namespace Domain.Models;

public record Sample(
    DateTime Timestamp,
    SampleSourcingPoint SourcingPoint,
    double PH,
    double Turbidity,
    double Temperature,
    int TDS
) { }
