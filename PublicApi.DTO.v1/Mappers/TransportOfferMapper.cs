using System;
using AutoMapper;
using PublicApi.DTO.v1.Enums;

namespace PublicApi.DTO.v1.Mappers
{
    public class TransportOfferMapper: BaseMapper<PublicApi.DTO.v1.TransportOffer, BLL.App.DTO.TransportOffer>
    {
        public TransportOfferMapper(IMapper mapper) : base(mapper)
        {
        }

        public static BLL.App.DTO.TransportOffer MapToBll(TransportOfferAdd transportOfferAdd)
        {
            var bllTransportOffer = new BLL.App.DTO.TransportOffer()
            {
                FreeSeats = transportOfferAdd.FreeSeats,
                Price = transportOfferAdd.Price,
                TransportOfferInfo = transportOfferAdd.TransportOfferInfo,
                TransportType = (BLL.App.DTO.Enums.ETransportType) transportOfferAdd.TransportType,
                AvailableLoadCapacity = transportOfferAdd.AvailableLoadCapacity,
                TransportMetaId = transportOfferAdd.TransportMetaId,
                VehicleId = transportOfferAdd.VehicleId,
                TrailerId = transportOfferAdd.TrailerId,
                UnitId = transportOfferAdd.UnitId
            };

            return bllTransportOffer;
        }
        
        public static TransportOfferListItem MapToListItem(TransportOffer? transportOffer)
        {
            var listItem = new  PublicApi.DTO.v1.TransportOfferListItem()
            {
                Id = transportOffer!.Id,
                FreeSeats = transportOffer.FreeSeats,
                Price = transportOffer.Price,
                TransportOfferInfo = transportOffer.TransportOfferInfo,
                TransportType = (ETransportType) transportOffer.TransportType,
                TransportMeta = transportOffer.TransportMeta,
                AvailableLoadCapacity = transportOffer.AvailableLoadCapacity,
                VehicleId = transportOffer.Vehicle.Id,
                AppUserId = transportOffer.AppUserId
            };
            if (transportOffer.Trailer != null)
            {
                listItem.TrailerId = transportOffer.Trailer.Id;
            }

            return listItem;
        }
        
    }
}