using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Lyrico.Application.Exceptions;
using Lyrico.Application.Extensions;
using Lyrico.Application.Services;
using Lyrico.Domain;
using MediatR;

namespace Lyrico.Application
{
    public class ReadLyricStatsComplete
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

                if(!wordCounts.Any())
                    return new Result();

                var result = new Result
                {
                    Mean = wordCounts.Average(t => t),
                    Mode = wordCounts.Mode(),
                    Median = wordCounts.Median()
                };

                result.Variance = wordCounts.PopulationVariance(result.Mean);
                result.StandardDeviation = result.Variance == null ? null : (double?) Math.Sqrt((double) result.Variance);

                result.MeanByRelease = artist.Releases.ToDictionary(r => r.Name,
                    r => r.TrackList.Select(t => t.Wordcount).Average(t => t));

                return result;
            }

            //The difference between the two definitions is somewhat subtle. If you have a complete population of values, in other words values for every member of a group of values, then you use the second population definition. If you have only a subset of the values for a population and you want to deduce something about the population as a whole (for example, you only polled 10% of the electorate), then you use the population definition. For more on this issue, see these posts at eard Statistics and Libweb.
            //http://csharphelper.com/blog/2015/12/make-an-extension-method-that-calculates-standard-deviation-in-c/
            //Look up in general polling calculation techniques
            //https://www.calculatorsoup.com/calculators/statistics/variance-calculator.php

            async Task<Artist> InitialiseArtist(string name)
            {
                var artist = await artistService.GetArtistAsync(name);

                if (artist == null)
                    throw new ArtistNotFoundException(name);

                var tracks = artist.Releases.SelectMany(r => r.TrackList);

                foreach (var track in tracks)
                {
                    track.Wordcount = await lyricService.GetLyricCountAsync(track.Name);
                }

                return artist;
            }
        }
    }
}
