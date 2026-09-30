using Application.Persistance;
using MediatR;

namespace Application.Features.SampleEvaluator.GetSampleEvaluator;

internal class QuerySampleEvaluator(AppDbContext appDbContext) : IRequestHandler<SampleEvaluatorQuery, Domain.Models.SampleEvaluator>
{
    public async Task<Domain.Models.SampleEvaluator> Handle(SampleEvaluatorQuery request, CancellationToken cancellationToken)
    {
        return await Task.FromResult(appDbContext.SampleEvaluator);
    }
}

public record SampleEvaluatorQuery() : IRequest<Domain.Models.SampleEvaluator>;
