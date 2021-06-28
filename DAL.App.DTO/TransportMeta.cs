using System;
using Domain.Base;

namespace DAL.App.DTO
{
    public class TransportMeta : DomainEntityId
    {
        
        public Guid StartLocationId { get; set; }
        public Location? StartLocation { get; set; }

        public Guid DestinationLocationId { get; set; }
        public Location? DestinationLocation { get; set; }

        public DateTime StartTime { get; set; }

        public TransportNeed? TransportNeed { get; set; }
        public TransportOffer? TransportOffer { get; set; }
    }
}