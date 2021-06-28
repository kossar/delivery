using Contracts.DAL.App.Repositories;
using Contracts.DAL.Base;

namespace Contracts.DAL.App
{
    public interface IAppUnitOfWork : IBaseUnitOfWork
    {
        IDimensionsRepository Dimensions { get; }
        ILocationRepository Locations { get; }
        IParcelRepository Parcels { get; }
        ITrailerRepository Trailers { get; }
        ITransportMetaRepository TransportMeta { get; }
        ITransportNeedRepository TransportNeeds { get; }
        ITransportOfferRepository TransportOffers { get; }
        ITransportRepository Transports { get; }
        IUnitRepository Units { get; }
        IVehicleRepository Vehicles { get; }
        IVehicleTypeRepository VehicleTypes { get; }
    }
}