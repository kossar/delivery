using System;
using System.Threading.Tasks;
using Contracts.DAL.Base.Repositories;
using DALAppDTO = DAL.App.DTO;

namespace Contracts.DAL.App.Repositories
{
    public interface IVehicleRepository: IBaseRepository<DALAppDTO.Vehicle>, IVehicleRepositoryCustom<DALAppDTO.Vehicle>
    {
        
    }
    public interface IVehicleRepositoryCustom<TEntity>
    {
        Task<bool> HasVehicles(Guid id);
        
        Task<int> GetUserVehiclesCount(Guid userId);
    }
}