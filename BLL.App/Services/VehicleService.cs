using System;
using System.Threading.Tasks;
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
    public class VehicleService: BaseEntityService<IAppUnitOfWork, IVehicleRepository, BLLAppDTO.Vehicle, DALAppDTO.Vehicle>, IVehicleService
    {
        public VehicleService(IAppUnitOfWork serviceUow, IVehicleRepository serviceRepository, IMapper mapper) : base(serviceUow, serviceRepository, new VehicleMapper(mapper))
        {
        }

        public async Task<bool> HasVehicles(Guid userId)
        {
            return await ServiceRepository.HasVehicles(userId);
        }

        public async Task<int> GetUserVehiclesCount(Guid userId)
        {
            return await ServiceRepository.GetUserVehiclesCount(userId);
        }

        public async Task<BLLAppDTO.Vehicle?> AddWithTransportOffer(BLLAppDTO.Vehicle vehicle, Guid userid, Guid? transportOfferId, bool noTracking = true)
        {
            var addedVehicle = ServiceUow.Vehicles.Add(Mapper.Map(vehicle)!);
            if (transportOfferId != null)
            {
                var transportOffer = await ServiceUow.TransportOffers.FirstOrDefaultAsync(transportOfferId.Value, userid);
                if (transportOffer != null)
                {
                    transportOffer.Vehicle = null;
                    transportOffer.VehicleId = addedVehicle.Id;
                    ServiceUow.TransportOffers.Update(transportOffer);
                }
            }

            return Mapper.Map(addedVehicle);
        }
    }
}