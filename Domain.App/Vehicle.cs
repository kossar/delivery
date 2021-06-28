using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Contracts.Domain.Base;
using Domain.App.Identity;
using Domain.Base;

namespace Domain.App
{
    public class Vehicle : DomainEntityId, IDomainAppUserId, IDomainAppUser<AppUser>
    {
        public Guid MakeId { get; set; }
        [InverseProperty(nameof(AppLangString.Make))]
        public AppLangString? Make { get; set; }
        
        public Guid ModelId { get; set; }
        [InverseProperty(nameof(AppLangString.Model))]
        public AppLangString? Model { get; set; }

        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }

        [MaxLength(32)]
        public string RegNr { get; set; } = default!;

        public Guid VehicleTypeId { get; set; }
        public VehicleType? VehicleType { get; set; }

        public Guid AppUserId { get; set; }
        public AppUser? AppUser { get; set; }

        public ICollection<TransportOffer>? TransportOffers { get; set; }
    }
}