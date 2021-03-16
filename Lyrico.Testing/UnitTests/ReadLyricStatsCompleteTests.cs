using System.Collections.Generic;
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
    public class ReadLyricStatsCompleteTests
    {
        readonly Mock<IArtistService> mockArtistService = new Mock<IArtistService>();
        readonly Mock<ILyricService> mockLyricService = new Mock<ILyricService>();
        readonly string artistName;
        readonly ReadLyricStatsComplete.Request request;
        readonly ReadLyricStatsComplete.Handler handler;

        public ReadLyricStatsCompleteTests()
        {
              artistName = "Test Artist";
             handler = new ReadLyricStatsComplete.Handler(mockArtistService.Object, mockLyricService.Object);
             request = new ReadLyricStatsComplete.Request() { ArtistName = artistName };
        }

        [Fact]
        public async Task Handle_ArtistFound_ReturnsStats()
        {
            // Arrange 
            var trackNames = new List<string> {"t1", "t2", "t3", "t4", "t5", "t6"};
            var tracks1 = new List<Track>
                {new Track(trackNames[0]), new Track(trackNames[1]), new Track(trackNames[2])};
            var tracks2 = new List<Track>
                {new Track(trackNames[3]), new Track(trackNames[4]), new Track(trackNames[5])};
            var releases = new List<Release>()
            {
                new Release("R1", tracks1, 2021),
                new Release("R2", tracks2, 2021)
            };
            var artist = new Artist(artistName, releases);
            mockArtistService.Setup(m => m.GetArtistAsync(artistName)).ReturnsAsync(artist);

            uint i = 1;
            foreach (var trackName in trackNames)
            {
                mockLyricService.Setup(m => m.GetLyricCountAsync(trackName)).ReturnsAsync(i * 100);
                i++;
            }

            //Test
            var result = await handler.Handle(request, new CancellationToken());

            //Assert
            Assert.Equal(350, result.Mean);
            Assert.Equal(350, result.Median);
            Assert.Equal(new List<uint>() {100, 200, 300, 400, 500, 600}, result.Mode);
            Assert.Equal(29166.67, (double) result.Variance, 2);
            Assert.Equal(170.78, (double) result.StandardDeviation, 2);
            Assert.Equal(2, result.MeanByRelease.Count);
            Assert.Equal(200, result.MeanByRelease["R1"]);
            Assert.Equal(500, result.MeanByRelease["R2"]);
        }

        [Fact]
        public async Task Handle_ArtistFoundNoReleases_ReturnsNullStats()
        {
            //Arrange 
            var releases = new List<Release>()
            {
                new Release("R1", new List<Track>(), 2021),
                new Release("R2", new List<Track>(), 2021)
            };
            var artist = new Artist(artistName, releases);
            mockArtistService.Setup(m => m.GetArtistAsync(artistName)).ReturnsAsync(artist);

            //Test
            var result = await handler.Handle(request, new CancellationToken());

            //Assert
            Assert.Equal(0,result.Mean);
            Assert.Null(result.Median);
            Assert.Null(result.Mode);
            Assert.Null(result.Variance);
            Assert.Null(result.StandardDeviation);
            Assert.Null(result.MeanByRelease);
        }

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
            Assert.Null(result.Mode);
            Assert.Null(result.Variance);
            Assert.Null(result.StandardDeviation);
            Assert.Null(result.MeanByRelease);
        }

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
    }
}
