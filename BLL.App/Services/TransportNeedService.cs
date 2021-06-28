using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BLL.App.Mappers;
using BLL.Base.Services;
using Contracts.BLL.App.Services;
using Contracts.BLL.Base.Mappers;
using Contracts.DAL.App;
using Contracts.DAL.App.Repositories;
using BLLAppDTO = BLL.App.DTO;
using DALAppDTO = DAL.App.DTO;

namespace BLL.App.Services
{
    public class TransportNeedService: BaseEntityService<IAppUnitOfWork, ITransportNeedRepository, BLLAppDTO.TransportNeed, DALAppDTO.TransportNeed>, ITransportNeedService
    {
        public TransportNeedService(IAppUnitOfWork serviceUow, ITransportNeedRepository serviceRepository, IMapper mapper) : base(serviceUow, serviceRepository, new TransportNeedMapper(mapper))
        {
        }

        public async Task<BLLAppDTO.TransportNeed?> GetWithParcelIds(Guid transportNeedId, bool noTracking = true)
        {
            return Mapper.Map(await ServiceRepository.GetWithParcelIds(transportNeedId, noTracking));
        }

        public async Task<IEnumerable<BLLAppDTO.TransportNeed>> GetByCountAsync(int count, bool noTracking = true)
        {
            var transportNeeds = await ServiceRepository.GetByCountAsync(count, noTracking);

            return transportNeeds.Select(n => Mapper.Map(n))!;

        }

        public async Task<IEnumerable<BLLAppDTO.TransportNeed>> GetUserUnFinishedTransportNeeds(Guid userId, bool noTracking = true)
        {
            var res =  await ServiceRepository.GetUserUnFinishedTransportNeeds(userId);
            return res.Select(x => Mapper.Map(x))!;
        }
    }
}