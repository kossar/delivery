using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Base;

namespace Domain.App
{
    public class VehicleType: DomainEntityId
    {
        public Guid VehicleTypeNameId { get; set; }
        [InverseProperty(nameof(AppLangString.VehicleTypeName))]
        public AppLangString? VehicleTypeName { get; set; }
        
        
        public bool ForGoods { get; set; }

        public ICollection<Vehicle>? Vehicles { get; set; }
    }
}