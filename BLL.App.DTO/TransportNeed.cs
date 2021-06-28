using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BLL.App.DTO.Enums;
using BLL.App.DTO.Identity;
using Contracts.Domain.Base;
using Domain.Base;

namespace BLL.App.DTO
{
    public class TransportNeed : DomainEntityId, IDomainAppUserId, IDomainAppUser<AppUser>
    {

        //public Guid? TransportNeedInfoId { get; set; }
        [Display(Name = "Info", ResourceType = typeof(Resources.BLL.App.DTO.TransportNeed))]
        [MaxLength(1024)]
        public string? TransportNeedInfo { get; set; }

        [Display(Name = "TransportType", ResourceType = typeof(Resources.BLL.App.DTO.TransportNeed))]
        public ETransportType TransportType { get; set; }

        [Display(Name = "PersonCount", ResourceType = typeof(Resources.BLL.App.DTO.TransportNeed))]
        public int PersonCount { get; set; }
        
        [Display(Name = "_TransportMeta", ResourceType = typeof(Resources.BLL.App.DTO.TransportMeta))]
        public Guid TransportMetaId { get; set; }
        
        [Display(Name = "_TransportMeta", ResourceType = typeof(Resources.BLL.App.DTO.TransportMeta))]
        public TransportMeta? TransportMeta { get; set; }
        
        public Guid AppUserId { get; set; }
        public AppUser? AppUser { get; set; }
        
        public List<Guid>? ParcelIds { get; set; }
        public ICollection<Parcel>? Parcels { get; set; }
        public ICollection<Transport>? Transports { get; set; }

        //[Display(Name = "StartDest", ResourceType = typeof(Resources.BLL.App.DTO.TransportNeed))]
        //public string StartDest => "From: " + TransportMeta!.StartLocation!.City + " To: " +
        //                           TransportMeta!.DestinationLocation!.City;

    }
}