using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Lyrico.Application.Services;
using Lyrico.Domain;

namespace Lyrico.Testing.SubcutaneousTests
{
    /// <summary>
    /// A mock artist service to return dummy data for the subcutaneous tests
    /// </summary>
    public class MockArtistService : IArtistService
    {
        Artist dummyArtist;
        public MockArtistService()
        {
            var tracks = new List<Track>();
            for (var i = 0; i < 10; i++)
            {
                tracks.Add(new Track("track - " + i));
            }

            var releases = new List<Release>() {new Release("Test Release", tracks)};

            dummyArtist = new Artist("Test Artist", releases);
        }

        public Task<Artist> GetArtistAsync(string artistName)
        {
            return artistName == "Test Artist" ? Task.FromResult(dummyArtist) : Task.FromResult<Artist>(null);
        }
    }

    /// <summary>
    /// A mock artist service to simulate an unavailable service
    /// </summary>
    public class MockUnavailableArtistService : IArtistService
    {
        public Task<Artist> GetArtistAsync(string artistName)
        {
            throw new HttpRequestException();
        }
    }
}
