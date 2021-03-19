using System.Threading.Tasks;

namespace Lyrico.Application.Services
{
    public interface ILyricService
    {
        /// <summary>
        /// Finds a given song and returns the number of words in that song
        /// </summary>
        /// <param name="artistName"></param>
        /// <param name="songName"></param>
        /// <returns></returns>
        Task<uint?> GetLyricCountAsync(string artistName, string songName);
    }
}
