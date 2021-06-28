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
    public class ParcelService: BaseEntityService<IAppUnitOfWork, IParcelRepository, BLLAppDTO.Parcel, DALAppDTO.Parcel>, IParcelService
    {
        public ParcelService(IAppUnitOfWork serviceUow, IParcelRepository serviceRepository, IMapper mapper) : base(serviceUow, serviceRepository, new ParcelMapper(mapper))
        {
        }
    }
}