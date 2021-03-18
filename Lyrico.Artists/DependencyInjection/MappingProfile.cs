using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using Lyrico.Domain;
using Lyrico.MusicBrainz.DTOs;

namespace Lyrico.MusicBrainz.DependencyInjection
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ArtistDto, Artist>();
            CreateMap<ReleaseDto, Release>()
                .ForMember(release => release.TrackList, opt => opt.MapFrom(dto => dto.Media.SelectMany(m => m.Tracks)))
                .ForMember(release => release.Name, opt => opt.MapFrom(dto => dto.Title))
                .ForMember(release => release.Year, opt => opt.MapFrom(dto => dto.Date.Substring(0,4))); //I need to look into the date schema better in the musicbranz docs. For now I'm just taking the first 4 characters and assuming it's the year. 
            CreateMap<TrackDto, Track>()
                .ForMember(track => track.Name, opt => opt.MapFrom(dto => dto.Title));
        }
    }
}
