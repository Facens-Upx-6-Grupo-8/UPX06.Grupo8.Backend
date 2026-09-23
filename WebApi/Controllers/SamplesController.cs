using Application.Persistance;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SamplesController(AppDbContext appDbContext) : ControllerBase
    {
        [HttpGet(Name = "GetSamples")]
        public ActionResult<IEnumerable<Sample>> Get() => Ok(appDbContext.Samples);

        [HttpPost(Name = "RegisterSample")]
        public IActionResult RegisterSample(Sample sample)
        {
            appDbContext.Samples.Add(sample);
            appDbContext.SaveChanges();

            return Created();
        }
    }
}
