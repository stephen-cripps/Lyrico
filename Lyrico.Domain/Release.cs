using System;
using System.Collections.Generic;

namespace Lyrico.Domain
{
    /// <summary>
    /// Defines a release
    /// </summary>
    public class Release
    {
        Release()
        { }

        public string Name { get; private set; }
        public IEnumerable<Track> TrackList { get; private set; }
     
        public Release(string name, IEnumerable<Track> trackList)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(name));

            Name = name;
            TrackList = trackList ?? throw new ArgumentNullException(nameof(trackList));
        }
    }
}
