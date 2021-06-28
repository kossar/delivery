using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Base.Resources.Base.Domain.Identity;
using Domain.Base.Identity;
using Microsoft.AspNetCore.Identity;

namespace Domain.App.Identity
{
    public class AppUser: BaseUser<AppUserRole>
    {
        public ICollection<TransportNeed>? TransportNeeds { get; set; }
        public ICollection<TransportOffer>? TransportOffers { get; set; }
        
        public ICollection<Vehicle>? Vehicles { get; set; }
        public ICollection<Trailer>? Trailers { get; set; }

        //public ICollection<AppUserOrganisation>? AppUserOrganisations { get; set; }
        
    }
}