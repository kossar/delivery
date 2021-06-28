using AutoMapper;

namespace BLL.App.Mappers
{
    public class VehicleTypeMapper: BaseMapper<BLL.App.DTO.VehicleType, DAL.App.DTO.VehicleType>
    {
        public VehicleTypeMapper(IMapper mapper) : base(mapper)
        {
        }
    }
}