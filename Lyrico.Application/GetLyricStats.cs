using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
            public int SongsWithLyricsFound { get; set; }
            public double Mean { get; set; }
            public double? Median { get; set; }
            public double? StandardDeviation { get; set; }
            public double? Variance { get; set; }
            public Dictionary<string, double?> MeanByRelease { get; set; }
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
                var artist = await FindArtist(request.ArtistName);

                if (artist == null)
                    throw new ArtistNotFoundException(request.ArtistName);

                await PopulateWordCount(artist);

                return CalculateStats(artist);
            }


            async Task<Artist> FindArtist(string name)
            {
                try
                {
                    return await artistService.GetArtistAsync(name);
                }
                catch (HttpRequestException)
                {
                    throw new ServiceUnavailableException(artistService.GetType().Name);
                }
            }

            async Task PopulateWordCount(Artist artist)
            {
                var tracks = artist.Releases
                    .SelectMany(r => r.TrackList)
                    .Distinct(new TrackNameComparer());

                var tasks = tracks.Select(track => AssignWordCount(artist.Name, track)).ToList();

                await Task.WhenAll(tasks);
            }

            async Task AssignWordCount(string artistName, Track track)
            {
                track.Wordcount = await lyricService.GetLyricCountAsync(artistName, track.Name);
            }

            Result CalculateStats(Artist artist)
            {
                var wordCounts = artist.Releases
                    .SelectMany(r => r.TrackList)
                    .Select(t => t.Wordcount)
                    .Where(c => c != null)
                    .Select(c => (uint)c)
                    .ToList();

                if (!wordCounts.Any())
                    return new Result();

                var result = new Result
                {
                    SongsWithLyricsFound = wordCounts.Count,
                    Mean = wordCounts.Average(t => t),
                    Median = wordCounts.Median()
                };

                result.Variance = wordCounts.PopulationVariance(result.Mean);
                result.StandardDeviation = result.Variance == null ? null : (double?)Math.Sqrt((double)result.Variance);

                result.MeanByRelease = artist.Releases.ToDictionary(r => r.Name,
                    r => r.TrackList.Select(t => t.Wordcount).Average(t => t));

                return result;
            }

        }

        class TrackNameComparer : IEqualityComparer<Track>
        {
            public bool Equals(Track x, Track y) => string.Equals(x.Name, y.Name, StringComparison.CurrentCultureIgnoreCase);

            public int GetHashCode(Track obj) => obj.Name.ToLower().GetHashCode();
        }
    }
}
