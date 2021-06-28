using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BLL.App.DTO.Enums;
using BLL.App.Mappers;
using BLL.Base.Services;
using Contracts.BLL.App.Services;
using Contracts.BLL.Base.Mappers;
using Contracts.DAL.App;
using Contracts.DAL.App.Repositories;
using BLLAppDTO = BLL.App.DTO;
using DALAppDTO = DAL.App.DTO;
using ETransportStatus = DAL.App.DTO.Enums.ETransportStatus;

namespace BLL.App.Services
{
    public class TransportService: BaseEntityService<IAppUnitOfWork, ITransportRepository, BLLAppDTO.Transport, DALAppDTO.Transport>, ITransportService
    {
        public TransportService(IAppUnitOfWork serviceUow, ITransportRepository serviceRepository, IMapper mapper) : base(serviceUow, serviceRepository, new TransportMapper(mapper))
        {
        }

        public async Task<IEnumerable<BLLAppDTO.Transport>> GetUserTransportNeedTransports(Guid userId, bool noTracking = true)
        {
           var res =  await ServiceRepository.GetUserTransportNeedTransports(userId);
           return res.Select(x => Mapper.Map(x))!;
        }

        public async Task<IEnumerable<BLLAppDTO.Transport>> GetUserTransportOfferTransports(Guid userId, bool noTracking = true)
        {
            var res =  await ServiceRepository.GetUserTransportOfferTransports(userId);
            return res.Select(x => Mapper.Map(x))!;
        }

        public async Task<IEnumerable<BLLAppDTO.Transport>> GetUserTransportNeedsWaitingForUserAction(Guid userId, bool noTracking = true)
        {
            var res = await ServiceRepository.GetUserTransportNeedsWaitingForUserAction(userId);
            return res.Select(x => Mapper.Map(x))!;
        }

        public async Task<IEnumerable<BLLAppDTO.Transport>> GetUserTransportOffersWaitingForUserAction(Guid userId, bool noTracking = true)
        {
            var res = await ServiceRepository.GetUserTransportOffersWaitingForUserAction(userId);
            return res.Select(x => Mapper.Map(x))!;
        }

        public async Task<BLLAppDTO.Transport> InitialTransportAdd(BLLAppDTO.Transport transport, Guid transportOfferId, Guid transportNeedId, Guid userId)
        {
            var dalTransport = Mapper.Map(transport)!;
            dalTransport.TransportOfferId = transportOfferId;
            dalTransport.TransportNeedId = transportNeedId;
            dalTransport.TransportStatus = ETransportStatus.Submitted;
            var transportOffer = await ServiceUow.TransportOffers.FirstOrDefaultAsync(transportOfferId);
            dalTransport.UpdatedByTransportOffer = transportOffer!.AppUserId == userId;
            var addedTransport = ServiceUow.Transports.Add(dalTransport);

            return Mapper.Map(addedTransport)!;
        }
        

        public async Task<BLLAppDTO.Transport> TransportActionAdd(BLLAppDTO.Transport transport, Guid userId, BLLAppDTO.Enums.ETransportStatus status)
        {

            //var offer = await ServiceUow.TransportOffers.FirstOrDefaultAsync(transport.TransportOfferId);
            var need = await ServiceUow.TransportNeeds.FirstOrDefaultAsync(transport.TransportNeedId);

            var newTransport = transport;
            newTransport.Id = Guid.Empty;
            newTransport.PickUpLocationId = Guid.Empty;
            newTransport.PickUpLocation!.Id = Guid.Empty;
            newTransport.TransportStatus = status;
            newTransport.UpdatedByTransportOffer = userId != need!.AppUserId;
            newTransport.LastUpdateTime = DateTime.Now;
            if (status == DTO.Enums.ETransportStatus.Delivered)
            {
                newTransport.DeliveredTime = DateTime.Now;
            }
            var dalTransport = Mapper.Map(newTransport);

            var addedTransport = ServiceUow.Transports.Add(dalTransport!);

            return Mapper.Map(addedTransport)!;
        }

        public async Task<BLLAppDTO.Transport> TransportSubmit(BLLAppDTO.Transport transport,  Guid transportOfferId, Guid transportNeedId, Guid userId)
        {
            var dalTransport = Mapper.Map(transport)!;
            dalTransport.Id = Guid.Empty;
            dalTransport.TransportNeedId = transportNeedId;
            dalTransport.TransportOfferId = transportOfferId;
            dalTransport.LastUpdateTime = DateTime.Now;
            dalTransport.PickUpLocationId = Guid.Empty;
            dalTransport.PickUpLocation!.Id = Guid.Empty;
            dalTransport.TransportStatus = ETransportStatus.Submitted;
            var transportOffer = await ServiceUow.TransportOffers.FirstOrDefaultAsync(transportOfferId);
            dalTransport.UpdatedByTransportOffer = transportOffer!.AppUserId == userId;
            var addedTransport = ServiceUow.Transports.Add(dalTransport);

            return Mapper.Map(addedTransport)!;
        }
    }
}