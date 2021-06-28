using Contracts.DAL.Base.Repositories;
using DALAppDTO = DAL.App.DTO;

namespace Contracts.DAL.App.Repositories
{
    public interface IDimensionsRepository: IBaseRepository<DALAppDTO.Dimensions>, IDimensionsRepositoryCustom<DALAppDTO.Dimensions>
    {
        
    }

    public interface IDimensionsRepositoryCustom<TEntity>
    {
        
    }
}