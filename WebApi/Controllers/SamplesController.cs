using Application.Features.QuerySamples;
using Application.Features.RegisterSample;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class SamplesController(IMediator mediator) : ControllerBase
{
    [HttpGet(Name = "GetSamples")]
    public async Task<ActionResult<IEnumerable<SamplesQueryResult>>> Get([FromQuery] SamplesQuery query) => Ok(await mediator.Send(query));

    [HttpPost(Name = "RegisterSample")]
    public async Task<IActionResult> RegisterSample(Sample sample)
    {
        await mediator.Send(new RegisterSampleCommand(sample));
        return Created();
    }
}
