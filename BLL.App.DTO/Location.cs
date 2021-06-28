using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Domain.Base;

namespace BLL.App.DTO
{
    public class Location : DomainEntityId
    {
        [MaxLength(64)] 
        [Display(Name = "Country", ResourceType = typeof(Resources.BLL.App.DTO.Location))]
        public string Country { get; set; } = default!;
        
        [Display(Name = "City", ResourceType = typeof(Resources.BLL.App.DTO.Location))]
        [MaxLength(64)] 
        public string City { get; set; } = default!;
        
        [Display(Name = "Address", ResourceType = typeof(Resources.BLL.App.DTO.Location))]
        [MaxLength(128)] 
        public string Address { get; set; } = default!;
        
        [Display(Name = "Info", ResourceType = typeof(Resources.BLL.App.DTO.Location))]
        [MaxLength(4096)]
        public string? LocationInfo { get; set; }
        //
        // public ICollection<TransportMeta>? StartLocations { get; set; }
        // public ICollection<TransportMeta>? DestinationLocations { get; set; }
        //
        // public ICollection<Transport>? Transports { get; set; }
        
    }
}