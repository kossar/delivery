using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Contracts.Domain.Base;
using DAL.App.DTO.Enums;
using DAL.App.DTO.Identity;
using Domain.Base;

namespace DAL.App.DTO
{
    public class TransportOffer : DomainEntityId, IDomainAppUserId, IDomainAppUser<AppUser>
    {
        
        [MaxLength(1024)] public string? TransportOfferInfo { get; set; }

        public ETransportType TransportType { get; set; }

        public decimal Price { get; set; }

        public int AvailableLoadCapacity { get; set; }
        public int FreeSeats { get; set; }
        
        public Guid TransportMetaId { get; set; }
        public TransportMeta? TransportMeta { get; set; }

        public Guid VehicleId { get; set; }
        public Vehicle? Vehicle { get; set; }

        public Guid? TrailerId { get; set; }
        public Trailer? Trailer { get; set; }
        public Guid AppUserId { get; set; }
        public AppUser? AppUser { get; set; }
        
        public Guid UnitId { get; set; }
        public Unit? Unit { get; set; }

        public Guid? OrganisationId { get; set; }
        public Organisation? Organisation { get; set; }

        public ICollection<Transport>? Transports { get; set; }
    }
}