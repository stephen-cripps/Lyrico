﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Lyrico.Application.Services;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
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
                Timeout = TimeSpan.FromSeconds(300)
            };
        }

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

        async Task<string> GetLyrics(string artistName, string songName)
        {
            var path = $"{artistName}/{songName}";
            var response = await client.GetAsync(path);

            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException(response.ReasonPhrase);

            return JsonConvert.DeserializeObject<LyricResponse>(await response.Content.ReadAsStringAsync()).Lyrics;
        }

        class LyricResponse
        {
            public string Lyrics { get; set; }
        }
    }
}