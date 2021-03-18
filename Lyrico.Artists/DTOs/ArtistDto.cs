﻿using System.Collections.Generic;
using Newtonsoft.Json;

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

        public IEnumerable<RecordingDto> Recordings { get; set; }
    }

    public class RecordingDto
    {
        public string Title { get; set; }
    }
}