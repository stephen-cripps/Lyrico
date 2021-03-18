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
            //Weird error being thrown here, doesn't seem to affect functionality
            var artistId = await SearchArtist(artistName);
            if (artistId == null)
                return null;

            var artist = await GetArtist(artistId);

            foreach (var release in artist.Releases)
            {
                release.Media = await GetReleaseMedia(release.Id);
            }

            return mapper.Map<Artist>(artist);
        }

        async Task<string> SearchArtist(string artistName)
        {
            var path = $"artist?query=artist:{artistName}&fmt=json&limit=1";

            var response = await client.GetAsync(path);

            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException(response.ReasonPhrase); //This is probably not the best way to do this

            var deserialisedResult = JsonConvert.DeserializeObject<ArtistSearchResult>(await response.Content.ReadAsStringAsync());

            var firstArtist = deserialisedResult.Artists.FirstOrDefault(a => a.Name == artistName);

            return firstArtist?.Id;
        }

        async Task<ArtistDto> GetArtist(string artistId)
        {
            var path = $"artist/{artistId}?inc=releases&fmt=json";
            var response = await client.GetAsync(path);

            return JsonConvert.DeserializeObject<ArtistDto>(await response.Content.ReadAsStringAsync());
        }

        async Task<IEnumerable<MediaDto>> GetReleaseMedia(string releaseId)
        {
            var path = $"release/{releaseId}?inc=recordings&fmt=json";
            var response = await client.GetAsync(path);
            var release = JsonConvert.DeserializeObject<ReleaseDto>(await response.Content.ReadAsStringAsync());
            return release.Media;
        }
    }
}
