using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Contracts.Domain.Base;
using Domain.App.Enums;
using Domain.App.Identity;
using Domain.Base;

namespace Domain.App
{
    public class TransportNeed : DomainEntityId, IDomainAppUserId, IDomainAppUser<AppUser>
    {

        public Guid? TransportNeedInfoId { get; set; }
        [InverseProperty(nameof(AppLangString.TransportNeedInfo))]
        public virtual AppLangString? TransportNeedInfo { get; set; }

        public ETransportType TransportType { get; set; }

        public int PersonCount { get; set; }
        
        public Guid TransportMetaId { get; set; }
        public TransportMeta? TransportMeta { get; set; }
        public Guid AppUserId { get; set; }
        public AppUser? AppUser { get; set; }
        public ICollection<Parcel>? Parcels { get; set; }
        public ICollection<Transport>? Transports { get; set; }
        
    }
}