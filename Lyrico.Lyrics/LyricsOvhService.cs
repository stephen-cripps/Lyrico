using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Lyrico.Application.Services;
using Microsoft.Extensions.Options;
using Options = Lyrico.Lyricsovh.Options;

namespace Lyrico.Lyricsovh
{
    public class LyricsOvhService : ILyricService
    {
        readonly HttpClient client;

        public LyricsOvhService(IOptions<Options> options)
        {
            client = new HttpClient
            {
                BaseAddress = new Uri(options.Value.BaseUrl),
                Timeout = TimeSpan.ParseExact(options.Value.Timeout, "ss", null)
            };
        }

        public Task<uint> GetLyricCountAsync(string artistName, string songName)
        {
            GetLyrics(artistName, songName);
            //If a song isn't found, its not handled it just times out. Use this https://docs.microsoft.com/en-us/dotnet/api/system.net.http.httpclient.timeout?view=net-5.0
            throw new NotImplementedException();
        }

        async Task<string> GetLyrics(string artistName, string songName)
        {
            var response = await client.GetAsync($"{artistName}/{songName}");

            return await response.Content.ReadAsStringAsync();
        }
    }
}
