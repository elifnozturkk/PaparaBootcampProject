using AutoMapper;
using PaparaApp.Project.API.Mapping.TenantFlat.Dtos;
using PaparaApp.Project.API.Mapping.TenantFlat;
using PaparaApp.Project.API.Models.Flats;
using PaparaApp.Project.API.Models.Flats.DTOs;

namespace PaparaApp.Project.API.Mapping.DtoProfile
{
    public class DtoProfile : Profile
    {
        public DtoProfile()
        {
            CreateMap<Flat, FlatDto>();

            CreateMap<FlatAddRequestDto, Flat>()
                .ForMember(dest => dest.BlockInfo, opt => opt.MapFrom(src => src.BlockInfo))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type))
                .ForMember(dest => dest.Floor, opt => opt.MapFrom(src => src.Floor))
                .ForMember(dest => dest.FlatNumber, opt => opt.MapFrom(src => src.FlatNumber));

            CreateMap<FlatUpdateRequestDto, Flat>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status));

            CreateMap<TenantFlat.TenantFlat, TenantFlatDto>();

            CreateMap<TenantFlatAddRequestDto, TenantFlat.TenantFlat>()
                .ForMember(dest => dest.TenantId, opt => opt.MapFrom(src => src.TenantId))
                .ForMember(dest => dest.FlatId, opt => opt.MapFrom(src => src.FlatId))
                .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.StartDate))
                .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.EndDate));

            CreateMap<TenantFlatUpdateRequestDto, TenantFlat.TenantFlat>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.EndDate));

        }
    }
}
