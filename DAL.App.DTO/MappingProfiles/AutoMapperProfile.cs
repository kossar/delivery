using AutoMapper;

namespace DAL.App.DTO.MappingProfiles
{
    public class AutoMapperProfile: Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<DAL.App.DTO.VehicleType, Domain.App.VehicleType>().ReverseMap();
            CreateMap<DAL.App.DTO.Dimensions, Domain.App.Dimensions>().ReverseMap();
            CreateMap<DAL.App.DTO.Location, Domain.App.Location>().ReverseMap();
            CreateMap<DAL.App.DTO.Parcel, Domain.App.Parcel>().ReverseMap();
            CreateMap<DAL.App.DTO.Trailer, Domain.App.Trailer>().ReverseMap();
            CreateMap<DAL.App.DTO.Transport, Domain.App.Transport>().ReverseMap();
            CreateMap<DAL.App.DTO.TransportMeta, Domain.App.TransportMeta>().ReverseMap();
            CreateMap<DAL.App.DTO.TransportNeed, Domain.App.TransportNeed>().ReverseMap();
            CreateMap<DAL.App.DTO.TransportOffer, Domain.App.TransportOffer>().ReverseMap();
            CreateMap<DAL.App.DTO.Unit, Domain.App.Unit>().ReverseMap();
            CreateMap<DAL.App.DTO.Vehicle, Domain.App.Vehicle>().ReverseMap();
            CreateMap<DAL.App.DTO.Identity.AppRole, Domain.App.Identity.AppRole>().ReverseMap();
            CreateMap<DAL.App.DTO.Identity.AppUser, Domain.App.Identity.AppUser>().ReverseMap();
        }
    }

}