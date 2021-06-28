using System;
using System.ComponentModel.DataAnnotations;
using BLL.App.DTO.Identity;
using Contracts.Domain.Base;
using Domain.Base;

namespace BLL.App.DTO
{
    public class Trailer : DomainEntityId, IDomainAppUserId, IDomainAppUser<AppUser>
    {
        [Display(Name = "LoadCapacity", ResourceType = typeof(Resources.BLL.App.DTO.Trailer))]
        public int LoadCapacity { get; set; }

        [Display(Name = "RegNr", ResourceType = typeof(Resources.BLL.App.DTO.Trailer))]
        [MaxLength(32)]
        public string RegNr { get; set; } = default!;
        
        [Display(Name = "_Unit", ResourceType = typeof(Resources.BLL.App.DTO.Unit))]
        public Guid UnitId { get; set; }
        
        [Display(Name = "_Unit", ResourceType = typeof(Resources.BLL.App.DTO.Unit))]
        public Unit? Unit { get; set; }
        
        [Display(Name = "_Dimension", ResourceType = typeof(Resources.BLL.App.DTO.Dimensions))]
        public Guid DimensionsId { get; set; }
        
        [Display(Name = "_Dimension", ResourceType = typeof(Resources.BLL.App.DTO.Dimensions))]
        public Dimensions? Dimensions { get; set; }

        public Guid AppUserId { get; set; }
        public AppUser? AppUser { get; set; }


    }
}