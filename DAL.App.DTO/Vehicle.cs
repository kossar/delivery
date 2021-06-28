using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Contracts.Domain.Base;
using DAL.App.DTO.Identity;
using Domain.Base;

namespace DAL.App.DTO
{
    public class Vehicle : DomainEntityId, IDomainAppUserId, IDomainAppUser<AppUser>
    {
        [MaxLength(32)]
        public string Make { get; set; } = default!;
        
        [MaxLength(32)]
        public string Model { get; set; } = default!;
        
        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }

        [MaxLength(32)]
        public string RegNr { get; set; } = default!;

        public Guid VehicleTypeId { get; set; }
        public VehicleType? VehicleType { get; set; }

        public Guid AppUserId { get; set; }
        public AppUser? AppUser { get; set; }

        public Guid? OrganisationId { get; set; }
        public Organisation? Organisation { get; set; }

        public ICollection<TransportOffer>? TransportOffers { get; set; }
    }
}