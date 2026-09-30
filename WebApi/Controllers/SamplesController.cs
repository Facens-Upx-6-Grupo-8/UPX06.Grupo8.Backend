using Application.Features.Samples.QuerySamples;
using Application.Features.Samples.RegisterSample;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class SamplesController(IMediator mediator) : ControllerBase
{
    [HttpGet(Name = "QuerySamples")]
    public async Task<ActionResult<IEnumerable<SamplesPagedQueryResult>>> Get([FromQuery] SamplesPagedQuery query) => Ok(await mediator.Send(query));

    [HttpGet("Recent", Name = "QueryRecentSamples")]
    public async Task<ActionResult<IEnumerable<Sample>>> GetRecent([FromQuery] SamplesByRecentQuery query) => Ok(await mediator.Send(query));

    [HttpPost(Name = "RegisterSample")]
    public async Task<IActionResult> RegisterSample(Sample sample)
    {
        await mediator.Send(new RegisterSampleCommand(sample));
        return Created();
    }
}
