using System.Threading.Tasks;
using Lyrico.Application.Services;

namespace Lyrico.Testing.SubcutaneousTests
{
    /// <summary>
    /// Mock Lyric Service to return dummy data for the subcutaneous tests
    /// </summary>
    public class MockLyricService : ILyricService
    {
        public Task<uint?> GetLyricCountAsync(string artistName, string songName)
        {
            //As the unit tests are already checking the maths, this is going to be really basic
            return Task.FromResult((uint?) 200);
        }
    }
}
