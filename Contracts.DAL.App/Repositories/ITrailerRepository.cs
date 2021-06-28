using System;
using System.Threading.Tasks;
using Contracts.DAL.Base.Repositories;
using DALAppDTO = DAL.App.DTO;

namespace Contracts.DAL.App.Repositories
{
    public interface ITrailerRepository: IBaseRepository<DALAppDTO.Trailer>, ITrailerRepositoryCustom<DALAppDTO.Trailer>
    {
        
    }
    public interface ITrailerRepositoryCustom<TEntity>
    {
        Task<bool> UserHasTrailers(Guid userId);
        
        Task<int> GetUserTrailerCount(Guid userId);
    }
}