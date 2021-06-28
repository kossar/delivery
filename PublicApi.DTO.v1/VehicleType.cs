using System;
using System.ComponentModel.DataAnnotations;

namespace PublicApi.DTO.v1
{
    public class VehicleTypeAdd
    {
        [MaxLength(32)]
        public string VehicleTypeName { get; set; } = default!;
        public bool ForGoods { get; set; }
    }

    public class VehicleType : VehicleTypeAdd
    {
        public Guid Id { get; set; }
    }
    
}