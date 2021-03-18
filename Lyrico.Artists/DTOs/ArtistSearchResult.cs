using System.Collections.Generic;

namespace Lyrico.MusicBrainz.DTOs
{
    public class ArtistSearchResult
    {
        public IEnumerable<ArtistDto> Artists { get; set; }
    }
}
