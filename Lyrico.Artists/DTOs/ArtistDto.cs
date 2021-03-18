using System.Collections.Generic;

namespace Lyrico.MusicBrainz.DTOs
{
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
        public IEnumerable<MediaDto> Media { get; set; }
        public string Date { get; set; }
    }

    public class MediaDto
    {
        public IEnumerable<TrackDto> Tracks { get; set; }
    }

    public class TrackDto
    {
        public string Title { get; set; }
    }
}