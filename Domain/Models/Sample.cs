using Domain.Enums;

namespace Domain.Models;

public record Sample(
    DateTime CreatedAt,
    SampleSource Source,
    double Value
) { }
