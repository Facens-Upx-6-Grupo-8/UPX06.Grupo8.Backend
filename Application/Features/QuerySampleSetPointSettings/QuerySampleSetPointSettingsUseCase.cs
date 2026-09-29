using Application.Persistance;
using Domain.Models;
using MediatR;

namespace Application.Features.QuerySampleSetPointSettings;

internal class QuerySampleSetPointSettingsUseCase(AppDbContext appDbContext) : IRequestHandler<SampleSetPointSettingsQuery, SampleSetPointSettings>
{
    public async Task<SampleSetPointSettings> Handle(SampleSetPointSettingsQuery request, CancellationToken cancellationToken)
    {
        return await Task.FromResult(appDbContext.SampleSetPointSettings);
    }
}

public record SampleSetPointSettingsQuery() : IRequest<SampleSetPointSettings>;
