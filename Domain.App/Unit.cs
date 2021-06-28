using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.App.Enums;
using Domain.Base;

namespace Domain.App
{
    public class Unit : DomainEntityId
    {

        public Guid UnitCodeId { get; set; }
        [InverseProperty(nameof(AppLangString.UnitCode))]
        public AppLangString? UnitCode { get; set; }
        
        public Guid UnitNameId { get; set; }
        [InverseProperty(nameof(AppLangString.UnitName))]
        public AppLangString? UnitName { get; set; }

        public EUnitType UnitType { get; set; }

        public ICollection<Parcel>? Parcels { get; set; }
        public ICollection<Dimensions>? Dimensions { get; set; }
        public ICollection<Trailer>? Trailers { get; set; }
        public ICollection<Vehicle>? Vehicles { get; set; }
        public ICollection<TransportOffer>? TransportOffers { get; set; }
    }
}