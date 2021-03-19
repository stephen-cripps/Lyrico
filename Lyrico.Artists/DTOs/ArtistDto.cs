using System.Collections.Generic;

namespace Lyrico.MusicBrainz.DTOs
{
    /// <summary>
    /// Object classes to deserialise results from the service into
    /// </summary>
    public class ArtistDto
    {
        public string Name { get; set; }

        public string Id { get; set; }

        public IEnumerable<ReleaseDto> Releases { get; set; }
    }

    public class ReleaseDto
    {
        public string Title { get; set; }

        public string Id { get; set; }

        public IEnumerable<RecordingDto> Recordings { get; set; }
    }

    public class RecordingDto
    {
        public string Title { get; set; }
    }
}