using Application.Persistance;
using Domain.Models;
using MediatR;

namespace Application.Features.RegisterSample;

internal class RegisterSampleUseCase(AppDbContext appDbContext) : IRequestHandler<RegisterSampleCommand>
{
    public async Task Handle(RegisterSampleCommand request, CancellationToken cancellationToken)
    {
        await appDbContext.Samples.AddAsync(request.Sample, cancellationToken);
        await appDbContext.SaveChangesAsync(cancellationToken);
    }
}

public record RegisterSampleCommand(Sample Sample) : IRequest;
