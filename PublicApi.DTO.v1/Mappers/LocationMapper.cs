using AutoMapper;

namespace PublicApi.DTO.v1.Mappers
{
    public class LocationMapper: BaseMapper<PublicApi.DTO.v1.Location, BLL.App.DTO.Location>
    {
        public LocationMapper(IMapper mapper) : base(mapper)
        {
        }

        public static BLL.App.DTO.Location MapToBll(LocationAdd locationAdd)
        {
            var bllLocation = new BLL.App.DTO.Location()
            {
                Country = locationAdd.Country,
                City = locationAdd.City,
                Address = locationAdd.Address,
                LocationInfo = locationAdd.LocationInfo
            };
            return bllLocation;
        }
    }
}