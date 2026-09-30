using Application.Features.SampleEvaluator.GetSampleEvaluator;
using Application.Features.SampleEvaluator.UpdateSampleEvaluator;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class SampleEvaluatorController(IMediator mediator) : ControllerBase
{
    [HttpGet(Name = "GetSampleEvaluator")]
    public async Task<ActionResult<SampleEvaluator>> GetSampleEvaluator() => await mediator.Send(new SampleEvaluatorQuery());

    [HttpPut(Name = "UpdateSampleEvaluator")]
    public async Task<IActionResult> UpdateSampleEvaluator(SampleEvaluator sampleEvaluator)
    {
        await mediator.Send(new UpdateSampleEvaluatorCommand(sampleEvaluator));
        return Ok();
    }
}
