using Application.Persistance;
using Domain.Models;
using MediatR;

namespace Application.Features.SampleEvaluator.EvaluateSample;

internal class EvaluateSampleUseCase(AppDbContext appDbContext) : IRequestHandler<EvaluateSampleCommand, EvaluatedSample>
{
    public Task<EvaluatedSample> Handle(EvaluateSampleCommand request, CancellationToken cancellationToken)
        => Task.FromResult(appDbContext.SampleEvaluator.Evaluate(request.Sample));
}

public record EvaluateSampleCommand(Sample Sample) : IRequest<EvaluatedSample>;