using Application.Persistance;
using Domain.Models;
using MediatR;

namespace Application.Features.UpdateSampleSetPointSettings;

internal class UpdateSampleSetPointSettingsUseCase(AppDbContext appDbContext) : IRequestHandler<UpdateSampleSetPointSettingsCommand>
{
    public async Task Handle(UpdateSampleSetPointSettingsCommand request, CancellationToken cancellationToken)
    {
        appDbContext.Update(request.SampleSetPointSettings);
        await appDbContext.SaveChangesAsync(cancellationToken);
    }
}

public record UpdateSampleSetPointSettingsCommand(SampleSetPointSettings SampleSetPointSettings) : IRequest;
