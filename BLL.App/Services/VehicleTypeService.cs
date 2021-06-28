using AutoMapper;
using BLL.App.Mappers;
using BLL.Base.Services;
using Contracts.BLL.App.Services;
using Contracts.BLL.Base.Mappers;
using Contracts.DAL.App;
using Contracts.DAL.App.Repositories;
using BLLAppDTO = BLL.App.DTO;
using DALAppDTO = DAL.App.DTO;

namespace BLL.App.Services
{
    public class VehicleTypeService: BaseEntityService<IAppUnitOfWork, IVehicleTypeRepository, BLLAppDTO.VehicleType, DALAppDTO.VehicleType>, IVehicleTypeService
    {
        public VehicleTypeService(IAppUnitOfWork serviceUow, IVehicleTypeRepository serviceRepository, IMapper mapper) : base(serviceUow, serviceRepository, new VehicleTypeMapper(mapper))
        {
        }
    }
}