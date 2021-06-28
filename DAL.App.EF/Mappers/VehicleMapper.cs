using AutoMapper;

namespace DAL.App.EF.Mappers
{
    public class VehicleMapper: BaseMapper<DAL.App.DTO.Vehicle, Domain.App.Vehicle>
    {
        public VehicleMapper(IMapper mapper) : base(mapper)
        {
        }
    }
}