using AutoMapper;

namespace PublicApi.DTO.v1.MappingProfiles
{
    public class AutoMapperProfile: Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<BLL.App.DTO.VehicleType, PublicApi.DTO.v1.VehicleType>().ReverseMap();
            CreateMap<BLL.App.DTO.Dimensions, PublicApi.DTO.v1.Dimensions>().ReverseMap();
            CreateMap<BLL.App.DTO.Location, PublicApi.DTO.v1.Location>().ReverseMap();
            CreateMap<BLL.App.DTO.Parcel, PublicApi.DTO.v1.Parcel>().ReverseMap();
            CreateMap<BLL.App.DTO.Trailer, PublicApi.DTO.v1.Trailer>().ReverseMap();
            CreateMap<BLL.App.DTO.Transport, PublicApi.DTO.v1.Transport>().ReverseMap();
            CreateMap<BLL.App.DTO.TransportMeta, PublicApi.DTO.v1.TransportMeta>().ReverseMap();
            CreateMap<BLL.App.DTO.TransportNeed, PublicApi.DTO.v1.TransportNeed>().ReverseMap();
            CreateMap<BLL.App.DTO.TransportOffer, PublicApi.DTO.v1.TransportOffer>().ReverseMap();
            CreateMap<BLL.App.DTO.Unit, PublicApi.DTO.v1.Unit>().ReverseMap();
            CreateMap<BLL.App.DTO.Vehicle, PublicApi.DTO.v1.Vehicle>().ReverseMap();
            // CreateMap<AppRole, DAL.App.DTO.Identity.AppRole>().ReverseMap();
            // CreateMap<AppUser, DAL.App.DTO.Identity.AppUser>().ReverseMap();
        }
    }
}