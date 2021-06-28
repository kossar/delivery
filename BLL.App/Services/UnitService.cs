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
    public class UnitService: BaseEntityService<IAppUnitOfWork, IUnitRepository, BLLAppDTO.Unit, DALAppDTO.Unit>, IUnitService
    {
        public UnitService(IAppUnitOfWork serviceUow, IUnitRepository serviceRepository, IMapper mapper) : base(serviceUow, serviceRepository, new UnitMapper(mapper))
        {
        }

        public async Task<IEnumerable<BLLAppDTO.Unit>> GetLengthUnits(bool noTracking = true)
        {
            var units = await ServiceRepository.GetLengthUnits(noTracking);

            return units.Select(u => Mapper.Map(u))!;
        }

        public async Task<IEnumerable<BLLAppDTO.Unit>> GetWeightUnits(bool noTracking = true)
        {
            var units = await ServiceRepository.GetWeightUnits(noTracking);

            return units.Select(u => Mapper.Map(u))!;
        }
    }
}