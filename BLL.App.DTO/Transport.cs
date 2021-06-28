using System;
using System.ComponentModel.DataAnnotations;
using BLL.App.DTO.Enums;
using Domain.Base;

namespace BLL.App.DTO
{
    public class Transport : DomainEntityId
    {
        [Display(Name = "TransportStatus", ResourceType = typeof(Resources.BLL.App.DTO.Transport))]
        public ETransportStatus TransportStatus { get; set; }
        

        [Display(Name = "FinalPrice", ResourceType = typeof(Resources.BLL.App.DTO.Transport))]
        public decimal FinalPrice { get; set; }
        
        
        [Display(Name = "PickUpTime", ResourceType = typeof(Resources.BLL.App.DTO.Transport))]
        public DateTime PickUpTime { get; set; }
        

        [Display(Name = "EstimatedDeliveryTime", ResourceType = typeof(Resources.BLL.App.DTO.Transport))]
        public DateTime? EstimatedDeliveryTime { get; set; }
        

        [Display(Name = "DeliveredTime", ResourceType = typeof(Resources.BLL.App.DTO.Transport))]
        public DateTime? DeliveredTime { get; set; }
        
        public DateTime LastUpdateTime { get; set; } = DateTime.Now;
        
        public bool UpdatedByTransportOffer { get; set; }
        

        [Display(Name = "PickUpLocation", ResourceType = typeof(Resources.BLL.App.DTO.Transport))]
        public Guid PickUpLocationId { get; set; }
        
        [Display(Name = "PickUpLocation", ResourceType = typeof(Resources.BLL.App.DTO.Transport))]
        public Location? PickUpLocation { get; set; }
        
        
        [Display(Name = "_TransportNeed", ResourceType = typeof(Resources.BLL.App.DTO.TransportNeed))]
        public Guid TransportNeedId { get; set; }
        
        
        [Display(Name = "_TransportNeed", ResourceType = typeof(Resources.BLL.App.DTO.TransportNeed))]
        public TransportNeed? TransportNeed { get; set; }
        

        [Display(Name = "_TransportOffer", ResourceType = typeof(Resources.BLL.App.DTO.TransportOffer))]
        public Guid TransportOfferId { get; set; }
        
        
        [Display(Name = "_TransportOffer", ResourceType = typeof(Resources.BLL.App.DTO.TransportOffer))]
        public TransportOffer? TransportOffer { get; set; }
        
    }
}