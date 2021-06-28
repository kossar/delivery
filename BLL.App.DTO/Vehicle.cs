using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BLL.App.DTO.Identity;
using Contracts.Domain.Base;
using Domain.Base;

namespace BLL.App.DTO
{
    public class Vehicle : DomainEntityId, IDomainAppUserId, IDomainAppUser<AppUser>
    {
        [Display(Name = "Make", ResourceType = typeof(Resources.BLL.App.DTO.Vehicle))]
        [MaxLength(32)]
        public string Make { get; set; } = default!;
        
        [Display(Name = "Model", ResourceType = typeof(Resources.BLL.App.DTO.Vehicle))]
        [MaxLength(32)]
        public string Model { get; set; } = default!;
        
        
        [Display(Name = "ReleaseDate", ResourceType = typeof(Resources.BLL.App.DTO.Vehicle))]
        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }

        [Display(Name = "RegNr", ResourceType = typeof(Resources.BLL.App.DTO.Vehicle))]
        [MaxLength(32)]
        public string RegNr { get; set; } = default!;

        [Display(Name = "_VehicleType", ResourceType = typeof(Resources.BLL.App.DTO.VehicleType))]
        public Guid VehicleTypeId { get; set; }
        
        [Display(Name = "_VehicleType", ResourceType = typeof(Resources.BLL.App.DTO.VehicleType))]
        public VehicleType? VehicleType { get; set; }

        public Guid AppUserId { get; set; }
        public AppUser? AppUser { get; set; }

        public ICollection<TransportOffer>? TransportOffers { get; set; }
        
        [Display(Name = "_Vehicle", ResourceType = typeof(Resources.BLL.App.DTO.Vehicle))]
        public string MakeModel => Make + " " + Model;

        [Display(Name = "Age", ResourceType = typeof(Resources.BLL.App.DTO.Vehicle))]
        public string Age =>  new DateTime(DateTime.Now.Subtract(ReleaseDate).Ticks).Year.ToString() + " " + 
                              Resources.BLL.App.DTO.Vehicle._Years;
    }
}