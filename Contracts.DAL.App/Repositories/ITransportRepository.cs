using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Contracts.DAL.Base.Repositories;
using DALAppDTO = DAL.App.DTO;

namespace Contracts.DAL.App.Repositories
{
    public interface ITransportRepository: IBaseRepository<DALAppDTO.Transport>, ITransportRepositoryCustom<DALAppDTO.Transport>
    {
        
    }
    public interface ITransportRepositoryCustom<TEntity>
    {
        Task<IEnumerable<TEntity>> GetUserTransportNeedTransports(Guid userId, bool noTracking = true);
        
        Task<IEnumerable<TEntity>> GetUserTransportOfferTransports(Guid userId, bool noTracking = true);
        
        Task<IEnumerable<TEntity>> GetUserTransportNeedsWaitingForUserAction(Guid userId, bool noTracking = true);
        
        Task<IEnumerable<TEntity>> GetUserTransportOffersWaitingForUserAction(Guid userId, bool noTracking = true);
    }
}