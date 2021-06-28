using AutoMapper;

namespace BLL.App.Mappers
{
    public class LocationMapper: BaseMapper<BLL.App.DTO.Location, DAL.App.DTO.Location>
    {
        public LocationMapper(IMapper mapper) : base(mapper)
        {
        }
    }
}