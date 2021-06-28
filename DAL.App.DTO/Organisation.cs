using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Domain.Base;

namespace DAL.App.DTO
{
    public class Organisation: DomainEntityId
    {
        [MaxLength(64)]
        public string Name { get; set; } = default!;
        // public ICollection<DAL.App.DTO.AppUserOrganisation>? AppUserOrganisations { get; set; }
        // public ICollection<Domain.App.TransportNeed>? TransportNeeds { get; set; }
        // public ICollection<Domain.App.TransportOffer>? TransportOffers { get; set; }
        // public ICollection<Domain.App.Vehicle>? Vehicles { get; set; }
        // public ICollection<Domain.App.Trailer>? Trailers { get; set; }
        public int UserCount { get; set; }
        public int? TransportNeedCount { get; set; }
        public int? TransportOfferCount { get; set; }
        public int? VehicleCount { get; set; }
        public int? TrailerCount { get; set; }
    }
}