using System;
using System.Collections.Generic;
using System.Linq;
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
    public class TransportOfferService: BaseEntityService<IAppUnitOfWork, ITransportOfferRepository, BLLAppDTO.TransportOffer, DALAppDTO.TransportOffer>, ITransportOfferService
    {
        public TransportOfferService(IAppUnitOfWork serviceUow, ITransportOfferRepository serviceRepository, IMapper mapper) : base(serviceUow, serviceRepository, new TransportOfferMapper(mapper))
        {
        }
        
        public async Task<IEnumerable<BLLAppDTO.TransportOffer>> GetByCountAsync(int count, bool noTracking = true)
        {
            var offers = await ServiceRepository.GetByCountAsync(count, noTracking);

            return offers.Select(n => Mapper.Map(n))!;

        }

        public async Task<IEnumerable<BLLAppDTO.TransportOffer>> GetUserUnFinishedTransportOffers(Guid userId, bool noTracking = true)
        {
            var offers =  await ServiceRepository.GetUserUnFinishedTransportOffers(userId);
            return offers.Select(x => Mapper.Map(x))!;
        }

        public async Task<int> GetUserUnFinishedTransportOffersCount(Guid userId, bool noTracking = true)
        {
            return await ServiceRepository.GetUserUnFinishedTransportOffersCount(userId, noTracking);
        }

        public async Task<BLLAppDTO.TransportOffer?> AddTrailerIdToTransportOffer(Guid transportOfferId, Guid trailerId)
        {
            var transportOffer = await ServiceUow.TransportOffers.FirstOrDefaultAsync(transportOfferId);
            if (transportOffer == null)
            {
                return null;
            }

            var trailer = await ServiceUow.Trailers.FirstOrDefaultAsync(trailerId);
            if (trailer == null)
            {
                return null;
            }

            transportOffer.Trailer = null;
            transportOffer.TrailerId = trailer.Id;
            return Mapper.Map(ServiceUow.TransportOffers.Update(transportOffer));
        }
        

        public BLLAppDTO.TransportOffer? AddVehicleIdToTransportOffer(BLLAppDTO.TransportOffer transportOffer, Guid vehicleId, Guid userId)
        {
            var dalTransportOffer = Mapper.Map(transportOffer);
            
            dalTransportOffer!.VehicleId = vehicleId;
            dalTransportOffer.Vehicle = null;
            var addedTransportOffer = ServiceUow.TransportOffers.Update(dalTransportOffer!);
            //await ServiceUow.SaveChangesAsync();
            return Mapper.Map(addedTransportOffer);
        }
        

        public BLLAppDTO.TransportOffer? RemoveTrailerFromTransportOffer(BLLAppDTO.TransportOffer transportOffer)
        {
            transportOffer.Trailer = null;
            transportOffer.TrailerId = null;
            var updatedTransportOffer = ServiceUow.TransportOffers.Update(Mapper.Map(transportOffer)!);

            return Mapper.Map(updatedTransportOffer);
        }
    }
}