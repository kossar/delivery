using AutoMapper;
using BLL.App.DTO.Identity;

namespace BLL.App.DTO.MappingProfiles
{
    public class AutoMapperProfile: Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<VehicleType, DAL.App.DTO.VehicleType>().ReverseMap();
            CreateMap<Dimensions, DAL.App.DTO.Dimensions>().ReverseMap();
            CreateMap<Location, DAL.App.DTO.Location>().ReverseMap();
            CreateMap<Parcel, DAL.App.DTO.Parcel>().ReverseMap();
            CreateMap<Trailer, DAL.App.DTO.Trailer>().ReverseMap();
            CreateMap<Transport, DAL.App.DTO.Transport>().ReverseMap();
            CreateMap<TransportMeta, DAL.App.DTO.TransportMeta>().ReverseMap();
            CreateMap<TransportNeed, DAL.App.DTO.TransportNeed>().ReverseMap();
            CreateMap<TransportOffer, DAL.App.DTO.TransportOffer>().ReverseMap();
            CreateMap<Unit, DAL.App.DTO.Unit>().ReverseMap();
            CreateMap<Vehicle, DAL.App.DTO.Vehicle>().ReverseMap();
            CreateMap<AppRole, DAL.App.DTO.Identity.AppRole>().ReverseMap();
            CreateMap<AppUser, DAL.App.DTO.Identity.AppUser>().ReverseMap();
        }
    }

}