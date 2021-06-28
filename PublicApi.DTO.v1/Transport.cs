using System;
using PublicApi.DTO.v1.Enums;

namespace PublicApi.DTO.v1
{
    public class TransportAdd
    {
        public ETransportStatus TransportStatus { get; set; }

        public decimal FinalPrice { get; set; }
        
        public DateTime PickUpTime { get; set; }

        public DateTime? EstimatedDeliveryTime { get; set; }

        public DateTime? DeliveredTime { get; set; }

        public Guid PickUpLocationId { get; set; }

        public Guid TransportNeedId { get; set; }

        public Guid TransportOfferId { get; set; }
    }

    public class Transport
    {
        public Guid Id { get; set; }
        
        public ETransportStatus TransportStatus { get; set; }

        public decimal FinalPrice { get; set; }
        
        public DateTime PickUpTime { get; set; }

        public DateTime? EstimatedDeliveryTime { get; set; }

        public DateTime? DeliveredTime { get; set; }

        public Location PickUpLocation { get; set; } = default!;

        public Guid TransportNeedId { get; set; }

        public Guid TransportOfferId { get; set; }
    }
}