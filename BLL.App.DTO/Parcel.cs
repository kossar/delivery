using System;
using System.ComponentModel.DataAnnotations;
using BLL.App.DTO.Identity;
using Domain.Base;

namespace BLL.App.DTO
{
    public class Parcel : DomainEntityId
    {
        [Display(Name = "Weight", ResourceType = typeof(Resources.BLL.App.DTO.Parcel))]
        public int Weight { get; set; }
        
        [Display(Name = "Info", ResourceType = typeof(Resources.BLL.App.DTO.Parcel))]
        [MaxLength(1024)]
        public string? ParcelInfo { get; set; }
        
        [Display(Name = "_Unit", ResourceType = typeof(Resources.BLL.App.DTO.Unit))]
        public Guid UnitId { get; set; }
        
        [Display(Name = "_Unit", ResourceType = typeof(Resources.BLL.App.DTO.Unit))]
        public Unit? Unit { get; set; }
        
        [Display(Name = "_Dimension", ResourceType = typeof(Resources.BLL.App.DTO.Dimensions))]
        public Guid DimensionsId { get; set; }
        
        [Display(Name = "_Dimension", ResourceType = typeof(Resources.BLL.App.DTO.Dimensions))]
        public Dimensions? Dimensions { get; set; }

        [Display(Name = "_TransportNeed", ResourceType = typeof(Resources.BLL.App.DTO.TransportNeed))]
        public Guid TransportNeedId { get; set; }
        
        [Display(Name = "_TransportNeed", ResourceType = typeof(Resources.BLL.App.DTO.TransportNeed))]
        public TransportNeed? TransportNeed { get; set; }
    }
}