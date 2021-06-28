using Contracts.DAL.Base.Repositories;
using DALAppDTO = DAL.App.DTO;

namespace Contracts.DAL.App.Repositories
{
    public interface IParcelRepository: IBaseRepository<DALAppDTO.Parcel>, IParcelRepositoryCustom<DALAppDTO.Parcel>
    {
        
    }

    public interface IParcelRepositoryCustom<TEntity>
    {
        
    }
}