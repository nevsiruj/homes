using Astuc.Domain.DTOs;

using AutoMapper;

using Domain.DTOs;

using Domain.Entities;



public class MappingProfile : Profile

{

    public MappingProfile()

    {

        // Mapeos para Inmueble
        CreateMap<Inmueble, InmuebleDTO>()
        .ForMember(dest => dest.ImagenesReferencia, opt => opt.MapFrom(src => src.ImagenesReferencia))
        .ForMember(dest => dest.Amenidades, opt => opt.MapFrom(src => src.Amenidades));

        CreateMap<InmuebleDTO, Inmueble>()
    .ForMember(dest => dest.ImagenesReferencia, opt => opt.MapFrom(src => src.ImagenesReferencia))
    .ForMember(dest => dest.Amenidades, opt => opt.MapFrom(src => src.Amenidades));

        // Mapeos para Imágenes
        CreateMap<ImagenInmueble, ImagenReferenciaDTO>();
        CreateMap<ImagenReferenciaDTO, ImagenInmueble>();

        // Mapeos para Amenidades
        CreateMap<AmenidadInmueble, AmenidadInmuebleDTO>();
        CreateMap<AmenidadInmuebleDTO, AmenidadInmueble>();
        CreateMap<UserDTO, ApplicationUser>().ReverseMap();

        // Mapeos para Preferencias (deben ir antes que Cliente)
        CreateMap<CreatePreferenciaDTO, Preferencias>();
        CreateMap<PreferenciaDTO, Preferencias>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ClienteId, opt => opt.Ignore());

        CreateMap<Preferencias, PreferenciaDTO>()
            .ForMember(dest => dest.PreferenciaAmenidades,
                opt => opt.MapFrom(src => src.PreferenciaAmenidades));

        CreateMap<Preferencias, CreatePreferenciaDTO>().ReverseMap();
        CreateMap<PreferenciaAmenidad, CreatePreferenciaAmenidadDTO>().ReverseMap();
        CreateMap<PreferenciaAmenidad, AmenidadInmuebleDTO>().ReverseMap();
        CreateMap<PreferenciaAmenidad, PreferenciaAmenidadDTO>().ReverseMap();

        CreateMap<Preferencias, PreferenciaDTO>()
            .ForMember(dest => dest.PreferenciaAmenidades,
                opt => opt.MapFrom(src => src.PreferenciaAmenidades))
            .ReverseMap();

        CreateMap<PreferenciaDTO, Preferencias>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ClienteId, opt => opt.Ignore());

        // Mapeos para Cliente (ahora AutoMapper ya sabe cómo mapear Preferencias)
        CreateMap<Cliente, ClienteDto>().ReverseMap();

        CreateMap<CreateClienteDTO, Cliente>()
            .ForMember(dest => dest.Preferencias, opt => opt.MapFrom(src => src.Preferencias));

        CreateMap<Cliente, CreateClienteDTO>()
            .ForMember(dest => dest.Preferencias, opt => opt.MapFrom(src => src.Preferencias));

        CreateMap<Match, MatchDto>().ReverseMap()
            .ForMember(dest => dest.Cliente, opt => opt.Ignore());

        // Mapeos para Interacción
        CreateMap<Interaccion, InteraccionDTO>().ReverseMap();

        // Mapeo de Proyecto
        CreateMap<Proyecto, ProyectoDTO>();
        CreateMap<ProyectoDTO, Proyecto>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());

        CreateMap<ImagenProyecto, ImagenProyectoDTO>();
        CreateMap<ImagenProyectoDTO, ImagenProyecto>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());

        CreateMap<AmenidadProyecto, AmenidadProyectoDTO>();
        CreateMap<AmenidadProyectoDTO, AmenidadProyecto>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());
    }

}