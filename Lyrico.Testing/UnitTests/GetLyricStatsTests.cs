using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Lyrico.Application;
using Lyrico.Application.Exceptions;
using Lyrico.Application.Services;
using Lyrico.Domain;
using Moq;
using Xunit;

namespace Lyrico.Testing.UnitTests
{
    public class GetLyricStatsTests
    {
        readonly Mock<IArtistService> mockArtistService = new Mock<IArtistService>();
        readonly Mock<ILyricService> mockLyricService = new Mock<ILyricService>();
        readonly string artistName;
        readonly GetLyricStats.Request request;
        readonly GetLyricStats.Handler handler;

        public GetLyricStatsTests()
        {
            artistName = "Test Artist";
            handler = new GetLyricStats.Handler(mockArtistService.Object, mockLyricService.Object);
            request = new GetLyricStats.Request() { ArtistName = artistName };
        }

        /// <summary>
        /// This test ensures the results we get are accurate and ignores songs where the lyrics were not found
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Handle_ArtistFound_ReturnsStats()
        {
            // Arrange 
            var trackNames = new List<string> { "t1", "t2", "t3", "t4", "t5", "t6" };
            var tracks1 = new List<Track>
                {new Track(trackNames[0]), new Track(trackNames[1]), new Track(trackNames[2])};
            var tracks2 = new List<Track>
                {new Track(trackNames[3]), new Track(trackNames[4]), new Track(trackNames[5]), new Track("Lyrics Not Found")};
            var releases = new List<Release>()
            {
                new Release("R1", tracks1),
                new Release("R2", tracks2)
            };
            var artist = new Artist(artistName, releases);
            mockArtistService.Setup(m => m.GetArtistAsync(artistName)).ReturnsAsync(artist);

            uint i = 1;
            foreach (var trackName in trackNames)
            {
                mockLyricService.Setup(m => m.GetLyricCountAsync(artistName, trackName)).ReturnsAsync(i * 100);
                i++;
            }

            mockLyricService.Setup(m => m.GetLyricCountAsync(artistName, "Lyrics Not Found")).Returns(Task.FromResult<uint?>(null));

            //Test
            var result = await handler.Handle(request, new CancellationToken());

            //Assert
            Assert.Equal(350, result.Mean);
            Assert.Equal(350, result.Median);
            Assert.Equal(29166.67, (double)result.Variance, 2);
            Assert.Equal(170.78, (double)result.StandardDeviation, 2);
            Assert.Equal(2, result.MeanByRelease.Count);
            Assert.Equal(200, result.MeanByRelease["R1"]);
            Assert.Equal(500, result.MeanByRelease["R2"]);
        }

        /// <summary>
        /// Ensures it can handle an artist with no releases
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Handle_ArtistFoundNoReleases_ReturnsNullStats()
        {
            //Arrange 
            var releases = new List<Release>()
            {
                new Release("R1", new List<Track>()),
                new Release("R2", new List<Track>())
            };
            var artist = new Artist(artistName, releases);
            mockArtistService.Setup(m => m.GetArtistAsync(artistName)).ReturnsAsync(artist);

            //Test
            var result = await handler.Handle(request, new CancellationToken());

            //Assert
            Assert.Equal(0, result.Mean);
            Assert.Null(result.Median);
            Assert.Null(result.Variance);
            Assert.Null(result.StandardDeviation);
            Assert.Null(result.MeanByRelease);
        }

        /// <summary>
        /// Ensures it can handle an aritst with no tracks
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Handle_ArtistFoundNoTracks_ReturnsNullStats()
        {
            //Arrange 
            var artist = new Artist(artistName, new List<Release>());
            mockArtistService.Setup(m => m.GetArtistAsync(artistName)).ReturnsAsync(artist);

            //Test
            var result = await handler.Handle(request, new CancellationToken());

            //Assert
            Assert.Equal(0, result.Mean);
            Assert.Null(result.Median);
            Assert.Null(result.Variance);
            Assert.Null(result.StandardDeviation);
            Assert.Null(result.MeanByRelease);
        }

        /// <summary>
        /// Ensures it can handle artists with no lyrics
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Handle_ArtistFoundLyricsNotFound_ReturnsNullStats()
        {
            // Arrange 
            var trackNames = new List<string> { "t1", "t2", "t3", "t4", "t5", "t6" };
            var tracks1 = new List<Track>
                {new Track(trackNames[0]), new Track(trackNames[1]), new Track(trackNames[2])};
            var tracks2 = new List<Track>
                {new Track(trackNames[3]), new Track(trackNames[4]), new Track(trackNames[5])};
            var releases = new List<Release>()
            {
                new Release("R1", tracks1),
                new Release("R2", tracks2)
            };
            var artist = new Artist(artistName, releases);
            mockArtistService.Setup(m => m.GetArtistAsync(artistName)).ReturnsAsync(artist);

            mockLyricService.Setup(m => m.GetLyricCountAsync(It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult<uint?>(null));

            //Test
            var result = await handler.Handle(request, new CancellationToken());

            //Assert
            Assert.Equal(0, result.Mean);
            Assert.Null(result.Median);
            Assert.Null(result.Variance);
            Assert.Null(result.StandardDeviation);
            Assert.Null(result.MeanByRelease);
        }

        /// <summary>
        /// Ensures it can handle not fiunding an aritst
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Handle_ArtistNotFound_ThrowArtistNotFoundException()
        {
            //Arrange 
            mockArtistService.Setup(m => m.GetArtistAsync(artistName)).Returns(Task.FromResult<Artist>(null));

            //Test
            var exception = await Assert.ThrowsAsync<ArtistNotFoundException>(() => handler.Handle(request, new CancellationToken()));

            //Assert
            Assert.Equal("'Test Artist' could not be found", exception.Message);

        }

        /// <summary>
        /// Ensures it can handle the artist service being down
        /// An equivalent has not been made for the lyric service, as it is so unreliable, exceptions are just caught and logged
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Handle_ArtistServiceNotAvailable_ThrowsServiceUnavailableException()
        {
            //Arrange 
            mockArtistService.Setup(m => m.GetArtistAsync(artistName)).Throws(new HttpRequestException());

            //Test
            var exception = await Assert.ThrowsAsync<ServiceUnavailableException>(() => handler.Handle(request, new CancellationToken()));

            //Assert
            Assert.Equal("An error occurred contacting the service. Service: IArtistServiceProxy", exception.Message);
        }
    }
}
