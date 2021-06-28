using System;
using Contracts.Domain.Base;
using DAL.App.DTO.Identity;
using Domain.Base;

namespace DAL.App.DTO
{
    public class AppUserOrganisation: DomainEntityId, IDomainAppUserId, IDomainAppUser<AppUser>
    {
        public Guid AppUserId { get; set; }
        public AppUser? AppUser { get; set; }
        public Guid OrganisationId { get; set; }
        public Organisation? Organisation { get; set; }
    }
}