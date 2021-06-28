using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Domain.Base;

namespace BLL.App.DTO
{
    public class VehicleType: DomainEntityId
    {
        [Display(Name = "Name", ResourceType = typeof(Resources.BLL.App.DTO.VehicleType))]
        [MaxLength(32)]
        public string VehicleTypeName { get; set; } = default!;
        
        [Display(Name = "ForGoods", ResourceType = typeof(Resources.BLL.App.DTO.VehicleType))]
        public bool ForGoods { get; set; }

        public ICollection<Vehicle>? Vehicles { get; set; }
    }
}