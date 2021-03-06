using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Domain.App.Identity;
using Domain.Base.Identity;
using Microsoft.AspNetCore.Identity;
using AppUserRole = Bll.App.DTO.Identity.AppUserRole;

namespace BLL.App.DTO.Identity
{
    public class AppUser: BaseUser<AppUserRole>
    {
        public ICollection<TransportNeed>? TransportNeeds { get; set; }
        public ICollection<TransportOffer>? TransportOffers { get; set; }

        public ICollection<Vehicle>? Vehicles { get; set; }
        public ICollection<Trailer>? Trailers { get; set; }


        public ICollection<Parcel>? Parcels { get; set; }
    }
}