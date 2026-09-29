using Application.Features.QuerySampleSetPointSettings;
using Application.Features.UpdateSampleSetPointSettings;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SampleSetPointSettingsController(IMediator mediator) : ControllerBase
    {
        [HttpGet(Name = "GetSetPointSettings")]
        public async Task<ActionResult<SampleSetPointSettings>> GetSetPointSettings() => await mediator.Send(new SampleSetPointSettingsQuery());

        [HttpPut(Name = "UpdateSetPointSettings")]
        public async Task<IActionResult> UpdateSetPointSettings(SampleSetPointSettings settings)
        {
            await mediator.Send(new UpdateSampleSetPointSettingsCommand(settings));
            return Ok();
        }
    }
}
