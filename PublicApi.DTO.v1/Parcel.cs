using System;
using System.ComponentModel.DataAnnotations;

namespace PublicApi.DTO.v1
{
    public class ParcelAdd
    {
        public int Weight { get; set; }

        [MaxLength(1024)]
        public string? ParcelInfo { get; set; }
        
        public Guid UnitId { get; set; }
        
        public Guid DimensionsId { get; set; }

        public Guid TransportNeedId { get; set; }
    }

    public class Parcel
    {
        public Guid Id { get; set; }

        public int Weight { get; set; }

        [MaxLength(1024)]
        public string? ParcelInfo { get; set; }
        
        public Guid UnitId { get; set; }
        
        public string? UnitCode { get; set; }
        
        public Dimensions? Dimensions { get; set; }
    }
}