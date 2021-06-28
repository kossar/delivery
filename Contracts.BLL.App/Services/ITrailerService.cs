using System;
using System.Threading.Tasks;
using Contracts.BLL.Base.Services;
using Contracts.DAL.App.Repositories;
using BLLAppDTO = BLL.App.DTO;
using DALAppDTO = DAL.App.DTO;

namespace Contracts.BLL.App.Services
{
    public interface ITrailerService: IBaseEntityService<BLLAppDTO.Trailer, DALAppDTO.Trailer>, ITrailerRepositoryCustom<BLLAppDTO.Trailer>
    {
        Task<BLLAppDTO.Trailer?> AddWithTransportOffer(BLLAppDTO.Trailer trailer, Guid userid, Guid? transportOfferId, bool noTracking = true);
    }
}