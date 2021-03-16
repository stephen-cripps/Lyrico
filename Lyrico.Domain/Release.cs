using System;
using System.Collections.Generic;

namespace Lyrico.Domain
{
    public class Release
    {
        public string Name { get; }
        public int Year { get; }
        public IEnumerable<Track> TrackList { get; }
     
        public Release(string name, IEnumerable<Track> trackList, int year)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(name));

            Name = name;
            TrackList = trackList ?? throw new ArgumentNullException(nameof(trackList));
            Year = year;
        }
    }
}
