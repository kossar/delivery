using AutoMapper;

namespace BLL.App.Mappers
{
    public class VehicleMapper: BaseMapper<BLL.App.DTO.Vehicle, DAL.App.DTO.Vehicle>
    {
        public VehicleMapper(IMapper mapper) : base(mapper)
        {
        }
    }
}