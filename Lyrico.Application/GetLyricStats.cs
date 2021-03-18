using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Lyrico.Application.Exceptions;
using Lyrico.Application.Extensions;
using Lyrico.Application.Services;
using Lyrico.Domain;
using MediatR;

namespace Lyrico.Application
{
    public class GetLyricStats
    {
        public class Request : IRequest<Result>
        {
            public string ArtistName { get; set; }
        }

        public class Result
        {
            public double Mean { get; set; }
            public double? Median { get; set; }
            public IEnumerable<uint> Mode { get; set; }
            public double? StandardDeviation { get; set; }
            public double? Variance { get; set; }
            public Dictionary<string, double> MeanByRelease { get; set; }
        }

        public class Handler : IRequestHandler<Request, Result>
        {
            readonly IArtistService artistService;
            readonly ILyricService lyricService;

            public Handler(IArtistService artistService, ILyricService lyricService)
            {
                this.artistService = artistService;
                this.lyricService = lyricService;
            }

            public async Task<Result> Handle(Request request, CancellationToken cancellationToken)
            {
                var artist = await InitialiseArtist(request.ArtistName);

                var wordCounts = artist.Releases
                    .SelectMany(r => r.TrackList)
                    .Select(t => t.Wordcount)
                    .ToList();

                if (!wordCounts.Any())
                    return new Result();

                var result = new Result
                {
                    Mean = wordCounts.Average(t => t),
                    Mode = wordCounts.Mode(),
                    Median = wordCounts.Median()
                };

                result.Variance = wordCounts.PopulationVariance(result.Mean);
                result.StandardDeviation = result.Variance == null ? null : (double?)Math.Sqrt((double)result.Variance);

                result.MeanByRelease = artist.Releases.ToDictionary(r => r.Name,
                    r => r.TrackList.Select(t => t.Wordcount).Average(t => t));

                return result;
            }

            async Task<Artist> InitialiseArtist(string name)
            {
                Artist artist;
                try
                {
                    artist = await artistService.GetArtistAsync(name);
                }
                catch (HttpRequestException)
                {
                    throw new ServiceUnavailableException(artistService.GetType().Name);
                }

                if (artist == null)
                    throw new ArtistNotFoundException(name);

                var tracks = artist.Releases.SelectMany(r => r.TrackList);

                foreach (var track in tracks)
                {
                    track.Wordcount = await lyricService.GetLyricCountAsync(artist.Name, track.Name);
                }

                return artist;
            }
        }
    }
}
