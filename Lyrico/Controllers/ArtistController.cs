using System.Threading.Tasks;
using Lyrico.Application;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Lyrico.Api.Controllers
{
    [ApiController]
    [Route("Artists")]
    public class ArtistController : ControllerBase
    {
        readonly IMediator medaitor;

        public ArtistController(IMediator medaitor)
        {
            this.medaitor = medaitor;
        }

        [HttpGet]
        [Route("LyricStats")]
        public async Task<IActionResult> GetLyricStats(string artistName)
        {
            var request = new GetLyricStats.Request() { ArtistName = artistName };

            var response = await medaitor.Send(request);

            return new OkObjectResult(response); 
        }
    }
}
