using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.App.Identity;
using Domain.Base;

namespace Domain.App
{
    public class Parcel : DomainEntityId
    {
        public int Weight { get; set; }
        
        public Guid? ParcelInfoId { get; set; }
        
        [InverseProperty(nameof(AppLangString.ParcelInfo))]
        public virtual AppLangString? ParcelInfo { get; set; }
        
        public Guid UnitId { get; set; }
        public Unit? Unit { get; set; }
        
        public Guid DimensionsId { get; set; }
        public Dimensions? Dimensions { get; set; }

        public Guid TransportNeedId { get; set; }
        public TransportNeed? TransportNeed { get; set; }
    }
}