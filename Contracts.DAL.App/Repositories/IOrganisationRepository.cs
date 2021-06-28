using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Contracts.DAL.Base.Repositories;
using DALAppDTO = DAL.App.DTO;

namespace Contracts.DAL.App.Repositories
{
    public interface IOrganisationRepository: IBaseRepository<DALAppDTO.Organisation>, IOrganisationRepositoryCustom<DALAppDTO.Organisation>
    {
        
    }
    public interface IOrganisationRepositoryCustom<TEntity>
    {
        Task<IEnumerable<TEntity>> GetAllWithAdditionalDataCount(Guid userId, bool noTracking = true);
        
        Task<TEntity?> GetPublicOrganisationDetails(Guid userId, bool noTracking = true);

    }
}