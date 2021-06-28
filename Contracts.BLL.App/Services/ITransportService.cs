using System;
using System.Threading.Tasks;
using Contracts.BLL.Base.Services;
using Contracts.DAL.App.Repositories;
using Domain.App.Enums;
using BLLAppDTO = BLL.App.DTO;
using DALAppDTO = DAL.App.DTO;

namespace Contracts.BLL.App.Services
{
    public interface ITransportService: IBaseEntityService<BLLAppDTO.Transport, DALAppDTO.Transport>, ITransportRepositoryCustom<BLLAppDTO.Transport>
    {

        Task<BLLAppDTO.Transport> InitialTransportAdd(BLLAppDTO.Transport transport, Guid transportOfferId, Guid transportNeedId, Guid userId);
        
        Task<BLLAppDTO.Transport> TransportActionAdd(BLLAppDTO.Transport transport, Guid userId, BLLAppDTO.Enums.ETransportStatus status);
        
        Task<BLLAppDTO.Transport> TransportSubmit(BLLAppDTO.Transport transport, Guid transportOfferId, Guid transportNeedId, Guid userId);
    }
}
