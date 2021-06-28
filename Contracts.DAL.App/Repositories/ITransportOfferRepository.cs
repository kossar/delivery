using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Contracts.DAL.Base.Repositories;
using DALAppDTO = DAL.App.DTO;

namespace Contracts.DAL.App.Repositories
{
    public interface ITransportOfferRepository: IBaseRepository<DALAppDTO.TransportOffer>, ITransportOfferRepositoryCustom<DALAppDTO.TransportOffer>
    {
        
    }
    public interface ITransportOfferRepositoryCustom<TEntity>
    {
        Task<IEnumerable<TEntity>> GetByCountAsync(int count,  bool noTracking = true);
        
        Task<IEnumerable<TEntity>> GetUserUnFinishedTransportOffers(Guid userId, bool noTracking = true);
        
        Task<int> GetUserUnFinishedTransportOffersCount(Guid userId, bool noTracking = true);
        
    }
}