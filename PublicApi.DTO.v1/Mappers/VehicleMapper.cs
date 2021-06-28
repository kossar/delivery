using AutoMapper;

namespace PublicApi.DTO.v1.Mappers
{
    public class VehicleMapper: BaseMapper<PublicApi.DTO.v1.Vehicle, BLL.App.DTO.Vehicle>
    {
        public VehicleMapper(IMapper mapper) : base(mapper)
        {
        }

        public static BLL.App.DTO.Vehicle MapToBll(VehicleAdd vehicleAdd)
        {
            var bllVehicle = new BLL.App.DTO.Vehicle()
            {
                Make = vehicleAdd.Make,
                Model = vehicleAdd.Model,
                RegNr = vehicleAdd.RegNr,
                ReleaseDate = vehicleAdd.ReleaseDate,
                VehicleTypeId = vehicleAdd.VehicleTypeId,
            };
            return bllVehicle;
        }
    }
}