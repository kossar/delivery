using System;
using System.ComponentModel.DataAnnotations;
using DAL.App.DTO.Identity;
using Domain.Base;

namespace DAL.App.DTO
{
    public class Parcel : DomainEntityId
    {
        public int Weight { get; set; }
        
        [MaxLength(1024)] public string? ParcelInfo { get; set; }
        
        public Guid UnitId { get; set; }
        public Unit? Unit { get; set; }
        
        public Guid DimensionsId { get; set; }
        public Dimensions? Dimensions { get; set; }

        public Guid TransportNeedId { get; set; }
        public TransportNeed? TransportNeed { get; set; }
    }
}