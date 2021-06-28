using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DAL.App.DTO.Enums;
using Domain.Base;

namespace DAL.App.DTO
{
    public class Unit : DomainEntityId
    {
        
        [MaxLength(8)]
        public string UnitCode { get; set; } = default!;
        
        [MaxLength(32)]
        public string UnitName { get; set; } = default!;
        
        public EUnitType UnitType { get; set; }

        public ICollection<Parcel>? Parcels { get; set; }
        public ICollection<Dimensions>? Dimensions { get; set; }
        public ICollection<Trailer>? Trailers { get; set; }
        public ICollection<Vehicle>? Vehicles { get; set; }
        public ICollection<TransportOffer>? TransportOffers { get; set; }
    }
}