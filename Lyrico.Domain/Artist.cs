using System;
using System.Collections.Generic;

namespace Lyrico.Domain
{
    public class Artist
    {
        public string Name { get; }
        public IEnumerable<Release> Releases { get; }

        public Artist(string name, IEnumerable<Release> releases)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(name));

            Name = name;
            Releases = releases;
        }
    }
}
