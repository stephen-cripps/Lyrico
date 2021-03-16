using System.Threading.Tasks;
using Lyrico.Domain;

namespace Lyrico.Application.Services
{
    public interface IArtistService
    {
        Task<Artist> GetArtistAsync(string artistName);
    }
}
