using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Domain.Base;

namespace DAL.App.DTO
{
    public class Dimensions: DomainEntityId
    {
        public decimal Width { get; set; }
        
        public decimal Height { get; set; }
        
        public decimal Length { get; set; }

        public Guid UnitId { get; set; }
        public Unit? Unit { get; set; }

        public ICollection<Parcel>? Parcels { get; set; }
        public ICollection<Trailer>? Trailers { get; set; }
    }
}