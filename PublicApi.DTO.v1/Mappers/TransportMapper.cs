using AutoMapper;

namespace PublicApi.DTO.v1.Mappers
{
    public class TransportMapper: BaseMapper<PublicApi.DTO.v1.Transport, BLL.App.DTO.Transport>
    {
        public TransportMapper(IMapper mapper) : base(mapper)
        {
        }

        public static BLL.App.DTO.Transport MapToBll(TransportAdd transportAdd)
        {
            var bllTransport = new BLL.App.DTO.Transport()
            {
                TransportStatus = (BLL.App.DTO.Enums.ETransportStatus) transportAdd.TransportStatus,
                FinalPrice = transportAdd.FinalPrice,
                PickUpTime = transportAdd.PickUpTime,
                EstimatedDeliveryTime = transportAdd.EstimatedDeliveryTime,
                DeliveredTime = transportAdd.DeliveredTime,
                PickUpLocationId = transportAdd.PickUpLocationId,
                TransportNeedId = transportAdd.TransportNeedId,
                TransportOfferId = transportAdd.TransportOfferId
            };

            return bllTransport;
        }
    }
}