using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Domain.Base;

namespace DAL.App.DTO
{
    public class VehicleType: DomainEntityId
    {
        [MaxLength(32)]
        public string VehicleTypeName { get; set; } = default!;
        public bool ForGoods { get; set; }

        public ICollection<Vehicle>? Vehicles { get; set; }
    }
}