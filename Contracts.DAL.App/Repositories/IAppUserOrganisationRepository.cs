using Contracts.DAL.Base.Repositories;
using DALAppDTO = DAL.App.DTO;

namespace Contracts.DAL.App.Repositories
{
    public interface IAppUserOrganisationRepository: IBaseRepository<DALAppDTO.AppUserOrganisation>, IAppUserOrganisationRepositoryCustom<DALAppDTO.AppUserOrganisation>
    {
        
    }
    public interface IAppUserOrganisationRepositoryCustom<TEntity>
    {
        
    }
}