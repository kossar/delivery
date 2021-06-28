using AutoMapper;
using BLL.App.Mappers;
using BLL.Base.Services;
using Contracts.BLL.App.Services;
using Contracts.DAL.App;
using Contracts.DAL.App.Repositories;
using BLLAppDTO = BLL.App.DTO;
using DALAppDTO = DAL.App.DTO;

namespace BLL.App.Services
{
    public class DimensionService: BaseEntityService<IAppUnitOfWork, IDimensionsRepository, BLLAppDTO.Dimensions, DALAppDTO.Dimensions>, IDimensionService
    {
        public DimensionService(IAppUnitOfWork serviceUow, IDimensionsRepository serviceRepository, IMapper mapper) : base(serviceUow, serviceRepository,new DimensionMapper(mapper))
        {
        }
    }
}