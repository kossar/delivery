using System;
using AutoMapper;
using BLL.App.Mappers;
using BLL.App.Services;
using BLL.Base;
using BLL.Base.Services;
using Contracts.BLL.App;
using Contracts.BLL.App.Services;
using Contracts.BLL.Base.Services;
using Contracts.DAL.App;
using Contracts.DAL.App.Repositories;
using Contracts.DAL.Base.Repositories;

namespace BLL.App
{
    public class AppBLL: BaseBLL<IAppUnitOfWork>, IAppBLL
    {
        protected IMapper Mapper;
        
        public AppBLL(IAppUnitOfWork uow, IMapper mapper) : base(uow)
        {
            Mapper = mapper;
        }

        public IDimensionService Dimensions  => 
            GetService<IDimensionService>(() => new DimensionService(Uow, Uow.Dimensions, Mapper));
        
        public ILocationService Locations  => 
            GetService<ILocationService>(() => new LocationService(Uow, Uow.Locations, Mapper));
        
        public IParcelService Parcels  => 
            GetService<IParcelService>(() => new ParcelService(Uow, Uow.Parcels, Mapper));
        
        public ITrailerService Trailers  => 
            GetService<ITrailerService>(() => new TrailerService(Uow, Uow.Trailers, Mapper));
        
        public ITransportMetaService TransportMeta  => 
            GetService<ITransportMetaService>(() => new TransportMetaService(Uow, Uow.TransportMeta, Mapper));
        
        public ITransportNeedService TransportNeeds  => 
            GetService<ITransportNeedService>(() => new TransportNeedService(Uow, Uow.TransportNeeds, Mapper));

        public ITransportOfferService TransportOffers  => 
            GetService<ITransportOfferService>(() => new TransportOfferService(Uow, Uow.TransportOffers, Mapper));
        
        
        public ITransportService Transports  => 
            GetService<ITransportService>(() => new TransportService(Uow, Uow.Transports, Mapper));
        
        public IUnitService Units  => 
            GetService<IUnitService>(() => new UnitService(Uow, Uow.Units, Mapper));


        public IVehicleService Vehicles  => 
            GetService<IVehicleService>(() => new VehicleService(Uow, Uow.Vehicles, Mapper));

        public IVehicleTypeService VehicleTypes => 
            GetService<IVehicleTypeService>(() => new VehicleTypeService(Uow, Uow.VehicleTypes, Mapper));
    }
    
}