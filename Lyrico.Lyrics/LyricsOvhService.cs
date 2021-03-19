using System;
using System.Net.Http;
using System.Threading.Tasks;
using Lyrico.Application.Services;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Lyrico.Lyricsovh
{
    /// <summary>
    /// Accesses the LyricsOvh api
    /// </summary>
    public class LyricsOvhService : ILyricService
    {
        readonly HttpClient client;

        public LyricsOvhService(IOptions<Options> options)
        {
            client = new HttpClient
            {
                BaseAddress = new Uri(options.Value.BaseUrl),
                Timeout = TimeSpan.FromSeconds(options.Value.Timeout)
            };
        }

        /// <summary>
        /// Sends a request for lyrics then counts the words
        /// </summary>
        /// <param name="artistName"></param>
        /// <param name="songName"></param>
        /// <returns></returns>
        public async Task<uint?> GetLyricCountAsync(string artistName, string songName)
        {
            string lyrics;
            try
            {
                lyrics = await GetLyrics(artistName, songName);
            }
            catch (Exception e)
            {
                 Console.WriteLine($"Error getting lyrics for {songName}: {e.Message}");
                return null;
            }

            var split = lyrics.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);

            var count = split.Length;

            Console.WriteLine(songName + ": " + count);

            return (uint?)count;
        }

        /// <summary>
        /// Sends a request to the service to get lyrics
        /// </summary>
        /// <param name="artistName"></param>
        /// <param name="songName"></param>
        /// <returns></returns>
        async Task<string> GetLyrics(string artistName, string songName)
        {
            var path = $"{artistName}/{songName}";
            var response = await client.GetAsync(path);

            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException(response.ReasonPhrase);

            return JsonConvert.DeserializeObject<LyricResponse>(await response.Content.ReadAsStringAsync()).Lyrics;
        }

        /// <summary>
        /// Defines the response from the API
        /// </summary>
        class LyricResponse
        {
            public string Lyrics { get; set; }
        }
    }
}
