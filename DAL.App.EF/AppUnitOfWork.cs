using AutoMapper;
using Contracts.DAL.App;
using Contracts.DAL.App.Repositories;
using DAL.App.EF.Repositories;
using DAL.Base.EF;

namespace DAL.App.EF
{
    public class AppUnitOfWork: BaseUnitOfWork<AppDbContext>, IAppUnitOfWork
    {
        protected IMapper Mapper;
        public AppUnitOfWork(AppDbContext uowDbContext, IMapper mapper) : base(uowDbContext)
        {
            Mapper = mapper;
        }
        
        
        public IDimensionsRepository Dimensions =>
            GetRepository(() => new DimensionsRepository(UowDbContext, Mapper));
        public ILocationRepository Locations =>
            GetRepository(() => new LocationRepository(UowDbContext, Mapper));
        public IParcelRepository Parcels =>
            GetRepository(() => new ParcelRepository(UowDbContext, Mapper));
        public ITrailerRepository Trailers =>
            GetRepository(() => new TrailerRepository(UowDbContext, Mapper));
        public ITransportMetaRepository TransportMeta =>
            GetRepository(() => new TransportMetaRepository(UowDbContext, Mapper));
        public ITransportNeedRepository TransportNeeds =>
            GetRepository(() => new TransportNeedRepository(UowDbContext, Mapper));
        public ITransportOfferRepository TransportOffers =>
            GetRepository(() => new TransportOfferRepository(UowDbContext, Mapper));
        public ITransportRepository Transports =>
            GetRepository(() => new TransportRepository(UowDbContext, Mapper));
        public IUnitRepository Units =>
            GetRepository(() => new UnitRepository(UowDbContext, Mapper));
        public IVehicleRepository Vehicles =>
            GetRepository(() => new VehicleRepository(UowDbContext, Mapper));
        
        public IVehicleTypeRepository VehicleTypes =>
            GetRepository(() => new VehicleTypeRepository(UowDbContext, Mapper));
    }
}