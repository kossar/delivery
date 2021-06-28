using System.Collections.Generic;
using System.Threading.Tasks;
using Contracts.DAL.Base.Repositories;
using DALAppDTO = DAL.App.DTO;

namespace Contracts.DAL.App.Repositories
{
    public interface IUnitRepository: IBaseRepository<DALAppDTO.Unit>, IUnitRepositoryCustom<DALAppDTO.Unit>
    {
        
    }
    public interface IUnitRepositoryCustom<TEntity>
    {
        Task<IEnumerable<TEntity>> GetLengthUnits(bool noTracking = true);
        
        Task<IEnumerable<TEntity>> GetWeightUnits(bool noTracking = true);
    }
}