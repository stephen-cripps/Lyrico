using System;

namespace Lyrico.Domain
{
    /// <summary>
    /// Defines a track
    /// </summary>
    public class Track
    {
        Track() 
        { }

        public string Name { get; private set; }

        public uint? Wordcount { get; set; }

        public Track(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(name));

            Name = name;
        }
    }
}
