using AutoMapper;

namespace PublicApi.DTO.v1.Mappers
{
    public class VehicleTypeMapper: BaseMapper<PublicApi.DTO.v1.VehicleType, BLL.App.DTO.VehicleType>
    {
        public VehicleTypeMapper(IMapper mapper) : base(mapper)
        {
        }

        public static BLL.App.DTO.VehicleType MapToBll(VehicleTypeAdd vehicleTypeAdd)
        {
            var bllVehicleType = new BLL.App.DTO.VehicleType
            {
                VehicleTypeName = vehicleTypeAdd.VehicleTypeName,
                ForGoods = vehicleTypeAdd.ForGoods
            };
            return bllVehicleType;
        }
    }
}