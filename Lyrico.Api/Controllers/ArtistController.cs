using System;
using System.Threading.Tasks;
using Lyrico.Application;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Lyrico.Api.Controllers
{
    /// <summary>
    /// Handles all artist-related requests
    /// </summary>
    [ApiController]
    [Route("Artists")]
    public class ArtistController : ControllerBase
    {
        readonly IMediator medaitor;

        public ArtistController(IMediator medaitor)
        {
            this.medaitor = medaitor;
        }

        /// <summary>
        /// Returns a set of lyric word count statistics for a given artist
        /// </summary>
        /// <param name="artistName"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("LyricStats")]
        public async Task<IActionResult> GetLyricStats(string artistName)
        {
            var request = new GetLyricStats.Request() { ArtistName = artistName };

            try
            {
                var response = await medaitor.Send(request);

                Console.WriteLine("Request Complete");

                return new OkObjectResult(response);
            }
            catch (ApplicationException e)
            {
                return e.ToActionResult();
            }

        }
    }
}
