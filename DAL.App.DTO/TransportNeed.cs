using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Contracts.Domain.Base;
using DAL.App.DTO.Enums;
using DAL.App.DTO.Identity;
using Domain.Base;

namespace DAL.App.DTO
{
    public class TransportNeed : DomainEntityId, IDomainAppUserId, IDomainAppUser<AppUser>
    {
        [MaxLength(1024)] public string? TransportNeedInfo { get; set; }

        public ETransportType TransportType { get; set; }

        public int PersonCount { get; set; }
        
        public Guid TransportMetaId { get; set; }
        public TransportMeta? TransportMeta { get; set; }
        public Guid AppUserId { get; set; }
        public AppUser? AppUser { get; set; }

        public Guid? OrganisationId { get; set; }
        public Organisation? Organisation { get; set; }

        public List<Guid>? ParcelIds { get; set; }

        public ICollection<Parcel>? Parcels { get; set; }
        public ICollection<Transport>? Transports { get; set; }
        
    }
}