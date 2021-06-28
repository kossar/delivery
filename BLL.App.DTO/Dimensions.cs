using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Domain.Base;

namespace BLL.App.DTO
{
    public class Dimensions: DomainEntityId
    {
       
        [Display(Name = "Width", ResourceType = typeof(Resources.BLL.App.DTO.Dimensions))]
        public decimal Width { get; set; }
        
        [Display(Name = "Height", ResourceType = typeof(Resources.BLL.App.DTO.Dimensions))]
        public decimal Height { get; set; }
        
        [Display(Name = "Length", ResourceType = typeof(Resources.BLL.App.DTO.Dimensions))]
        public decimal Length { get; set; }

        [Display(Name = "_Unit", ResourceType = typeof(Resources.BLL.App.DTO.Unit))]
        public Guid UnitId { get; set; }
        
        [Display(Name = "_Unit", ResourceType = typeof(Resources.BLL.App.DTO.Unit))]
        public Unit? Unit { get; set; }

        public ICollection<Parcel>? Parcels { get; set; }
        public ICollection<Trailer>? Trailers { get; set; }

        [Display(Name = "Size", ResourceType = typeof(Resources.BLL.App.DTO.Dimensions))]
        public string Dim => Width + " x " + Height + " x " + Length;
    }
}