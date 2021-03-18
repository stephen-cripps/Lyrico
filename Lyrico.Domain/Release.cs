using System;
using System.Collections.Generic;

namespace Lyrico.Domain
{
    public class Release
    {
        Release()
        { }

        public string Name { get; private set; }
        public int Year { get; private set; }
        public IEnumerable<Track> TrackList { get; private set; }
     
        public Release(string name, IEnumerable<Track> trackList, int year)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(name));

            Name = name;
            TrackList = trackList ?? throw new ArgumentNullException(nameof(trackList));
        }
    }
}
