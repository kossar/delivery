using System;
using DAL.App.DTO.Enums;
using Domain.Base;

namespace DAL.App.DTO
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