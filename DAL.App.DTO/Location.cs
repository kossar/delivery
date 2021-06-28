using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Domain.Base;

namespace DAL.App.DTO
{
    public class Location : DomainEntityId
    {
        [MaxLength(64)] public string Country { get; set; } = default!;
        
        [MaxLength(64)] public string City { get; set; } = default!;
        
        [MaxLength(128)] public string Address { get; set; } = default!;
        
        [MaxLength(4096)] public string? LocationInfo { get; set; }

        // public ICollection<TransportMeta>? StartLocations { get; set; }
        // public ICollection<TransportMeta>? DestinationLocations { get; set; }
        //
        // public ICollection<Transport>? Transports { get; set; }
        public int TransportAdStartLocationCount { get; set; }
        public int TransportAdDestinationLocationCount { get; set; }
        public int PickUpLocationCount { get; set; }
    }
}