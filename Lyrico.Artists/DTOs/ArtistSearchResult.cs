using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Lyrico.MusicBrainz.DTOs
{
    public class ArtistSearchResult
    {
        public IEnumerable<ArtistDto> Artists { get; set; }
    }

    public class ReleaseSearchResult
    {
        public IEnumerable<ReleaseDto> Releases { get; set; }

        [JsonProperty(PropertyName = "release-count")]
        public int ReleaseCount { get; set; }
    }

    public class RecordingSearchResult
    {
        public IEnumerable<RecordingDto> Recordings { get; set; }
        [JsonProperty(PropertyName = "recording-count")]
        public int RecordingCount { get; set; }
    }
}
