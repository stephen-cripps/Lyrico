using System.Threading.Tasks;
using Lyrico.Domain;

namespace Lyrico.Application.Services
{
    public interface IArtistService
    {
        /// <summary>
        /// Finds an artist in the artist service and returns an Artist object.
        /// Returns null if artist cannot be found. 
        /// </summary>
        /// <param name="artistName"></param>
        /// <returns></returns>
        Task<Artist> GetArtistAsync(string artistName);
    }
}
