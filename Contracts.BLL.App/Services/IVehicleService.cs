using System;
using System.Threading.Tasks;
using Contracts.BLL.Base.Services;
using Contracts.DAL.App.Repositories;
using BLLAppDTO = BLL.App.DTO;
using DALAppDTO = DAL.App.DTO;
    
namespace Contracts.BLL.App.Services

{
    public interface IVehicleService: IBaseEntityService<BLLAppDTO.Vehicle, DALAppDTO.Vehicle>, IVehicleRepositoryCustom<BLLAppDTO.Vehicle>
    {
        Task<BLLAppDTO.Vehicle?> AddWithTransportOffer(BLLAppDTO.Vehicle vehicle, Guid userid, Guid? transportOfferId, bool noTracking = true);
    }
}