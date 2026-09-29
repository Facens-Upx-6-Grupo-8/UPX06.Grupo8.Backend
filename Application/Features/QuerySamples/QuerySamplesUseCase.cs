using Application.Persistance;
using Domain.Models;
using MediatR;

namespace Application.Features.QuerySamples;

internal class QuerySamplesUseCase(AppDbContext appDbContext) : IRequestHandler<SamplesQuery, IReadOnlyList<Sample>>
{
    public async Task<IReadOnlyList<Sample>> Handle(SamplesQuery request, CancellationToken cancellationToken)
    {
        return appDbContext.Samples.ToList();
    }
}

public record SamplesQuery() : IRequest<IReadOnlyList<Sample>>;
