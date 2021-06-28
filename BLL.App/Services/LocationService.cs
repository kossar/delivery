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
    public class LocationService: BaseEntityService<IAppUnitOfWork, ILocationRepository, BLLAppDTO.Location, DALAppDTO.Location>, ILocationService
    {
        public LocationService(IAppUnitOfWork serviceUow, ILocationRepository serviceRepository, IMapper mapper) : base(serviceUow, serviceRepository, new LocationMapper(mapper))
        {
        }

        public async Task<IEnumerable<BLLAppDTO.Location>> GetAllWithAdditionalDataCount(Guid userId, bool noTracking = true)
        {
            return (await ServiceRepository.GetAllWithAdditionalDataCount(userId, noTracking)).Select(x => Mapper.Map(x))!;
        }
    }
}