using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Contracts.DAL.Base.Repositories;
using DALAppDTO = DAL.App.DTO;

namespace Contracts.DAL.App.Repositories
{
    public interface ITransportNeedRepository: IBaseRepository<DALAppDTO.TransportNeed>, ITransportNeedRepositoryCustom<DALAppDTO.TransportNeed>
    {
        
    }
    public interface ITransportNeedRepositoryCustom<TEntity>
    {
        Task<TEntity?> GetWithParcelIds(Guid transportNeedId, bool noTracking = true);

        Task<IEnumerable<TEntity>> GetByCountAsync(int count, bool noTracking = true);
        
        Task<IEnumerable<TEntity>> GetUserUnFinishedTransportNeeds(Guid userId, bool noTracking = true);
    }
}