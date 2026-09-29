using Application.Features.QuerySamples;
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
        return Ok(new DashboardScreenData(recentSamples));
    }
    private readonly SamplesByRecentQuery _samplesAfterFiltrationFromLast24HoursQuery = new(TimeSpan.FromHours(24), SampleSourcingPoint.AfterFiltration);
}

// TODO: Finalize structure as shown in wireframe.
public record DashboardScreenData(
    IReadOnlyList<Sample> RecentSamples
);
