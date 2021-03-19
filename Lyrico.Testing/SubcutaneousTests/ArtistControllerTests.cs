using System.Threading.Tasks;
using Lyrico.Api.Controllers;
using Lyrico.Application;
using Lyrico.Application.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Xunit;

namespace Lyrico.Testing.SubcutaneousTests
{
    /// <summary>
    /// Tests the artist controller endpoints end to end
    /// </summary>
    public class ArtistControllerTests
    {
        ArtistController controller;

        /// <summary>
        /// Configures DI
        /// </summary>
        public ArtistControllerTests()
        {
            var host = new HostBuilder().ConfigureServices(services =>
                {
                    services.AddTransient<IArtistService, MockArtistService>()
                        .AddTransient<ILyricService, MockLyricService>()
                        .AddMediatR(typeof(GetLyricStats).Assembly);
                })
                .Build();

            controller = new ArtistController(host.Services.GetRequiredService<IMediator>());
        }

        /// <summary>
        /// Tests a successful result
        /// Is only verifying the right type sis returned. Unit tests are used to validate the maths
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetLyricStats_ArtistFound_Returns200()
        {
            var res = (await controller.GetLyricStats("Test Artist")) as ObjectResult;

            Assert.Equal(200, res.StatusCode);
            Assert.Equal(typeof(GetLyricStats.Result), res.Value.GetType());
        }

        /// <summary>
        /// Tests the handling of an artist not found
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetLyricStats_ArtistNotFound_Returns404()
        {
            var res = (await controller.GetLyricStats("Fake Artist")) as NotFoundObjectResult;

            Assert.Equal(404, res.StatusCode);
        }

        /// <summary>
        /// Tests handling when the Artist service is unavailable
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetLyricStats_ArtistServiceDown_Returns503()
        {
            var host = new HostBuilder().ConfigureServices(services =>
                {
                    services.AddTransient<IArtistService, MockUnavailableArtistService>()
                        .AddTransient<ILyricService, MockLyricService>()
                        .AddMediatR(typeof(GetLyricStats).Assembly);
                })
                .Build();

            var controller = new ArtistController(host.Services.GetRequiredService<IMediator>());

            var res = (await controller.GetLyricStats("Test Artist")) as StatusCodeResult;

            Assert.Equal(503, res.StatusCode);
        }
    }
}
