using System;
using Contracts.BLL.App.Services;
using Contracts.BLL.Base;
using Contracts.BLL.Base.Services;
using BLLAppDTO = BLL.App.DTO;
using DALAppDTO = DAL.App.DTO;

namespace Contracts.BLL.App
{
    public interface IAppBLL: IBaseBLL
    {
        IDimensionService Dimensions { get; }
        ILocationService Locations { get; }
        IParcelService Parcels { get; }
        ITrailerService Trailers { get; }
        ITransportMetaService TransportMeta { get; }
        ITransportNeedService TransportNeeds { get; }
        ITransportOfferService TransportOffers { get; }
        ITransportService Transports { get; }
        IUnitService Units { get; }
        IVehicleService Vehicles { get; }
        IVehicleTypeService VehicleTypes { get; }
    }
}