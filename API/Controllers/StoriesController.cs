using API.Services;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Settings;
using System.Threading.Tasks;

namespace API.Controllers
{
    [ApiController]
    [Route("/stories")]
    public class StoriesController : ControllerBase
    {
        private readonly IFirebaseService firebaseService;
        private readonly ILogger log;


        public StoriesController(IFirebaseService firebaseService, ILogger logger)
        {
            this.firebaseService = firebaseService;
            log = logger.ForContext<StoriesController>();
        }

        [HttpGet]
        public async Task<IActionResult> GetStoriesOrderedByScore([FromQuery] int n = 0)
        {
            if (n <= 0)
            { 
                return NoContent();
            }

            var stories = await firebaseService.GetStoriesOrderedByScoreAsync(n);
            return Ok(stories);
        }
    }
}
