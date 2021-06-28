using System;
using System.Threading.Tasks;
using Contracts.BLL.Base.Services;
using Contracts.DAL.App.Repositories;
using BLLAppDTO = BLL.App.DTO;
using DALAppDTO = DAL.App.DTO;

namespace Contracts.BLL.App.Services
{
    public interface ITransportOfferService: IBaseEntityService<BLLAppDTO.TransportOffer, DALAppDTO.TransportOffer>, ITransportOfferRepositoryCustom<BLLAppDTO.TransportOffer>
    {
        Task<BLLAppDTO.TransportOffer?> AddTrailerIdToTransportOffer(Guid transportOfferId, Guid trailerId);
        
        BLLAppDTO.TransportOffer? AddVehicleIdToTransportOffer(BLLAppDTO.TransportOffer transportOffer, Guid vehicleId, Guid userId);
        
        BLLAppDTO.TransportOffer? RemoveTrailerFromTransportOffer(BLLAppDTO.TransportOffer transportOffer);
        
    }
}