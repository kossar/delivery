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
    public class TransportMetaService: BaseEntityService<IAppUnitOfWork, ITransportMetaRepository, BLLAppDTO.TransportMeta, DALAppDTO.TransportMeta>, ITransportMetaService
    {
        public TransportMetaService(IAppUnitOfWork serviceUow, ITransportMetaRepository serviceRepository, IMapper mapper) : base(serviceUow, serviceRepository, new TransportMetaMapper(mapper))
        {
        }
    }
}