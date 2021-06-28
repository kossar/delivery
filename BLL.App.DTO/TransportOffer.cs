using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BLL.App.DTO.Enums;
using BLL.App.DTO.Identity;
using Contracts.Domain.Base;
using Domain.Base;

namespace BLL.App.DTO
{
    public class TransportOffer : DomainEntityId, IDomainAppUserId, IDomainAppUser<AppUser>
    {
        
        [Display(Name = "Info", ResourceType = typeof(Resources.BLL.App.DTO.TransportOffer))]
        [MaxLength(1024)]
        public string? TransportOfferInfo { get; set; }
        

        [Display(Name = "TransportType", ResourceType = typeof(Resources.BLL.App.DTO.TransportOffer))]
        public ETransportType TransportType { get; set; }
        

        [Display(Name = "Price", ResourceType = typeof(Resources.BLL.App.DTO.TransportOffer))]
        public decimal Price { get; set; }
        

        [Display(Name = "AvailableLoadCapacity", ResourceType = typeof(Resources.BLL.App.DTO.TransportOffer))]
        public int AvailableLoadCapacity { get; set; }
        
        [Display(Name = "Unit", ResourceType = typeof(Resources.BLL.App.DTO.TransportOffer))]
        public Guid UnitId { get; set; }
        [Display(Name = "Unit", ResourceType = typeof(Resources.BLL.App.DTO.TransportOffer))]
        public Unit? Unit { get; set; }
        
        
        [Display(Name = "FreeSeats", ResourceType = typeof(Resources.BLL.App.DTO.TransportOffer))]
        public int FreeSeats { get; set; }
        
        [Display(Name = "_TransportMeta", ResourceType = typeof(Resources.BLL.App.DTO.TransportMeta))]
        public Guid TransportMetaId { get; set; }
        
        [Display(Name = "_TransportMeta", ResourceType = typeof(Resources.BLL.App.DTO.TransportMeta))]
        public TransportMeta? TransportMeta { get; set; }

        [Display(Name = "_Vehicle", ResourceType = typeof(Resources.BLL.App.DTO.Vehicle))]
        public Guid VehicleId { get; set; }
        
        [Display(Name = "_Vehicle", ResourceType = typeof(Resources.BLL.App.DTO.Vehicle))]
        public Vehicle? Vehicle { get; set; }
        

        [Display(Name = "_Trailer", ResourceType = typeof(Resources.BLL.App.DTO.Trailer))]
        public Guid? TrailerId { get; set; }
        
        [Display(Name = "_Trailer", ResourceType = typeof(Resources.BLL.App.DTO.Trailer))]
        public Trailer? Trailer { get; set; }
        
        public Guid AppUserId { get; set; }
        public AppUser? AppUser { get; set; }

        public ICollection<Transport>? Transports { get; set; }
    }
}