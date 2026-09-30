using Application.Persistance;
using Domain.Models;
using MediatR;

namespace Application.Features.SampleEvaluator.UpdateSampleEvaluator;

internal class UpdateSampleEvaluatorUseCase(AppDbContext appDbContext) : IRequestHandler<UpdateSampleEvaluatorCommand>
{
    public async Task Handle(UpdateSampleEvaluatorCommand request, CancellationToken cancellationToken)
    {
        appDbContext.Update(request.SampleEvaluator);
        await appDbContext.SaveChangesAsync(cancellationToken);
    }
}

public record UpdateSampleEvaluatorCommand(Domain.Models.SampleEvaluator SampleEvaluator) : IRequest;
