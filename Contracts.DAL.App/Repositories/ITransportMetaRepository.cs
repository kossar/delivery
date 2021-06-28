using Contracts.DAL.Base.Repositories;
using DALAppDTO = DAL.App.DTO;

namespace Contracts.DAL.App.Repositories
{
    public interface ITransportMetaRepository: IBaseRepository<DALAppDTO.TransportMeta>, ITransportMetaRepositoryCustom<DALAppDTO.TransportMeta>
    {
        
    }
    public interface ITransportMetaRepositoryCustom<TEntity>
    {
        
    }
}