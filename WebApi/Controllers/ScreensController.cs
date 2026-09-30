using Application.Features.SampleEvaluator.GetSampleEvaluator;
using Application.Features.Samples.QuerySamples;
using Domain.Enums;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class ScreensController(IMediator mediator) : ControllerBase
{
    [HttpGet("Dashboard", Name = "GetDashboardScreenData")]
    public async Task<ActionResult<DashboardScreenData>> GetDashboardScreenData()
    {
        var recentSamples = await mediator.Send(_samplesAfterFiltrationFromLast24HoursQuery);

        if (recentSamples.Count == 0)
        {
            return Ok(new DashboardScreenData(null, recentSamples));
        }

        var latestSample = recentSamples
            .Where(x => x.SourcingPoint is SampleSourcingPoint.AfterFiltration)
            .MaxBy(x => x.Timestamp);
        
        var sampleEvaluator = await mediator.Send(new SampleEvaluatorQuery());

        var latestSampleEvaluation = sampleEvaluator.Evaluate(latestSample!);

        return Ok(new DashboardScreenData(latestSampleEvaluation, recentSamples));
    }

    private readonly SamplesByRecentQuery _samplesAfterFiltrationFromLast24HoursQuery = new(TimeSpan.FromHours(24), SampleSourcingPoint.AfterFiltration);
}

public record DashboardScreenData(EvaluatedSample? LatestSampleEvaluation, IReadOnlyList<Sample> Samples);
