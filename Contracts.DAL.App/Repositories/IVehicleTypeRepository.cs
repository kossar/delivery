using Contracts.DAL.Base.Repositories;
using DALAppDTO = DAL.App.DTO;

namespace Contracts.DAL.App.Repositories
{
    public interface IVehicleTypeRepository: IBaseRepository<DALAppDTO.VehicleType>,  IVehicleTypeRepositoryCustom<DALAppDTO.VehicleType>
    {
        
    }

    public interface IVehicleTypeRepositoryCustom<TEntity>
    {
    }
}