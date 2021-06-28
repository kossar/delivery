using System;
using System.ComponentModel.DataAnnotations;

namespace PublicApi.DTO.v1
{
    public class LocationAdd
    {
        [MaxLength(64)] public string Country { get; set; } = default!;
        
        [MaxLength(64)] public string City { get; set; } = default!;

        [MaxLength(128)] public string Address { get; set; } = default!;
        [MaxLength(4096)] public string? LocationInfo { get; set; }
    }
    public class Location: LocationAdd
    {
        public Guid Id { get; set; }
        // public int TransportAdStartLocationCount { get; set; }
        // public int TransportAdDestinationLocationCount { get; set; }
        // public int PickUpLocationCount { get; set; }
    }
}