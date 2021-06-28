using System;
using System.ComponentModel.DataAnnotations;
using Contracts.Domain.Base;
using Domain.App.Identity;
using Domain.Base;

namespace Domain.App
{
    public class Trailer : DomainEntityId, IDomainAppUserId, IDomainAppUser<AppUser>
    {
        public int LoadCapacity { get; set; }

        [MaxLength(32)]
        public string RegNr { get; set; } = default!;
        
        public Guid UnitId { get; set; }
        public Unit? Unit { get; set; }
        
        public Guid DimensionsId { get; set; }
        public Dimensions? Dimensions { get; set; }

        public Guid AppUserId { get; set; }
        public AppUser? AppUser { get; set; }

    }
}