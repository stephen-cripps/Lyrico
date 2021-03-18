using System.Threading.Tasks;

namespace Lyrico.Application.Services
{
    public interface ILyricService
    {
        Task<uint?> GetLyricCountAsync(string artistName, string songName);
    }
}
