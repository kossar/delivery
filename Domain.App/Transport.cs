using System;
using Domain.App.Enums;
using Domain.App.Identity;
using Domain.Base;

namespace Domain.App
{
    public class Transport : DomainEntityId
    {
        public ETransportStatus TransportStatus { get; set; }

        public decimal FinalPrice { get; set; }
        
        public DateTime PickUpTime { get; set; }

        public DateTime? EstimatedDeliveryTime { get; set; }

        public DateTime? DeliveredTime { get; set; }

        public DateTime LastUpdateTime { get; set; } = DateTime.Now;
        
        public bool UpdatedByTransportOffer { get; set; }

        public Guid PickUpLocationId { get; set; }
        public Location? PickUpLocation { get; set; }

        public Guid TransportNeedId { get; set; }
        public TransportNeed? TransportNeed { get; set; }

        public Guid TransportOfferId { get; set; }
        public TransportOffer? TransportOffer { get; set; }
        
    }
}