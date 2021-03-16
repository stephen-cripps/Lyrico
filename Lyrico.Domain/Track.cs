using System;

namespace Lyrico.Domain
{
    public class Track
    {

        public string Name { get; }

        public uint Wordcount { get; set; }

        public Track(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(name));

            Name = name;
        }
    }
}
