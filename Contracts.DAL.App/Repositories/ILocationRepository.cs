using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Contracts.DAL.Base.Repositories;
using DALAppDTO = DAL.App.DTO;

namespace Contracts.DAL.App.Repositories
{
    public interface ILocationRepository: IBaseRepository<DALAppDTO.Location>,  ILocationRepositoryCustom<DALAppDTO.Location>
    {
        
    }

    public interface ILocationRepositoryCustom<TEntity>
    {
        Task<IEnumerable<TEntity>> GetAllWithAdditionalDataCount(Guid userId = default, bool noTracking = true);
    }
}