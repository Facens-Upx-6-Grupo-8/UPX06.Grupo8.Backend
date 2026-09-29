using Application.Persistance;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SampleSetPointSettingsController(AppDbContext appDbContext) : ControllerBase
    {
        [HttpGet(Name = "GetSetPointSettings")]
        public ActionResult<SampleSetPointSettings> GetSetPointSettings() => Ok(appDbContext.SampleSetPointSettings);

        [HttpPut(Name = "UpdateSetPointSettings")]
        public IActionResult UpdateSetPointSettings(SampleSetPointSettings sample)
        {
            appDbContext.Update(sample);
            appDbContext.SaveChanges();

            return Ok();
        }
    }
}
