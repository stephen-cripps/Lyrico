using AutoMapper;
using Lyrico.Domain;
using Lyrico.MusicBrainz.DTOs;

namespace Lyrico.MusicBrainz.DependencyInjection
{
    /// <summary>
    /// Defines a mapping between the DTO and domain model
    /// </summary>
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ArtistDto, Artist>();
            CreateMap<ReleaseDto, Release>()
                .ForMember(release => release.Name, opt => opt.MapFrom(dto => dto.Title))
                .ForMember(release => release.TrackList, opt => opt.MapFrom(dto => dto.Recordings));
            CreateMap<RecordingDto, Track>()
                .ForMember(track => track.Name, opt => opt.MapFrom(dto => dto.Title));
        }
    }
}
