using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using AutoMapper;
using Lyrico.Application.Services;
using Lyrico.Domain;
using Lyrico.MusicBrainz.DTOs;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Options = Lyrico.MusicBrainz.DependencyInjection.Options;

namespace Lyrico.MusicBrainz
{
    public class MusicBrainzService : IArtistService
    {
        readonly IMapper mapper;

        readonly HttpClient client;

        public MusicBrainzService(IOptions<Options> options, IMapper mapper)
        {
            this.mapper = mapper;
            client = new HttpClient();
            client.BaseAddress = new Uri(options.Value.BaseUrl);
            client.DefaultRequestHeaders.Add("User-Agent", options.Value.UserAgent);
        }

        public async Task<Artist> GetArtistAsync(string artistName)
        {
            var artist = await SearchArtist(artistName);

            if (artist == null)
                return null;

            Console.WriteLine("Artist Found");

            artist.Releases = (await GetReleases(artist.Id)).Distinct(new ReleaseNameComparer());
            Console.WriteLine("Releases Read");

            // I'm removing duplicates to make debugging faster
            foreach (var release in artist.Releases)
            {
                release.Recordings = await GetRecordings(release.Id);
                Console.WriteLine($"{release.Title} Tracklist Read");
            }

            return mapper.Map<Artist>(artist);
        }

        async Task<ArtistDto> SearchArtist(string artistName)
        {
            var path = $"artist?query=artist:{artistName}&fmt=json&limit=1";

            var response = await client.GetAsync(path);

            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException(response.ReasonPhrase);

            var deserialisedResult = JsonConvert.DeserializeObject<ArtistSearchResult>(await response.Content.ReadAsStringAsync());

            var firstArtist = deserialisedResult.Artists.FirstOrDefault(a => a.Name == artistName);

            return firstArtist;
        }


        async Task<IEnumerable<ReleaseDto>> GetReleases(string artistId)
        {
            var offset = 0;
            var releaseCount = 1;

            var releases = new List<ReleaseDto>();
            while (releases.Count < releaseCount)
            {
                System.Threading.Thread.Sleep(1000); //To avoid rate limiting

                //I'm looking at just official albums to keep the number of results down 
                // I don't think there's a way to ignore live albums without doing extra calls to the bakend
                var path = $"release?artist={artistId}&type=album&status=official&fmt=json&offset={offset}";
                var response = await client.GetAsync(path);

                if (!response.IsSuccessStatusCode)
                    throw new HttpRequestException(response.ReasonPhrase);

                var result = JsonConvert.DeserializeObject<ReleaseSearchResult>(await response.Content.ReadAsStringAsync());

                releases.AddRange(result.Releases);
                releaseCount = result.ReleaseCount;
                offset += 25;
            }

            return releases;
        }

        async Task<IEnumerable<RecordingDto>> GetRecordings(string releaseId)
        {
            var offset = 0;
            var recordingCount = 1;

            var recordings = new List<RecordingDto>();
            while (recordings.Count < recordingCount)
            {
                System.Threading.Thread.Sleep(1000); //To avoid rate limiting

                var path = $"recording?release={releaseId}&fmt=json&offset={offset}";
                var response = await client.GetAsync(path);

                if (!response.IsSuccessStatusCode)
                    throw new HttpRequestException(response.ReasonPhrase);

                var result = JsonConvert.DeserializeObject<RecordingSearchResult>(await response.Content.ReadAsStringAsync());

                recordings.AddRange(result.Recordings);
                recordingCount = result.RecordingCount;
                offset += 25;
            }

            return recordings; 
        }

        class ReleaseNameComparer : IEqualityComparer<ReleaseDto>
        {
            public bool Equals(ReleaseDto x, ReleaseDto y) => x.Title == y.Title;

            public int GetHashCode(ReleaseDto obj) => obj.Title.GetHashCode();
        }
    }
}
