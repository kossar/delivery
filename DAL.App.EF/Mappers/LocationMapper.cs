using AutoMapper;

namespace DAL.App.EF.Mappers
{
    public class LocationMapper: BaseMapper<DAL.App.DTO.Location, Domain.App.Location>
    {
        public LocationMapper(IMapper mapper) : base(mapper)
        {
        }
    }
}