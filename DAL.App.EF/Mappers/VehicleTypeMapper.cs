using AutoMapper;

namespace DAL.App.EF.Mappers
{
    public class VehicleTypeMapper: BaseMapper<DAL.App.DTO.VehicleType, Domain.App.VehicleType>
    {
        public VehicleTypeMapper(IMapper mapper) : base(mapper)
        {
        }
    }
}