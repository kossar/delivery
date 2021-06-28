using System;
using System.ComponentModel.DataAnnotations;
using Domain.Base;

namespace BLL.App.DTO
{
    public class TransportMeta : DomainEntityId
    {
        [Display(Name = "StartLocation", ResourceType = typeof(Resources.BLL.App.DTO.TransportMeta))]
        public Guid StartLocationId { get; set; }
        
        [Display(Name = "StartLocation", ResourceType = typeof(Resources.BLL.App.DTO.TransportMeta))]
        public Location? StartLocation { get; set; }
        

        [Display(Name = "DestinationLocation", ResourceType = typeof(Resources.BLL.App.DTO.TransportMeta))]
        public Guid DestinationLocationId { get; set; }
        
        [Display(Name = "DestinationLocation", ResourceType = typeof(Resources.BLL.App.DTO.TransportMeta))]
        public Location? DestinationLocation { get; set; }
        

        [Display(Name = "StartTime", ResourceType = typeof(Resources.BLL.App.DTO.TransportMeta))]
        public DateTime StartTime { get; set; }

        public TransportNeed? TransportNeed { get; set; }
        public TransportOffer? TransportOffer { get; set; }

        public string StartToDest => "From " + StartLocation?.City + " To: " + DestinationLocation?.City ;
    }
}