using System;
using System.ComponentModel.DataAnnotations;
using PublicApi.DTO.v1.Enums;

namespace PublicApi.DTO.v1
{
    public class TransportOfferAdd
    {
        [MaxLength(1024)]
        public string? TransportOfferInfo { get; set; }

        public ETransportType TransportType { get; set; }

        public decimal Price { get; set; }

        public int AvailableLoadCapacity { get; set; }
        public int FreeSeats { get; set; }
        
        public Guid TransportMetaId { get; set; }
        
        public Guid UnitId { get; set; }

        public Guid VehicleId { get; set; }

        public Guid? TrailerId { get; set; }
    }

    public class TransportOfferListItem
    {
        public Guid Id { get; set; }
        
        public string? TransportOfferInfo { get; set; }

        public ETransportType TransportType { get; set; }

        public decimal Price { get; set; }

        public int AvailableLoadCapacity { get; set; }
        public int FreeSeats { get; set; }

        public TransportMeta? TransportMeta { get; set; }

        public Guid AppUserId { get; set; }
        public Guid VehicleId { get; set; }

        public Guid UnitId { get; set; }
        public Guid? TrailerId { get; set; }
    }

    public class TransportOffer
    {   
        public Guid Id { get; set; }
        
        [MaxLength(1024)]
        public string? TransportOfferInfo { get; set; }

        public ETransportType TransportType { get; set; }

        public decimal Price { get; set; }

        public int AvailableLoadCapacity { get; set; }
        public int FreeSeats { get; set; }
        
        public Guid AppUserId { get; set; }

        public Guid UnitId { get; set; }

        public Vehicle Vehicle { get; set; } = default!;

        public TransportMeta TransportMeta { get; set; } = default!;

        public Trailer? Trailer { get; set; }
    }
}